using System;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.UI.ViewModels
{
	// Token: 0x02000032 RID: 50
	internal class DurationMessageViewModel : BindableObject
	{
		// Token: 0x0600012E RID: 302 RVA: 0x000135EC File Offset: 0x000117EC
		public DurationMessageViewModel(TimeSpan duration)
		{
			string text = "";
			if (duration.Days > 0)
			{
				text += string.Format(<Module>.smethod_5<string>(639162535), duration.Days);
			}
			if (duration.Hours > 0)
			{
				text += string.Format(<Module>.smethod_2<string>(-619999406), duration.Hours);
			}
			if (duration.Minutes > 0)
			{
				text += string.Format(<Module>.smethod_4<string>(1390894875), duration.Minutes);
			}
			if (duration.Seconds > 0)
			{
				text += string.Format(<Module>.smethod_3<string>(-2140166618), duration.Seconds);
			}
			this.DurationMessage = text;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006C64 File Offset: 0x00004E64
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00006C6C File Offset: 0x00004E6C
		public string DurationMessage
		{
			get
			{
				return this._durationMessage;
			}
			set
			{
				this._durationMessage = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1646583575));
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000136BC File Offset: 0x000118BC
		public RelayCommand CloseCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._closeCommand) == null)
				{
					result = (this._closeCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.ClearFrame(FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x040000B6 RID: 182
		private string _durationMessage;

		// Token: 0x040000B7 RID: 183
		private RelayCommand _closeCommand;
	}
}
