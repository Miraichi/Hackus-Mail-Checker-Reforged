using System;
using System.Windows;

namespace Hackus_Mail_Checker_Reforged.UI.Models
{
	// Token: 0x0200005E RID: 94
	public class BindingProxy : Freezable
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x00007C58 File Offset: 0x00005E58
		protected override Freezable CreateInstanceCore()
		{
			return new BindingProxy();
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00007C5F File Offset: 0x00005E5F
		// (set) Token: 0x060002BB RID: 699 RVA: 0x00007C6C File Offset: 0x00005E6C
		public object Data
		{
			get
			{
				return base.GetValue(BindingProxy.DataProperty);
			}
			set
			{
				base.SetValue(BindingProxy.DataProperty, value);
			}
		}

		// Token: 0x0400019F RID: 415
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(<Module>.smethod_2<string>(-742385848), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
	}
}
