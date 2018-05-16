using System.Collections.Generic;
using System.Linq;

namespace Utils
{
	public class QueryString
	{
		#region " Members "

		private readonly string _queryString;
		private readonly IDictionary<string, string> _queryItems;

		#endregion

		public string this[string key]
		{
			get
			{
				string item = null;

				if (_queryItems.ContainsKey(key))
				{
					item = _queryItems[key];
				}

				return item;
			}
		}

		public QueryString(string queryString)
		{
			_queryString = queryString.Replace("?", string.Empty);
			_queryItems = ParseItems(_queryString);
		}

		public override string ToString()
		{
			return _queryString;
		}

		private static IDictionary<string, string> ParseItems(string queryString)
		{
			var items = new Dictionary<string, string>();

			if (!string.IsNullOrWhiteSpace(queryString))
			{
				var itemsArray = queryString.Split('&');

				itemsArray.Where(i => i.Contains("=")).ToList()
						  .ForEach(i =>
						   {
							   var keyValue = i.Split('=');
							   items.Add(keyValue[0], keyValue[1]);
						   });
			}

			return items;
		}
	}
}
