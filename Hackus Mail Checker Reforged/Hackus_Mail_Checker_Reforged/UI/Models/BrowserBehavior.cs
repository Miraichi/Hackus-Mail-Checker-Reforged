using System;
using System.Windows;
using System.Windows.Controls;

namespace Hackus_Mail_Checker_Reforged.UI.Models
{
	// Token: 0x0200005F RID: 95
	public class BrowserBehavior
	{
		// Token: 0x060002BE RID: 702 RVA: 0x00007CB2 File Offset: 0x00005EB2
		[AttachedPropertyBrowsableForType(typeof(WebBrowser))]
		public static string GetHtml(WebBrowser d)
		{
			return (string)d.GetValue(BrowserBehavior.HtmlProperty);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00007CC4 File Offset: 0x00005EC4
		public static void SetHtml(WebBrowser d, string value)
		{
			d.SetValue(BrowserBehavior.HtmlProperty, value);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00017894 File Offset: 0x00015A94
		private static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			WebBrowser webBrowser = dependencyObject as WebBrowser;
			if (webBrowser != null)
			{
				webBrowser.NavigateToString((e.NewValue as string) ?? <Module>.smethod_4<string>(654522324));
			}
		}

		// Token: 0x040001A0 RID: 416
		public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(<Module>.smethod_4<string>(1202758427), typeof(string), typeof(BrowserBehavior), new FrameworkPropertyMetadata(new PropertyChangedCallback(BrowserBehavior.OnHtmlChanged)));
	}
}
