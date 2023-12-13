using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Hackus_Mail_Checker_Reforged.Models.Enums;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x02000089 RID: 137
	internal class CheckerStateToVisibilityConverter : IValueConverter
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x0001ADE0 File Offset: 0x00018FE0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			CheckerState checkerState = (CheckerState)value;
			string a = (string)parameter;
			if (a == <Module>.smethod_4<string>(-1425906993))
			{
				if (checkerState != CheckerState.Closing)
				{
					if (checkerState != CheckerState.Stopped)
					{
						return Visibility.Collapsed;
					}
				}
				return Visibility.Visible;
			}
			if (!(a == <Module>.smethod_5<string>(2134532426)))
			{
				if (!(a == <Module>.smethod_2<string>(-1199341508)))
				{
					return DependencyProperty.UnsetValue;
				}
				if (checkerState != CheckerState.Paused)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
			else
			{
				if (checkerState == CheckerState.Running)
				{
					return Visibility.Visible;
				}
				return Visibility.Collapsed;
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000986A File Offset: 0x00007A6A
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
