namespace System.Collections.Generic
{
    internal interface IReadOnlyDictionary<TKey, TValue> : IEnumerable
    {
        bool ContainsKey(TKey key);
        ICollection<TKey> Keys { get; }
        ICollection<TValue> Values { get; }
        int Count { get; }
        bool TryGetValue(TKey key, out TValue value);
        TValue this[TKey key] { get; }
        bool Contains(KeyValuePair<TKey, TValue> item);
        void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex);
        new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
    }
}