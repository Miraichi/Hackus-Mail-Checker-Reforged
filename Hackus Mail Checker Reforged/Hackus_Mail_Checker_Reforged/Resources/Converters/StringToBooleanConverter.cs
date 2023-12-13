using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x02000092 RID: 146
	internal class StringToBooleanConverter : IValueConverter
	{
		// Token: 0x060004E2 RID: 1250 RVA: 0x000098B9 File Offset: 0x00007AB9
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrWhiteSpace(value as string))
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000986A File Offset: 0x00007A6A
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
