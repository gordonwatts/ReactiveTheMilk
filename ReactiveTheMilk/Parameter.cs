using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NETFX_CORE
using System.Net;
#else
using System.Web;
#endif

namespace ReactiveTheMilk
{
	/// <summary>
	/// RTM APIパラメーター
	/// </summary>
	public class Parameter
	{
		public string Key { get; private set; }
		public string Value { get; set; }

		public Parameter(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}
	}

	public static class ParametersExtension
	{
		public static void Add(this ICollection<Parameter> parameters, string key, string value)
		{
			parameters.Add(new Parameter(key, value));
		}

		/// <summary>
		/// RTM APIパラメーターをPOSTデータ用の文字列にする
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static String ToPostData(this IEnumerable<Parameter> parameters)
		{
#if NETFX_CORE
            return parameters
                .Select(x => WebUtility.UrlEncode(x.Key) + "=" + WebUtility.UrlEncode(x.Value))
                .Join("&");
#else
            return parameters
				.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value))
				.Join("&");
#endif
		}
	}
}
