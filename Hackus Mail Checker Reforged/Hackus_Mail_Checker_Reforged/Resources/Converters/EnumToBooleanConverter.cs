using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x0200008B RID: 139
	public class EnumToBooleanConverter : IValueConverter
	{
		// Token: 0x060004CD RID: 1229 RVA: 0x0001AF50 File Offset: 0x00019150
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = parameter as string;
			if (text == null)
			{
				return DependencyProperty.UnsetValue;
			}
			if (!Enum.IsDefined(value.GetType(), value))
			{
				return DependencyProperty.UnsetValue;
			}
			return Enum.Parse(value.GetType(), text).Equals(value);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001AF2C File Offset: 0x0001912C
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
