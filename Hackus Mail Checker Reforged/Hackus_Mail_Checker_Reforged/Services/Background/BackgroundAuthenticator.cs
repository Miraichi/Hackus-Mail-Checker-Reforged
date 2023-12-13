using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Hackus_Mail_Checker_Reforged.Components.Startup;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Newtonsoft.Json;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Services.Background
{
	// Token: 0x0200007E RID: 126
	internal class BackgroundAuthenticator
	{
		// Token: 0x0600047E RID: 1150 RVA: 0x00009579 File Offset: 0x00007779
		private BackgroundAuthenticator()
		{
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00009596 File Offset: 0x00007796
		public static BackgroundAuthenticator Instance
		{
			get
			{
				BackgroundAuthenticator result;
				if ((result = BackgroundAuthenticator._instance) == null)
				{
					result = (BackgroundAuthenticator._instance = new BackgroundAuthenticator());
				}
				return result;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x000095AC File Offset: 0x000077AC
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x000095B4 File Offset: 0x000077B4
		public string Username { get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x000095BD File Offset: 0x000077BD
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x000095C5 File Offset: 0x000077C5
		public string Token { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x000095CE File Offset: 0x000077CE
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x000095D6 File Offset: 0x000077D6
		public string Key { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x000095DF File Offset: 0x000077DF
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x000095E7 File Offset: 0x000077E7
		public string DatabasePath { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x000095F0 File Offset: 0x000077F0
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x000095F8 File Offset: 0x000077F8
		public string DatabaseVersion { get; set; }

		// Token: 0x0600048A RID: 1162 RVA: 0x00009601 File Offset: 0x00007801
		public void SetProperties(string username, string token, string key, string databasePath, string databaseVersion)
		{
			this.Username = username;
			this.Token = token;
			this.Key = key;
			this.DatabasePath = databasePath;
			this.DatabaseVersion = databaseVersion;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001A074 File Offset: 0x00018274
		public void Start()
		{
			this._authDispatcher = new DispatcherTimer(DispatcherPriority.Background);
			this._authDispatcher.Interval = TimeSpan.FromMinutes(15.0);
			this._authDispatcher.Tick += this.UpdateToken;
			this._authDispatcher.Start();
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001A0C8 File Offset: 0x000182C8
		private void UpdateToken(object sender, EventArgs e)
		{
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					httpRequest.IgnoreProtocolErrors = true;
					httpRequest.Authorization = this.Token;
					FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>(<Module>.smethod_2<string>(1814480769), this.Token)
					}, false, null);
					string text = httpRequest.Post(new Uri(this.BaseUri, <Module>.smethod_4<string>(-728613001)), formUrlEncodedContent).ToString();
					if (!text.ContainsOne(new string[]
					{
						<Module>.smethod_5<string>(1833709877),
						<Module>.smethod_3<string>(-1097280352)
					}))
					{
						TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(text);
						this.Token = tokenResponse.Token;
					}
				}
			}
			catch (AuthenticatorException exception)
			{
				FileManager.LogUnhandledException(exception, <Module>.smethod_5<string>(-1548457650));
				Environment.Exit(0);
			}
			catch
			{
			}
		}

		// Token: 0x04000274 RID: 628
		private static BackgroundAuthenticator _instance;

		// Token: 0x04000275 RID: 629
		private DispatcherTimer _authDispatcher;

		// Token: 0x04000276 RID: 630
		public readonly Uri BaseUri = new Uri(<Module>.smethod_2<string>(-1645450326));
	}
}
