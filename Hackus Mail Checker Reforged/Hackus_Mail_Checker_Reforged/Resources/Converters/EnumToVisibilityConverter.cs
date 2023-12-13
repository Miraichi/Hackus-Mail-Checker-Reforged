using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x0200008C RID: 140
	public class EnumToVisibilityConverter : IValueConverter
	{
		// Token: 0x060004D0 RID: 1232 RVA: 0x0001AF98 File Offset: 0x00019198
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = parameter as string;
			if (text == null)
			{
				return DependencyProperty.UnsetValue;
			}
			if (Enum.IsDefined(value.GetType(), value))
			{
				return Enum.Parse(value.GetType(), text).Equals(value) ? Visibility.Visible : Visibility.Collapsed;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001AFE8 File Offset: 0x000191E8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = parameter as string;
			if (text != null)
			{
				return Enum.Parse(targetType, text);
			}
			return DependencyProperty.UnsetValue;
		}
	}
}
