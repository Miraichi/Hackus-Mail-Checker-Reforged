using System;
using System.ComponentModel;

namespace Hackus_Mail_Checker_Reforged.UI.Models
{
	// Token: 0x0200005D RID: 93
	public class BindableObject : INotifyPropertyChanged
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002B5 RID: 693 RVA: 0x00017820 File Offset: 0x00015A20
		// (remove) Token: 0x060002B6 RID: 694 RVA: 0x00017858 File Offset: 0x00015A58
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060002B7 RID: 695 RVA: 0x00007C3C File Offset: 0x00005E3C
		public void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
