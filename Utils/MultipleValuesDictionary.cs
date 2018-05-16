using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Utils
{
	[DefaultProperty("Items")]
	public class MultipleValuesDictionary<TKey, TValue>
	{
		private readonly List<KeyValuePair<TKey, TValue>> _items = new List<KeyValuePair<TKey, TValue>>();

		public IList<TKey> Keys => _items.Select(i => i.Key).Distinct().ToList();

		public IList<TValue> Items(TKey key)
		{
			return this[key];
		}

		public IList<TValue> this[TKey key] => _items.Where(i => i.Key.Equals(key))
													 .Select(i => i.Value).ToList();

		public void Add(TKey key, TValue value)
		{
			_items.Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		public void Remove(TKey key)
		{
			_items.RemoveAll((a) => a.Key.Equals(key));
		}

		public void Remove(TKey key,
						   TValue value)
		{
			_items.RemoveAll(a => a.Key.Equals(key) &&
								  a.Value.Equals(value));
		}

		public void Remove(KeyValuePair<TKey, TValue> item)
		{
			_items.Remove(item);
		}

		public void RemoveAll(Predicate<KeyValuePair<TKey, TValue>> match)
		{
			_items.RemoveAll(match);
		}

		public void RemoveAt(int index)
		{
			_items.RemoveAt(index);
		}
	}
}
