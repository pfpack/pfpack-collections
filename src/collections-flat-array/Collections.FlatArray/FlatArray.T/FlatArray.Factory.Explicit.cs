﻿using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

partial class FlatArray<T>
{
    public static FlatArray<T> Empty
        =>
        InnerEmptyFlatArray.Value;

    public static FlatArray<T> From([AllowNull] params T[] source)
        =>
        source is not null ? InnerFromArray(source) : InnerEmptyFlatArray.Value;

    public static FlatArray<T> From([AllowNull] List<T> source)
        =>
        source is not null ? InnerFromList(source) : InnerEmptyFlatArray.Value;

    public static FlatArray<T> From([AllowNull] IEnumerable<T> source)
        =>
        source switch
        {
            null
            =>
            InnerEmptyFlatArray.Value,

            T[] array
            =>
            InnerFromArray(array),

            List<T> list
            =>
            InnerFromList(list),

            FlatArray<T> flatArray
            =>
            InnerFromFlatArray(flatArray),

            ImmutableArray<T> immutableArray
            =>
            InnerFromImmutableArray(immutableArray),

            ICollection<T> coll
            =>
            InnerFromICollection(coll),

            IReadOnlyList<T> list
            =>
            InnerFromIReadOnlyList(list),

            _ =>
            InnerFromIEnumerable(source)
        };

    private static FlatArray<T> InnerFromArray(T[] source)
        =>
        source.Length > 0 ? new(InnerCloneArray(source), default) : InnerEmptyFlatArray.Value;

    private static FlatArray<T> InnerFromList(List<T> source)
        =>
        InnerFromICollection(source);

    private static FlatArray<T> InnerFromFlatArray(FlatArray<T> source)
        =>
        source.items.Length > 0 ? new(InnerCloneArray(source), default) : InnerEmptyFlatArray.Value;

    private static FlatArray<T> InnerFromImmutableArray(ImmutableArray<T> source)
    {
        if (source.IsDefault)
        {
            return InnerEmptyFlatArray.Value;
        }

        var count = source.Length;
        if (count is not > 0)
        {
            return InnerEmptyFlatArray.Value;
        }

        var array = new T[count];
        source.CopyTo(array, 0);

        // Clone for the safety purposes
        return new(InnerCloneArray(array), default);
    }

    private static FlatArray<T> InnerFromICollection(ICollection<T> source)
    {
        var count = source.Count;
        if (count is not > 0)
        {
            return InnerEmptyFlatArray.Value;
        }

        var array = new T[count];
        source.CopyTo(array, 0);

        // Clone for the safety purposes
        return new(InnerCloneArray(array), default);
    }

    private static FlatArray<T> InnerFromIReadOnlyList(IReadOnlyList<T> source)
    {
        var count = source.Count;
        if (count is not > 0)
        {
            return InnerEmptyFlatArray.Value;
        }

        var array = new T[count];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = source[i];
        }

        return new(array, default);
    }

    private static FlatArray<T> InnerFromIEnumerable(IEnumerable<T> source)
    {
        using var enumerator = source.GetEnumerator();

        if (enumerator.MoveNext() is false)
        {
            return InnerEmptyFlatArray.Value;
        }

        int index = 0;
        var array = new T[4];

        do
        {
            if (index < array.Length)
            {
                array[index++] = enumerator.Current;
            }
            else
            {
                Array.Resize(ref array, array.Length * 2);
                array[index++] = enumerator.Current;
            }
        }
        while (enumerator.MoveNext());

        // Here the index is equal to the actual count
        Array.Resize(ref array, index);

        return new(array, default);
    }
}
