using System.Collections.Generic;

namespace IOTAplus.Utilities
{
	/// <summary>
	/// It's necessary to convert wallet data and other network responses to and from JSON.
	/// this static Json class provides such conversion tools.
	/// </summary>
	public static class Json
	{
		/// <summary>
		/// Receives a Dictionary<string, string> and returns a multiline JSON string.
		/// </summary>
		/// <param name="d">The dictionary to parse.</param>
		/// <returns>JSON string with line-breaks for human-readability.</returns>
		public static string DictionaryToJson (Dictionary<string, string> d)
		{
			string s = "{\n";
			foreach (var kvp in d)
			{
				s = string.Concat (s, "\"", kvp.Key, "\":\"", kvp.Value, "\",\n");
			}

			s = string.Concat (s.Substring (0, s.Length - 2), "\n}");
			return s;
		}

		/// <summary>
		/// Receives a List<Dictionary<string, string>> and returns a multiline JSON string.
		/// </summary>
		/// <param name="l">The list of dictionaries to parse.</param>
		/// <returns>JSON string with line-breaks for human-readability.</returns>
		public static string ListDictionaryToJson (List<Dictionary<string, string>> l)
		{
			string s = "{\n";

			foreach (var d in l)
			{
				s = string.Concat (s, DictionaryToJson (d), "\n");
			}
			
			s = string.Concat (s.Substring (0, s.Length - 1), "\n}");

			return s;
		}
	}
}