namespace System.Collections.Generic
{
    internal class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        readonly IDictionary<TKey, TValue> _dictionary;
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }
        public bool ContainsKey(TKey key) { return _dictionary.ContainsKey(key); }
        public ICollection<TKey> Keys { get { return _dictionary.Keys; } }
        public bool TryGetValue(TKey key, out TValue value) { return _dictionary.TryGetValue(key, out value); }
        public ICollection<TValue> Values { get { return _dictionary.Values; } }
        public TValue this[TKey key] { get { return _dictionary[key]; } }
        public bool Contains(KeyValuePair<TKey, TValue> item) { return _dictionary.Contains(item); }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { _dictionary.CopyTo(array, arrayIndex); }
        public int Count { get { return _dictionary.Count; } }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() { return _dictionary.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _dictionary.GetEnumerator(); }
    }
}