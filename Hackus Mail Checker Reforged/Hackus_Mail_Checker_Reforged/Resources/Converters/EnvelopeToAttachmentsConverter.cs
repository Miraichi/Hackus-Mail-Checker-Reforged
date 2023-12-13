using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MailBee.ImapMail;

namespace Hackus_Mail_Checker_Reforged.Resources.Converters
{
	// Token: 0x0200008D RID: 141
	internal class EnvelopeToAttachmentsConverter : IValueConverter
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x0001B00C File Offset: 0x0001920C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ImapBodyStructure imapBodyStructure = value as ImapBodyStructure;
			if (imapBodyStructure == null)
			{
				return false;
			}
			using (IEnumerator enumerator = imapBodyStructure.GetAllParts().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					ImapBodyStructure imapBodyStructure2 = (ImapBodyStructure)obj;
					if ((imapBodyStructure2.Disposition != null && imapBodyStructure2.Disposition.ToLower() == <Module>.smethod_4<string>(1203370434)) || (imapBodyStructure2.Filename != null && imapBodyStructure2.Filename != string.Empty) || (imapBodyStructure2.ContentType != null && imapBodyStructure2.ContentType.ToLower() == <Module>.smethod_3<string>(1714078507)))
					{
						return true;
					}
				}
				goto IL_B7;
			}
			object result;
			return result;
			IL_B7:
			return false;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000986A File Offset: 0x00007A6A
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
