using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x0200008A RID: 138
	public class ContentTypeConverter : IValueConverter
	{
		// Token: 0x060004CA RID: 1226 RVA: 0x0001AE74 File Offset: 0x00019074
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (text == null)
			{
				return DependencyProperty.UnsetValue;
			}
			if (text.ContainsIgnoreCase(<Module>.smethod_4<string>(-1861715738)))
			{
				return <Module>.smethod_6<string>(1419011345);
			}
			if (text.ContainsIgnoreCase(<Module>.smethod_5<string>(994824230)))
			{
				return <Module>.smethod_5<string>(-1057683735);
			}
			if (text.ContainsIgnoreCase(<Module>.smethod_5<string>(804872864)))
			{
				return <Module>.smethod_3<string>(-52400511);
			}
			if (text.ContainsIgnoreCase(<Module>.smethod_6<string>(-1245577822)))
			{
				return <Module>.smethod_4<string>(1639179179);
			}
			if (text.ContainsIgnoreCase(<Module>.smethod_2<string>(-250041601)))
			{
				return <Module>.smethod_5<string>(-334835332);
			}
			return <Module>.smethod_4<string>(-1338745244);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001AF2C File Offset: 0x0001912C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = parameter as string;
			if (text == null)
			{
				return DependencyProperty.UnsetValue;
			}
			return Enum.Parse(targetType, text);
		}
	}
}
