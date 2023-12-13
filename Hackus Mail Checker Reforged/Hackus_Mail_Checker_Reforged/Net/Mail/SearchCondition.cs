using System;
using System.Collections.Generic;
using System.Text;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000EB RID: 235
	public class SearchCondition
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0000A556 File Offset: 0x00008756
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x0000A55E File Offset: 0x0000875E
		public virtual SearchCondition.Fields? Field { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0000A567 File Offset: 0x00008767
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x0000A56F File Offset: 0x0000876F
		public virtual object Value { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0000A578 File Offset: 0x00008778
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0000A580 File Offset: 0x00008780
		internal List<SearchCondition> Conditions { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0000A589 File Offset: 0x00008789
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0000A591 File Offset: 0x00008791
		internal string Operator { get; set; }

		// Token: 0x0600071E RID: 1822 RVA: 0x0000A59A File Offset: 0x0000879A
		public static SearchCondition Answered()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Answered)
			};
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0000A5AE File Offset: 0x000087AE
		public static SearchCondition BCC(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.BCC),
				Value = text
			};
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0000A5C8 File Offset: 0x000087C8
		public static SearchCondition Before(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Before),
				Value = date
			};
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0000A5E7 File Offset: 0x000087E7
		public static SearchCondition Body(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Body),
				Value = text
			};
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0000A601 File Offset: 0x00008801
		public static SearchCondition Cc(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Cc),
				Value = text
			};
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0000A61B File Offset: 0x0000881B
		public static SearchCondition Deleted()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Deleted)
			};
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0000A62F File Offset: 0x0000882F
		public static SearchCondition Draft()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Draft)
			};
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0000A643 File Offset: 0x00008843
		public static SearchCondition Flagged()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Flagged)
			};
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0000A657 File Offset: 0x00008857
		public static SearchCondition From(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.From),
				Value = text
			};
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0000A671 File Offset: 0x00008871
		public static SearchCondition Header(string name, string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Header),
				Value = name + <Module>.smethod_3<string>(2023933234) + text.ToQuotedString()
			};
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0000A6A0 File Offset: 0x000088A0
		public static SearchCondition Keyword(string name, string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Keyword),
				Value = text
			};
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0000A6BA File Offset: 0x000088BA
		public static SearchCondition Larger(long size)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Larger),
				Value = size
			};
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0000A6D9 File Offset: 0x000088D9
		public static SearchCondition New()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.New)
			};
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0000A6ED File Offset: 0x000088ED
		public static SearchCondition Old()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Old)
			};
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0000A701 File Offset: 0x00008901
		public static SearchCondition operator &(SearchCondition a, SearchCondition b)
		{
			return a.And(new SearchCondition[]
			{
				b
			});
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000A713 File Offset: 0x00008913
		public static SearchCondition operator |(SearchCondition a, SearchCondition b)
		{
			return a.Or(new SearchCondition[]
			{
				b
			});
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0000A725 File Offset: 0x00008925
		public static SearchCondition Recent()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Recent)
			};
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000A739 File Offset: 0x00008939
		public static SearchCondition Seen()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Seen)
			};
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0000A74D File Offset: 0x0000894D
		public static SearchCondition SentBefore(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.SentBefore),
				Value = date
			};
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0000A76D File Offset: 0x0000896D
		public static SearchCondition SentOn(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.SentOn),
				Value = date
			};
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0000A78D File Offset: 0x0000898D
		public static SearchCondition SentSince(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.SentSince),
				Value = date
			};
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0000A7AD File Offset: 0x000089AD
		public static SearchCondition Smaller(long size)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Smaller),
				Value = size
			};
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0000A7CD File Offset: 0x000089CD
		public static SearchCondition Subject(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Subject),
				Value = text
			};
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0000A7E8 File Offset: 0x000089E8
		public static SearchCondition Text(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Text),
				Value = text
			};
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0000A803 File Offset: 0x00008A03
		public static SearchCondition To(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.To),
				Value = text
			};
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0000A81E File Offset: 0x00008A1E
		public static SearchCondition UID(string ids)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.UID),
				Value = ids
			};
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0000A839 File Offset: 0x00008A39
		public static SearchCondition Unanswered()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Unanswered)
			};
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0000A84D File Offset: 0x00008A4D
		public static SearchCondition Undeleted()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Undeleted)
			};
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0000A861 File Offset: 0x00008A61
		public static SearchCondition Undraft()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Undraft)
			};
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0000A875 File Offset: 0x00008A75
		public static SearchCondition Unflagged()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Unflagged)
			};
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0000A889 File Offset: 0x00008A89
		public static SearchCondition Unkeyword(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Unkeyword),
				Value = text
			};
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0000A8A4 File Offset: 0x00008AA4
		public static SearchCondition Unseen()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Unseen)
			};
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0000A8B8 File Offset: 0x00008AB8
		public virtual SearchCondition And(params SearchCondition[] other)
		{
			return SearchCondition.Join(string.Empty, this, other);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0000A8C6 File Offset: 0x00008AC6
		public virtual SearchCondition Not(params SearchCondition[] other)
		{
			return SearchCondition.Join(<Module>.smethod_3<string>(1854450874), this, other);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0000A8D9 File Offset: 0x00008AD9
		public virtual SearchCondition Or(params SearchCondition[] other)
		{
			return SearchCondition.Join(<Module>.smethod_6<string>(1722844771), this, other);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0002D398 File Offset: 0x0002B598
		public override string ToString()
		{
			if (this.Conditions != null && this.Conditions.Count > 0 && this.Operator != null)
			{
				return (this.Operator.ToUpper() + <Module>.smethod_5<string>(1775690315) + string.Join<SearchCondition>(<Module>.smethod_3<string>(1011621448), this.Conditions) + <Module>.smethod_5<string>(-726659454)).Trim();
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Field != null)
			{
				stringBuilder.Append(this.Field.ToString().ToUpper());
			}
			if (this.Value != null)
			{
				object obj = this.Value;
				SearchCondition.Fields? field = this.Field;
				if (field != null)
				{
					SearchCondition.Fields valueOrDefault = field.GetValueOrDefault();
					switch (valueOrDefault)
					{
					case SearchCondition.Fields.BCC:
					case SearchCondition.Fields.Body:
					case SearchCondition.Fields.From:
						break;
					case SearchCondition.Fields.Before:
					case SearchCondition.Fields.Cc:
						goto IL_EA;
					default:
						if (valueOrDefault - SearchCondition.Fields.Subject > 2)
						{
							goto IL_EA;
						}
						break;
					}
					obj = Convert.ToString(obj).ToQuotedString();
				}
				IL_EA:
				if (obj is DateTime)
				{
					obj = ((DateTime)obj).GetRFC2060Date().ToQuotedString();
				}
				if (this.Field != null)
				{
					stringBuilder.Append(<Module>.smethod_3<string>(2023933234));
				}
				stringBuilder.Append(obj);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0002D4D8 File Offset: 0x0002B6D8
		private static SearchCondition Join(string condition, SearchCondition left, params SearchCondition[] right)
		{
			condition = condition.ToUpper();
			if (left.Operator != condition)
			{
				left = new SearchCondition
				{
					Operator = condition,
					Conditions = new List<SearchCondition>
					{
						left
					}
				};
			}
			left.Conditions.AddRange(right);
			return left;
		}

		// Token: 0x020000EC RID: 236
		public enum Fields
		{
			// Token: 0x0400039A RID: 922
			BCC,
			// Token: 0x0400039B RID: 923
			Before,
			// Token: 0x0400039C RID: 924
			Body,
			// Token: 0x0400039D RID: 925
			Cc,
			// Token: 0x0400039E RID: 926
			From,
			// Token: 0x0400039F RID: 927
			Header,
			// Token: 0x040003A0 RID: 928
			Keyword,
			// Token: 0x040003A1 RID: 929
			Larger,
			// Token: 0x040003A2 RID: 930
			On,
			// Token: 0x040003A3 RID: 931
			SentBefore,
			// Token: 0x040003A4 RID: 932
			SentOn,
			// Token: 0x040003A5 RID: 933
			SentSince,
			// Token: 0x040003A6 RID: 934
			Since,
			// Token: 0x040003A7 RID: 935
			Smaller,
			// Token: 0x040003A8 RID: 936
			Subject,
			// Token: 0x040003A9 RID: 937
			Text,
			// Token: 0x040003AA RID: 938
			To,
			// Token: 0x040003AB RID: 939
			UID,
			// Token: 0x040003AC RID: 940
			Unkeyword,
			// Token: 0x040003AD RID: 941
			All,
			// Token: 0x040003AE RID: 942
			Answered,
			// Token: 0x040003AF RID: 943
			Deleted,
			// Token: 0x040003B0 RID: 944
			Draft,
			// Token: 0x040003B1 RID: 945
			Flagged,
			// Token: 0x040003B2 RID: 946
			New,
			// Token: 0x040003B3 RID: 947
			Old,
			// Token: 0x040003B4 RID: 948
			Recent,
			// Token: 0x040003B5 RID: 949
			Seen,
			// Token: 0x040003B6 RID: 950
			Unanswered,
			// Token: 0x040003B7 RID: 951
			Undeleted,
			// Token: 0x040003B8 RID: 952
			Undraft,
			// Token: 0x040003B9 RID: 953
			Unflagged,
			// Token: 0x040003BA RID: 954
			Unseen
		}
	}
}
