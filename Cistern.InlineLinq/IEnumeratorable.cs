﻿namespace Cistern.InlineLinq
{
    public interface IEnumeratorable<T>
    {
        void Initialize();
        void Dispose();
        int? TryGetCount(out int? upperBound);
        bool TryGetSpan(out ReadOnlySpan<T> span);
        bool TryGetNext(out T current);
    }
}