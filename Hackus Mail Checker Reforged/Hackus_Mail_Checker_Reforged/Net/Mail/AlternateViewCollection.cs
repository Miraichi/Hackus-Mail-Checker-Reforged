using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000DC RID: 220
	public class AlternateViewCollection : Collection<Attachment>
	{
		// Token: 0x06000699 RID: 1689 RVA: 0x0002BBC0 File Offset: 0x00029DC0
		public IEnumerable<Attachment> OfType(string contentType)
		{
			contentType = (contentType ?? string.Empty).ToLower();
			return this.OfType((string x) => x.Is(contentType));
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0002BC08 File Offset: 0x00029E08
		public IEnumerable<Attachment> OfType(Func<string, bool> predicate)
		{
			return from x in this
			where predicate(AlternateViewCollection.<>c__DisplayClass1_0.smethod_0(x.ContentType ?? string.Empty))
			select x;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0002BC34 File Offset: 0x00029E34
		public Attachment GetHtmlView()
		{
			Attachment result;
			if ((result = this.OfType(<Module>.smethod_4<string>(1173908513)).FirstOrDefault<Attachment>()) == null)
			{
				result = this.OfType((string ct) => AlternateViewCollection.<>c.smethod_0(ct, <Module>.smethod_2<string>(1569658303))).FirstOrDefault<Attachment>();
			}
			return result;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002BC84 File Offset: 0x00029E84
		public Attachment GetTextView()
		{
			Attachment result;
			if ((result = this.OfType(<Module>.smethod_4<string>(384644453)).FirstOrDefault<Attachment>()) == null)
			{
				result = this.OfType((string ct) => AlternateViewCollection.<>c.smethod_1(ct, <Module>.smethod_5<string>(-683345321))).FirstOrDefault<Attachment>();
			}
			return result;
		}
	}
}
