using System;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Hackus_Mail_Checker_Reforged.UI.Models
{
	// Token: 0x02000060 RID: 96
	public class PopupExtended : Popup
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x00007D0D File Offset: 0x00005F0D
		public PopupExtended()
		{
			base.Opened += this.Popupex_Opened;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000178CC File Offset: 0x00015ACC
		private void Popupex_Opened(object sender, EventArgs e)
		{
			DispatcherTimer time = new DispatcherTimer();
			time.Interval = TimeSpan.FromSeconds(0.5);
			time.Start();
			time.Tick += delegate(object sender, EventArgs e)
			{
				PopupExtended.<>c__DisplayClass1_0.smethod_0(this, false);
				PopupExtended.<>c__DisplayClass1_0.smethod_1(time);
			};
		}
	}
}
