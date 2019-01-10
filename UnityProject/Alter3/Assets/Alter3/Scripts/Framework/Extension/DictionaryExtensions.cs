using System.Collections.Generic;

public static class DictionaryExtensions
{
	/// <summary>
	/// 同じキーがあった場合置き換えする
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="self"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> self,TKey key,TValue value)
	{
		if (self.ContainsKey(key))
		{
			self[key] = value;
		}
		else
		{
			self.Add(key, value);
		}
	}

	/// <summary>
	/// 同じキーがある場合はスキップ
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="self"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	public static void AddOrSkip<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue value)
	{
		if (self.ContainsKey(key))
		{
		}
		else
		{
			self.Add(key, value);
		}
	}

}

