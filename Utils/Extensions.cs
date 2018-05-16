using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Utils
{
	public static class Extensions
	{
		public static T ValueOrDefault<T>(this T? instance) where T : struct
		{
			return instance ?? default(T);
		}

		public static object ValueOrNull<T>(this T? instance) where T : struct
		{
			return instance.HasValue ? (object)instance.Value : null;
		}

		public static void InitilizeDefaultPropertiesValues(this object instance)
		{
			if (instance != null)
			{
				var properties = instance.GetType().GetProperties();

				foreach (var property in properties)
				{
					var defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
					if (defaultValueAttribute != null)
					{
						property.SetValue(instance,
										  defaultValueAttribute.Value);
					}
				}
			}
		}

		public static string Remove(this string value,
									string toRemove)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}
			if (toRemove == null)
			{
				throw new ArgumentNullException(nameof(toRemove));
			}
			return value.Replace(toRemove,
								 string.Empty);
		}

		public static string Replace(this string value,
									 params char[] values)
		{
			return Replace(value,
						   values.Select(c => c.ToString()).ToArray());
		}

		public static string Replace(this string value,
									 params string[] values)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}
			if ((values.Length % 2) != 0)
			{
				throw new Exception($"{nameof(values)} length must be even");
			}

			var newValue = value;

			for (var i = 0; i < values.Length; i = i + 2)
			{
				newValue = newValue.Replace(values[i], values[i + 1]);
			}

			return newValue;
		}

		public static bool IsNullOrEmpty(this object instance)
		{
			var nullOrEmpty = false;

			if (instance == null)
			{
				nullOrEmpty = true;
			}
			else if (instance is string)
			{
				nullOrEmpty = string.IsNullOrEmpty(instance.ToString());
			}
			else
			{
				var propInfo = instance.GetType().GetProperties()
												 .FirstOrDefault(a => a.Name.ToLower() == "count" ||
																 a.Name.ToLower() == "length");

				if (propInfo != null)
				{
					object objValue = propInfo.GetValue(instance, null);

					if (ulong.TryParse(objValue.ToString(), out var lngCount))
					{
						nullOrEmpty = lngCount == 0;
					}
				}
			}

			return nullOrEmpty;
		}
	}
}
