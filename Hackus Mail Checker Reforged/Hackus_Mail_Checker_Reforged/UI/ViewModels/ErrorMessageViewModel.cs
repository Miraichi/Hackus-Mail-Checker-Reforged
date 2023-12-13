using System;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.UI.ViewModels
{
	// Token: 0x02000034 RID: 52
	internal class ErrorMessageViewModel : BindableObject
	{
		// Token: 0x06000135 RID: 309 RVA: 0x00006C91 File Offset: 0x00004E91
		public ErrorMessageViewModel()
		{
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006C99 File Offset: 0x00004E99
		public ErrorMessageViewModel(string message)
		{
			this.ErrorMessage = message;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006CA8 File Offset: 0x00004EA8
		public ErrorMessageViewModel(string message, Action afterClose)
		{
			this.ErrorMessage = message;
			this._actionAfterClose = afterClose;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00006CBE File Offset: 0x00004EBE
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00006CC6 File Offset: 0x00004EC6
		public string ErrorMessage
		{
			get
			{
				return this._errorMessage;
			}
			set
			{
				this._errorMessage = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-1011983820));
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00013704 File Offset: 0x00011904
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
						if (this._actionAfterClose != null)
						{
							this._actionAfterClose();
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x040000BA RID: 186
		private Action _actionAfterClose;

		// Token: 0x040000BB RID: 187
		private string _errorMessage;

		// Token: 0x040000BC RID: 188
		private RelayCommand _closeCommand;
	}
}
