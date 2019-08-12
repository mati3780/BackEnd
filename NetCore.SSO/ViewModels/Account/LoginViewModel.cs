using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCore.SSO.ViewModels.Account
{
	public class LoginViewModel : LoginInputViewModel
	{
		public bool AllowRememberLogin { get; set; }
		public bool EnableLocalLogin { get; set; }

		public IEnumerable<ExternalProviderViewModel> ExternalProviders { get; set; }
		public IEnumerable<ExternalProviderViewModel> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

		public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
		public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
	}
}