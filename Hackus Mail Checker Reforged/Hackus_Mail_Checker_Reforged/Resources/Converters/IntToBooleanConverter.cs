using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x0200008E RID: 142
	internal class IntToBooleanConverter : IValueConverter
	{
		// Token: 0x060004D6 RID: 1238 RVA: 0x00009871 File Offset: 0x00007A71
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((int)value > 0)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000986A File Offset: 0x00007A6A
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
