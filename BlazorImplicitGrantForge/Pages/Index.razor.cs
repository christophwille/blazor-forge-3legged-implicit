using Autodesk.Forge;
using BlazorImplicitGrantForge.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorImplicitGrantForge.Pages
{
	public partial class Index
	{
		// https://forge.autodesk.com/en/docs/oauth/v2/tutorials/get-3-legged-token-implicit/
		private readonly string clientId = "";
		private readonly string scope = "data:read";

		private string authorizeUrl;

		public Index()
		{
			string redirectUri = WebUtility.UrlEncode("https://localhost:5001/oauthcallback");
			authorizeUrl = $"https://developer.api.autodesk.com/authentication/v1/authorize?response_type=token&client_id={clientId}&redirect_uri={redirectUri}&scope={scope}";
		}

		private bool authenticated;
		private string userInfo;
		protected override async Task OnInitializedAsync()
		{
			authenticated = TokenService.AccessToken != null;

			if (authenticated)
			{
				// userInfo = await GetNameFromProfileAsync();
				userInfo = await GetUserProfileJsonAsync();
			}
		}
		private async Task<string> GetUserProfileJsonAsync()
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenService.AccessToken);

			var response = await client.GetStringAsync("https://developer.api.autodesk.com/userprofile/v1/users/@me");

			return response;
		}

		/*
		 Unhandled exception rendering component: System.Net.Requests is not supported on this platform.
			System.PlatformNotSupportedException: System.Net.Requests is not supported on this platform.
			   at System.Net.WebRequest.get_DefaultWebProxy()
			   at RestSharp.RestClient.ConfigureHttp(IRestRequest request)
			   at RestSharp.RestClient.ExecuteAsync(IRestRequest request, Action`2 callback, String httpMethod, Func`4 getWebRequest)
			   at RestSharp.RestClient.ExecuteAsync(IRestRequest request, Action`2 callback, Method httpMethod)
			   at RestSharp.RestClient.ExecuteAsync(IRestRequest request, Action`2 callback)
			   at RestSharp.RestClient.ExecuteAsync(IRestRequest request, CancellationToken token)
			--- End of stack trace from previous location ---
			   at Autodesk.Forge.Client.ApiClient.CallApiAsync(String path, Method method, Dictionary`2 queryParams, Object postBody, Dictionary`2 headerParams, Dictionary`2 formParams, Dictionary`2 fileParams, Dictionary`2 pathParams, String contentType)
			   at Autodesk.Forge.UserProfileApi.GetUserProfileAsyncWithHttpInfo()
			   at Autodesk.Forge.UserProfileApi.GetUserProfileAsync()
			   at BlazorImplicitGrantForge.Pages.Index.GetNameFromProfileAsync() in d:\Demos\BlazorImplicitGrantForge\BlazorImplicitGrantForge\Pages\Index.razor.cs:line 42
			   at BlazorImplicitGrantForge.Pages.Index.OnInitializedAsync() in d:\Demos\BlazorImplicitGrantForge\BlazorImplicitGrantForge\Pages\Index.razor.cs:line 32
			   at Microsoft.AspNetCore.Components.ComponentBase.RunInitAndSetParametersAsync()
		*/
		private async Task<string> GetNameFromProfileAsync()
		{
			UserProfileApi userApi = new UserProfileApi();
			userApi.Configuration.AccessToken = TokenService.AccessToken;

			dynamic userProfile = await userApi.GetUserProfileAsync().ConfigureAwait(false);
			return string.Format("{0} {1}", userProfile.firstName, userProfile.lastName);
		}
	}
}
