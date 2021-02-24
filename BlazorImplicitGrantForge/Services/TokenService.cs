using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorImplicitGrantForge.Services
{
	public class TokenService
	{
		public static string AccessToken { get; set; }

		public bool ParseUri(string uri)
		{
			int fragmentLocation = uri.IndexOf('#');
			string fragment = uri.Substring(fragmentLocation + 1);
			string[] parts = fragment.Split('&');

			foreach (var part in parts)
			{
				string[] partParts = part.Split('=');
				switch (partParts[0])
				{
					case "access_token":
						AccessToken = partParts[1];
						break;
					case "token_type":
						// expected: Bearer
						break;
					case "expires_in":
						// expected: 86399
						break;
					default:
						break;
				}
			}

			return true;
		}
	}
}
