﻿using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.JsonConverters
{
    internal class JsonStringEnumMemberConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : struct, Enum
    {
        private class EnumInfo
        {
#pragma warning disable SA1401 // Fields should be private
            public string Name;
            public TEnum EnumValue;
            public ulong RawValue;
#pragma warning restore SA1401 // Fields should be private

            public EnumInfo(string name, TEnum enumValue, ulong rawValue)
            {
                Name = name;
                EnumValue = enumValue;
                RawValue = rawValue;
            }
        }

        private const BindingFlags EnumBindings = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        private const int MaximumAutoGrowthCacheSize = 64;

#if NETSTANDARD2_0
        private static readonly string[] s_Split = new string[] { ", " };
#endif

        private readonly bool _AllowIntegerValues;
        private readonly ulong? _DeserializationFailureFallbackValueRaw;
        private readonly TEnum? _DeserializationFailureFallbackValue;
        private readonly Type _EnumType;
        private readonly TypeCode _EnumTypeCode;
        private readonly bool _IsFlags;
        private readonly object _TransformedToRawCopyLockObject = new object();
        private readonly object _RawToTransformedCopyLockObject = new object();
        private Dictionary<TEnum, EnumInfo> _RawToTransformed;
        private Dictionary<string, EnumInfo> _TransformedToRaw;

        public JsonStringEnumMemberConverter(JsonStringEnumMemberConverterOptions options)
        {
            _EnumType = typeof(TEnum);

            JsonStringEnumMemberConverterOptions computedOptions
                = _EnumType.GetCustomAttribute<JsonStringEnumMemberConverterOptionsAttribute>(false)?.Options
                ?? options;

            _AllowIntegerValues = computedOptions?.AllowIntegerValues ?? true;
            _EnumTypeCode = Type.GetTypeCode(_EnumType);
            _IsFlags = _EnumType.IsDefined(typeof(FlagsAttribute), true);

            ulong? deserializationFailureFallbackValue = computedOptions?.ConvertedDeserializationFailureFallbackValue;

            string[] builtInNames = _EnumType.GetEnumNames();
            Array builtInValues = _EnumType.GetEnumValues();

            int numberOfBuiltInNames = builtInNames.Length;

            _RawToTransformed = new Dictionary<TEnum, EnumInfo>(numberOfBuiltInNames);
            _TransformedToRaw = new Dictionary<string, EnumInfo>(numberOfBuiltInNames);

            for (int i = 0; i < numberOfBuiltInNames; i++)
            {
                Enum enumValue = (Enum)builtInValues.GetValue(i);

                if (enumValue == null)
                    continue;

                ulong rawValue = JsonStringEnumMemberConverter.GetEnumValue(_EnumTypeCode, enumValue);

                string name = builtInNames[i];
                FieldInfo field = _EnumType.GetField(name, EnumBindings);

                if (field == null)
                    continue;

                string transformedName = field.GetCustomAttribute<EnumMemberAttribute>(true)?.Value ??
                                         field.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name ??
                                         computedOptions?.NamingPolicy?.ConvertName(name) ??
                                         name;

                if (!(enumValue is TEnum))
                    throw new NotSupportedException();

                if (deserializationFailureFallbackValue.HasValue && rawValue == deserializationFailureFallbackValue.Value)
                {
                    _DeserializationFailureFallbackValueRaw = deserializationFailureFallbackValue;
                    if (!_DeserializationFailureFallbackValue.HasValue)
                        _DeserializationFailureFallbackValue = (TEnum)enumValue;
                }

                TEnum typedValue = (TEnum)enumValue;
                _RawToTransformed[typedValue] = new EnumInfo(transformedName, typedValue, rawValue);
                _TransformedToRaw[transformedName] = new EnumInfo(name, typedValue, rawValue);
            }

            if (deserializationFailureFallbackValue.HasValue && !_DeserializationFailureFallbackValue.HasValue)
                throw new JsonException($"JsonStringEnumMemberConverter could not find a definition on Enum type {_EnumType} matching deserializationFailureFallbackValue '{deserializationFailureFallbackValue}'.");
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonTokenType token = reader.TokenType;

            if (token == JsonTokenType.String)
            {
                string enumString = reader.GetString() ?? throw new JsonException($"Unable to convert null value to Enum type {_EnumType.Name}");

                Dictionary<string, EnumInfo> transformedToRaw = _TransformedToRaw;

                // Case sensitive search attempted first.
                if (transformedToRaw.TryGetValue(enumString, out EnumInfo enumInfo))
                    return enumInfo.EnumValue;

                if (_IsFlags)
                    return ConvertFlagsStringValueToEnumValue(enumString, transformedToRaw);

                // Case insensitive search attempted second.
                foreach (KeyValuePair<string, EnumInfo> enumItem in transformedToRaw)
                {
                    if (string.Equals(enumItem.Key, enumString, StringComparison.OrdinalIgnoreCase))
                        return enumItem.Value.EnumValue;
                }

                return _DeserializationFailureFallbackValue ?? throw new JsonException($"Unable to convert value '{enumString}' to Enum type {_EnumType.Name}");
            }

            return ReadNumericEnumValue(ref reader, token);
        }

        public override TEnum ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonTokenType token = reader.TokenType;

            if (token != JsonTokenType.PropertyName)
                throw new JsonException($"Unable to convert value to Enum type {_EnumType.Name}");

            string enumString = reader.GetString();

            if (enumString == null)
                throw new JsonException($"Unable to convert value to Enum type {_EnumType.Name}");

            Dictionary<string, EnumInfo> transformedToRaw = _TransformedToRaw;

            // Case sensitive search attempted first.
            if (transformedToRaw.TryGetValue(enumString, out EnumInfo enumInfo))
                return enumInfo.EnumValue;

            // For Enums used a dictionary keys, numeric form is still a string eg { "0": "value" }.
            if (ulong.TryParse(enumString, out ulong numericValue))
            {
                if (!_AllowIntegerValues)
                {
                    if (_DeserializationFailureFallbackValue.HasValue)
                        return _DeserializationFailureFallbackValue.Value;

                    throw new JsonException($"Unable to convert value to Enum type {_EnumType.Name}");
                }
                else
                {
                    return (TEnum)Enum.ToObject(_EnumType, numericValue);
                }
            }

            if (_IsFlags)
                return ConvertFlagsStringValueToEnumValue(enumString, transformedToRaw);

            // Case insensitive search attempted second.
            foreach (KeyValuePair<string, EnumInfo> enumItem in transformedToRaw)
            {
                if (string.Equals(enumItem.Key, enumString, StringComparison.OrdinalIgnoreCase))
                    return enumItem.Value.EnumValue;
            }

            if (_DeserializationFailureFallbackValue.HasValue)
                return _DeserializationFailureFallbackValue.Value;

            throw new JsonException($"Unable to convert value '{enumString}' to Enum type {_EnumType.Name}");
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            Dictionary<TEnum, EnumInfo> rawToTransformed = _RawToTransformed;
            if (rawToTransformed.TryGetValue(value, out EnumInfo enumInfo))
            {
                writer.WriteStringValue(enumInfo.Name);
                return;
            }

            ulong rawValue = JsonStringEnumMemberConverter.GetEnumValue(_EnumTypeCode, value);

            if (_IsFlags
                && TryGetStringForFlagsEnumValue(value, rawValue, rawToTransformed, out string flagsValueString))
            {
                writer.WriteStringValue(flagsValueString);
                return;
            }

            if (!_AllowIntegerValues)
                throw new JsonException($"Enum type {_EnumType} does not have a mapping for integer value '{rawValue.ToString(CultureInfo.CurrentCulture)}'.");

            Span<byte> data = stackalloc byte[20];
            WriteNumericValueToSpan(rawValue, ref data);
            writer.WriteRawValue(data, skipInputValidation: true);
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            Dictionary<TEnum, EnumInfo> rawToTransformed = _RawToTransformed;
            if (rawToTransformed.TryGetValue(value, out EnumInfo enumInfo))
            {
                writer.WritePropertyName(enumInfo.Name);
                return;
            }

            ulong rawValue = JsonStringEnumMemberConverter.GetEnumValue(_EnumTypeCode, value);

            if (_IsFlags
                && TryGetStringForFlagsEnumValue(value, rawValue, rawToTransformed, out string flagsValueString))
            {
                writer.WritePropertyName(flagsValueString);
                return;
            }

            if (!_AllowIntegerValues)
                throw new JsonException($"Enum type {_EnumType} does not have a mapping for integer value '{rawValue.ToString(CultureInfo.CurrentCulture)}'.");

            Span<byte> data = stackalloc byte[20];
            WriteNumericValueToSpan(rawValue, ref data);
            writer.WritePropertyName(data);
        }

        private TEnum ConvertFlagsStringValueToEnumValue(string value, Dictionary<string, EnumInfo> transformedToRaw)
        {
            ulong calculatedValue = 0;

#if NETSTANDARD2_0
            string[] flagValues = value.Split(s_Split, StringSplitOptions.None);
#else
			string[] flagValues = value.Split(", ");
#endif
            foreach (string flagValue in flagValues)
            {
                // Case sensitive search attempted first.
                if (transformedToRaw.TryGetValue(flagValue, out EnumInfo enumInfo))
                {
                    calculatedValue |= enumInfo.RawValue;
                }
                else
                {
                    // Case insensitive search attempted second.
                    bool matched = false;
                    foreach (KeyValuePair<string, EnumInfo> enumItem in transformedToRaw)
                    {
                        if (string.Equals(enumItem.Key, flagValue, StringComparison.OrdinalIgnoreCase))
                        {
                            calculatedValue |= enumItem.Value.RawValue;
                            matched = true;
                            break;
                        }
                    }

                    if (!matched)
                    {
                        if (_DeserializationFailureFallbackValueRaw.HasValue)
                            calculatedValue |= _DeserializationFailureFallbackValueRaw.Value;
                        else
                            throw new JsonException($"Unable to convert value '{flagValue}' to Enum type {_EnumType.Name}");
                    }
                }
            }

            TEnum enumValue = (TEnum)Enum.ToObject(_EnumType, calculatedValue);
            if (transformedToRaw.Count < MaximumAutoGrowthCacheSize)
            {
                lock (_TransformedToRawCopyLockObject)
                {
                    if (!_TransformedToRaw.ContainsKey(value) && _TransformedToRaw.Count < MaximumAutoGrowthCacheSize)
                    {
                        Dictionary<string, EnumInfo> transformedToRawCopy = new Dictionary<string, EnumInfo>(_TransformedToRaw);
                        transformedToRawCopy[value] = new EnumInfo(value, enumValue, calculatedValue);
                        _TransformedToRaw = transformedToRawCopy;
                    }
                }
            }

            return enumValue;
        }

        private TEnum ReadNumericEnumValue(ref Utf8JsonReader reader, JsonTokenType tokenType)
        {
            if (tokenType != JsonTokenType.Number || !_AllowIntegerValues)
            {
                if (_DeserializationFailureFallbackValue.HasValue)
                {
                    reader.Skip();
                    return _DeserializationFailureFallbackValue.Value;
                }

                throw new JsonException($"Unable to convert value to Enum type {_EnumType.Name}");
            }

            switch (_EnumTypeCode)
            {
                case TypeCode.Int32:
                    if (reader.TryGetInt32(out int int32))
                    {
                        return (TEnum)Enum.ToObject(_EnumType, int32);
                    }
                    break;
                case TypeCode.Int64:
                    if (reader.TryGetInt64(out long int64))
                    {
                        return (TEnum)Enum.ToObject(_EnumType, int64);
                    }
                    break;
                case TypeCode.Int16:
                    if (reader.TryGetInt16(out short int16))
                    {
                        return (TEnum)Enum.ToObject(_EnumType, int16);
                    }
                    break;
                case TypeCode.Byte:
                    if (reader.TryGetByte(out byte ubyte8))
                    {
                        return (TEnum)Enum.ToObject(_EnumType, ubyte8);
                    }
                    break;
                case TypeCode.UInt32:
                    if (reader.TryGetUInt32(out uint uint32))
                    {
                        return (TEnum)Enum.ToObject(_EnumType, uint32);
                    }
                    break;
                case TypeCode.UInt64:
                    if (reader.TryGetUInt64(out ulong uint64))
                    {
                        return (TEnum)Enum.ToObject(_EnumType, uint64);
                    }
                    break;
                case TypeCode.UInt16:
                    if (reader.TryGetUInt16(out ushort uint16))
                    {
                        return (TEnum)Enum.ToObject(_EnumType, uint16);
                    }
                    break;
                case TypeCode.SByte:
                    if (reader.TryGetSByte(out sbyte byte8))
                    {
                        return (TEnum)Enum.ToObject(_EnumType, byte8);
                    }
                    break;
            }

            throw new JsonException($"Unable to convert value to Enum type {_EnumType.Name}");
        }

        private bool TryGetStringForFlagsEnumValue(
            TEnum value,
            ulong rawValue,
            Dictionary<TEnum, EnumInfo> rawToTransformed,
#if !NETSTANDARD2_0
			[NotNullWhen(true)]
#endif
            out string flagsValueString)
        {
            ulong calculatedValue = 0;

            StringBuilder Builder = new StringBuilder();
            foreach (KeyValuePair<TEnum, EnumInfo> enumItem in rawToTransformed)
            {
                EnumInfo enumInfo = enumItem.Value;
                if (!value.HasFlag(enumInfo.EnumValue)
                    || enumInfo.RawValue == 0) // Definitions with 'None' should hit the cache case.
                {
                    continue;
                }

                // Track the value to make sure all bits are represented.
                calculatedValue |= enumInfo.RawValue;

                if (Builder.Length > 0)
                    Builder.Append(", ");
                Builder.Append(enumInfo.Name);
            }

            if (calculatedValue == rawValue)
            {
                flagsValueString = Builder.ToString();
                if (rawToTransformed.Count < MaximumAutoGrowthCacheSize)
                {
                    lock (_RawToTransformedCopyLockObject)
                    {
                        if (!_RawToTransformed.ContainsKey(value) && _RawToTransformed.Count < MaximumAutoGrowthCacheSize)
                        {
                            Dictionary<TEnum, EnumInfo> rawToTransformedCopy = new Dictionary<TEnum, EnumInfo>(_RawToTransformed);
                            rawToTransformedCopy[value] = new EnumInfo(flagsValueString, value, rawValue);
                            _RawToTransformed = rawToTransformedCopy;
                        }
                    }
                }
                return true;
            }

            flagsValueString = null;
            return false;
        }

        private void WriteNumericValueToSpan(ulong rawValue, ref Span<byte> data)
        {
            int bytesWritten;

            switch (_EnumTypeCode)
            {
                case TypeCode.Int32:
                    Utf8Formatter.TryFormat((int)rawValue, data, out bytesWritten);
                    break;
                case TypeCode.Int64:
                    Utf8Formatter.TryFormat((long)rawValue, data, out bytesWritten);
                    break;
                case TypeCode.Int16:
                    Utf8Formatter.TryFormat((short)rawValue, data, out bytesWritten);
                    break;
                case TypeCode.Byte:
                    Utf8Formatter.TryFormat((byte)rawValue, data, out bytesWritten);
                    break;
                case TypeCode.UInt32:
                    Utf8Formatter.TryFormat((uint)rawValue, data, out bytesWritten);
                    break;
                case TypeCode.UInt64:
                    Utf8Formatter.TryFormat(rawValue, data, out bytesWritten);
                    break;
                case TypeCode.UInt16:
                    Utf8Formatter.TryFormat((ushort)rawValue, data, out bytesWritten);
                    break;
                case TypeCode.SByte:
                    Utf8Formatter.TryFormat((sbyte)rawValue, data, out bytesWritten);
                    break;
                default:
                    throw new JsonException(); // GetEnumValue should have already thrown.
            }

#if NETSTANDARD2_0
            data = data.Slice(0, bytesWritten);
#else
			data = data[..bytesWritten];
#endif
        }
    }
}
