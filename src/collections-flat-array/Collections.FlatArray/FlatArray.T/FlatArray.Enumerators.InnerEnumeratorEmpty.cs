﻿namespace System.Collections.Generic;

partial class FlatArray<T>
{
    private sealed class InnerEnumeratorEmpty : IEnumerator<T>
    {
        internal InnerEnumeratorEmpty() { }

        public bool MoveNext() => false;

        public T Current => throw new InvalidOperationException();

        object IEnumerator.Current => throw new InvalidOperationException();

        public void Reset() { }

        public void Dispose() { }
    }
}