using BlazorImplicitGrantForge.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorImplicitGrantForge.Pages
{
	public partial class OAuthCallback
	{
		[Inject] ILogger<OAuthCallback> Logger { get; set; }
		[Inject] NavigationManager NavManager { get; set; }

		private string info;
		protected override void OnInitialized()
		{
			var svc = new TokenService();
			bool parseOk = svc.ParseUri(NavManager.Uri);

			if (null != TokenService.AccessToken)
			{
				NavManager.NavigateTo("/");
			}

			info = NavManager.Uri;
		}
	}
}
