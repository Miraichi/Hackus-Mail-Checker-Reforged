using System;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels.Tabs
{
	// Token: 0x0200018C RID: 396
	internal class SettingsTabViewModel : BindableObject, IDisposable
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0000C607 File Offset: 0x0000A807
		public ViewerSettings ViewerSettings
		{
			get
			{
				return ViewerSettings.Instance;
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0000D0DD File Offset: 0x0000B2DD
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				this._isDisposed = true;
			}
		}

		// Token: 0x04000655 RID: 1621
		private bool _isDisposed;
	}
}
