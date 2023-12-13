using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x02000091 RID: 145
	public class StatisticsToSpeedValueConverter : IValueConverter
	{
		// Token: 0x060004DF RID: 1247 RVA: 0x0001B204 File Offset: 0x00019404
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (ThreadsManager.Instance.State == CheckerState.Stopped)
			{
				return 100;
			}
			if (StatisticsManager.Instance.MaxSpeed != 0)
			{
				return (double)StatisticsManager.Instance.Speed / (double)StatisticsManager.Instance.MaxSpeed * 100.0;
			}
			return 0;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000986A File Offset: 0x00007A6A
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
