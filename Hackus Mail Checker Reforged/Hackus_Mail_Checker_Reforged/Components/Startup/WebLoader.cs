using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001CB RID: 459
	internal class WebLoader : BindableObject
	{
		// Token: 0x06000D77 RID: 3447 RVA: 0x0000DD4E File Offset: 0x0000BF4E
		public WebLoader()
		{
			this._stopwatch = new Stopwatch();
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0000DD61 File Offset: 0x0000BF61
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x0000DD69 File Offset: 0x0000BF69
		public string Speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				this._speed = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-365831698));
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0000DD82 File Offset: 0x0000BF82
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x0000DD8A File Offset: 0x0000BF8A
		public double Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				this._progress = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-976444071));
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0000DDA3 File Offset: 0x0000BFA3
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x0000DDAB File Offset: 0x0000BFAB
		public bool IsError { get; set; }

		// Token: 0x06000D7E RID: 3454 RVA: 0x000458A4 File Offset: 0x00043AA4
		public Task Download(Uri uri, string filePath)
		{
			WebLoader.<Download>d__15 <Download>d__;
			<Download>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Download>d__.<>4__this = this;
			<Download>d__.uri = uri;
			<Download>d__.filePath = filePath;
			<Download>d__.<>1__state = -1;
			<Download>d__.<>t__builder.Start<WebLoader.<Download>d__15>(ref <Download>d__);
			return <Download>d__.<>t__builder.Task;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0000DDB4 File Offset: 0x0000BFB4
		private void Completed(object sender, AsyncCompletedEventArgs e)
		{
			this._stopwatch.Reset();
			if (e.Error != null)
			{
				this.IsError = true;
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x000458F8 File Offset: 0x00043AF8
		private void Changed(object sender, DownloadProgressChangedEventArgs e)
		{
			this.Progress = (double)e.ProgressPercentage;
			this.Speed = string.Format(<Module>.smethod_3<string>(486158097), ((double)e.BytesReceived / 1024.0 / this._stopwatch.Elapsed.TotalSeconds).ToString(<Module>.smethod_5<string>(1215375585)));
		}

		// Token: 0x04000736 RID: 1846
		private WebClient _client;

		// Token: 0x04000737 RID: 1847
		private Stopwatch _stopwatch;

		// Token: 0x04000738 RID: 1848
		private string _speed;

		// Token: 0x04000739 RID: 1849
		private double _progress;
	}
}
