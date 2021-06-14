namespace System.Collections.Generic
{
    internal static class DictionaryExtensions
    {
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> instance)
        {
            if (instance is null) throw new ArgumentNullException("instance");
            return new ReadOnlyDictionary<TKey, TValue>(instance);
        }
    }
}