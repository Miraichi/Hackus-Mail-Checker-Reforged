using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Hackus_Mail_Checker_Reforged.Services.Managers;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x02000090 RID: 144
	public class StatisticsToProgressValueConverter : IValueConverter
	{
		// Token: 0x060004DC RID: 1244 RVA: 0x00009891 File Offset: 0x00007A91
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (double)StatisticsManager.Instance.CheckedStrings / (double)StatisticsManager.Instance.LoadedStrings * 100.0;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000986A File Offset: 0x00007A6A
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
