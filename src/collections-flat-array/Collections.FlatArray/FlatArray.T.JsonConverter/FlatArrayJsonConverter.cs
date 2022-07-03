using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.Collections.Generic;

internal sealed partial class FlatArrayJsonConverter<T> : JsonConverter<FlatArray<T>>
{
    private static Type ItemType => ItemTypeDefault.Value;

    private readonly JsonConverter<T>? itemConverter;

    public FlatArrayJsonConverter([AllowNull] JsonSerializerOptions options)
        =>
        itemConverter = (JsonConverter<T>?)options?.GetConverter(ItemType);

    private static class ItemTypeDefault
    {
        internal static readonly Type Value = typeof(T);
    }
}