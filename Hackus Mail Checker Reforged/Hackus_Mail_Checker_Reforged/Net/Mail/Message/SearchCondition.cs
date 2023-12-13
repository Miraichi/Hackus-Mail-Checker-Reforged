using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Message
{
	// Token: 0x02000119 RID: 281
	public class SearchCondition
	{
		// Token: 0x060008B0 RID: 2224 RVA: 0x0000B333 File Offset: 0x00009533
		public static SearchCondition All()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.All)
			};
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0000B346 File Offset: 0x00009546
		public static SearchCondition Text(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Text),
				Value = text
			};
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0000B361 File Offset: 0x00009561
		public static SearchCondition BCC(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.BCC),
				Value = text
			};
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0000B37B File Offset: 0x0000957B
		public static SearchCondition Before(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Before),
				Value = date
			};
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0000B39A File Offset: 0x0000959A
		public static SearchCondition Body(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Body),
				Value = text
			};
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0000B3B4 File Offset: 0x000095B4
		public static SearchCondition Cc(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Cc),
				Value = text
			};
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0000B3CE File Offset: 0x000095CE
		public static SearchCondition From(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.From),
				Value = text
			};
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0000B3E9 File Offset: 0x000095E9
		public static SearchCondition Header(string name, string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Header),
				Quote = false,
				Value = name + <Module>.smethod_4<string>(-405843621) + text.QuoteString()
			};
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000B420 File Offset: 0x00009620
		public static SearchCondition Keyword(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Keyword),
				Value = text
			};
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0000B43B File Offset: 0x0000963B
		public static SearchCondition Larger(long size)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Larger),
				Value = size
			};
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0000B45B File Offset: 0x0000965B
		public static SearchCondition Smaller(long size)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Smaller),
				Value = size
			};
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0000B47B File Offset: 0x0000967B
		public static SearchCondition SentBefore(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.SentBefore),
				Value = date
			};
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0000B49B File Offset: 0x0000969B
		public static SearchCondition SentOn(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.SentOn),
				Value = date
			};
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0000B4BB File Offset: 0x000096BB
		public static SearchCondition SentSince(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.SentSince),
				Value = date
			};
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0000B4DB File Offset: 0x000096DB
		public static SearchCondition Since(DateTime date)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Since),
				Value = date
			};
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0000B4FB File Offset: 0x000096FB
		public static SearchCondition Subject(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Subject),
				Value = text
			};
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0000B516 File Offset: 0x00009716
		public static SearchCondition To(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.To),
				Value = text
			};
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0000B531 File Offset: 0x00009731
		public static SearchCondition UID(params uint[] uids)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.UID),
				Value = uids
			};
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x000352DC File Offset: 0x000334DC
		public static SearchCondition GreaterThan(uint uid)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.UID),
				Value = (uid + 1U).ToString() + <Module>.smethod_5<string>(-1652571624),
				Quote = false
			};
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00035324 File Offset: 0x00033524
		public static SearchCondition LessThan(uint uid)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.UID),
				Value = <Module>.smethod_3<string>(-1869059079) + (uid - 1U).ToString(),
				Quote = false
			};
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0000B54C File Offset: 0x0000974C
		public static SearchCondition Unkeyword(string text)
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Unkeyword),
				Value = text
			};
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0000B567 File Offset: 0x00009767
		public static SearchCondition Answered()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Answered)
			};
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0000B57A File Offset: 0x0000977A
		public static SearchCondition Deleted()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Deleted)
			};
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0000B58D File Offset: 0x0000978D
		public static SearchCondition Draft()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Draft)
			};
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0000B5A0 File Offset: 0x000097A0
		public static SearchCondition Flagged()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Flagged)
			};
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0000B5B3 File Offset: 0x000097B3
		public static SearchCondition New()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.New)
			};
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0000B5C7 File Offset: 0x000097C7
		public static SearchCondition Old()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Old)
			};
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0000B5DB File Offset: 0x000097DB
		public static SearchCondition Recent()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Recent)
			};
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0000B5EF File Offset: 0x000097EF
		public static SearchCondition Seen()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Seen)
			};
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0000B603 File Offset: 0x00009803
		public static SearchCondition Unanswered()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Unanswered)
			};
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0000B617 File Offset: 0x00009817
		public static SearchCondition Undeleted()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Undeleted)
			};
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0000B62B File Offset: 0x0000982B
		public static SearchCondition Undraft()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Undraft)
			};
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0000B63F File Offset: 0x0000983F
		public static SearchCondition Unflagged()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Unflagged)
			};
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0000B653 File Offset: 0x00009853
		public static SearchCondition Unseen()
		{
			return new SearchCondition
			{
				Field = new SearchCondition.Fields?(SearchCondition.Fields.Unseen)
			};
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0000B667 File Offset: 0x00009867
		public SearchCondition And(SearchCondition other)
		{
			return SearchCondition.Join(string.Empty, this, other);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0000B675 File Offset: 0x00009875
		public SearchCondition Not(SearchCondition other)
		{
			return SearchCondition.Join(<Module>.smethod_2<string>(-1128663498), this, other);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0000B688 File Offset: 0x00009888
		public SearchCondition Or(SearchCondition other)
		{
			return SearchCondition.Join(<Module>.smethod_6<string>(1722844771), this, other);
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0000B69B File Offset: 0x0000989B
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0000B6A3 File Offset: 0x000098A3
		private object Value { get; set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0000B6AC File Offset: 0x000098AC
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0000B6B4 File Offset: 0x000098B4
		private SearchCondition.Fields? Field { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0000B6BD File Offset: 0x000098BD
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x0000B6C5 File Offset: 0x000098C5
		private List<SearchCondition> Conditions { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0000B6CE File Offset: 0x000098CE
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x0000B6D6 File Offset: 0x000098D6
		private string Operator { get; set; }

		// Token: 0x060008DD RID: 2269 RVA: 0x0000B6DF File Offset: 0x000098DF
		private static SearchCondition Join(string condition, SearchCondition left, SearchCondition right)
		{
			return new SearchCondition
			{
				Operator = condition.ToUpper(),
				Conditions = new List<SearchCondition>
				{
					left,
					right
				}
			};
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0003536C File Offset: 0x0003356C
		private static string Join<T>(string seperator, IEnumerable<T> values)
		{
			IList<string> list = new List<string>();
			values.AsParallel<T>().ForAll(delegate(T x)
			{
				list.Add(x.ToString());
			});
			return string.Join(seperator, list.ToArray<string>());
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x000353B4 File Offset: 0x000335B4
		public override string ToString()
		{
			if (this.Conditions != null && this.Conditions.Count > 0 && this.Operator != null)
			{
				return (this.Operator.ToUpper() + <Module>.smethod_2<string>(536010274) + SearchCondition.Join<SearchCondition>(<Module>.smethod_5<string>(1965641681), this.Conditions) + <Module>.smethod_6<string>(90024148)).Trim();
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Field != null)
			{
				stringBuilder.Append(this.Field.ToString().ToUpper());
			}
			if (this.Value == null)
			{
				return stringBuilder.ToString();
			}
			object obj = this.Value;
			SearchCondition.Fields? field = this.Field;
			if (field != null)
			{
				SearchCondition.Fields valueOrDefault = field.GetValueOrDefault();
				if (valueOrDefault <= SearchCondition.Fields.Body)
				{
					if (valueOrDefault != SearchCondition.Fields.BCC && valueOrDefault != SearchCondition.Fields.Body)
					{
						goto IL_136;
					}
				}
				else if (valueOrDefault != SearchCondition.Fields.From && valueOrDefault - SearchCondition.Fields.Subject > 2)
				{
					goto IL_136;
				}
				string text = obj as string;
				if (text.IsAscii())
				{
					if (this.Quote)
					{
						obj = text.QuoteString();
					}
				}
				else
				{
					stringBuilder.AppendLine(<Module>.smethod_2<string>(-2037002651) + Encoding.UTF8.GetBytes(text).Length.ToString() + <Module>.smethod_6<string>(-1464243441));
				}
			}
			IL_136:
			if (!(obj is DateTime))
			{
				if (obj is uint[])
				{
					obj = SearchCondition.Join<uint>(<Module>.smethod_3<string>(-854885990), (uint[])obj);
				}
			}
			else
			{
				obj = ((DateTime)obj).GetRFC2060Date().QuoteString();
			}
			if (this.Field != null)
			{
				stringBuilder.Append(<Module>.smethod_2<string>(1849819774));
			}
			stringBuilder.Append(obj);
			return stringBuilder.ToString();
		}

		// Token: 0x04000441 RID: 1089
		private bool Quote = true;

		// Token: 0x0200011A RID: 282
		private enum Fields
		{
			// Token: 0x04000443 RID: 1091
			All,
			// Token: 0x04000444 RID: 1092
			Answered,
			// Token: 0x04000445 RID: 1093
			BCC,
			// Token: 0x04000446 RID: 1094
			Before,
			// Token: 0x04000447 RID: 1095
			Body,
			// Token: 0x04000448 RID: 1096
			Cc,
			// Token: 0x04000449 RID: 1097
			Deleted,
			// Token: 0x0400044A RID: 1098
			Draft,
			// Token: 0x0400044B RID: 1099
			Flagged,
			// Token: 0x0400044C RID: 1100
			From,
			// Token: 0x0400044D RID: 1101
			Header,
			// Token: 0x0400044E RID: 1102
			Keyword,
			// Token: 0x0400044F RID: 1103
			Larger,
			// Token: 0x04000450 RID: 1104
			New,
			// Token: 0x04000451 RID: 1105
			Not,
			// Token: 0x04000452 RID: 1106
			Old,
			// Token: 0x04000453 RID: 1107
			On,
			// Token: 0x04000454 RID: 1108
			Recent,
			// Token: 0x04000455 RID: 1109
			Seen,
			// Token: 0x04000456 RID: 1110
			SentBefore,
			// Token: 0x04000457 RID: 1111
			SentOn,
			// Token: 0x04000458 RID: 1112
			SentSince,
			// Token: 0x04000459 RID: 1113
			Since,
			// Token: 0x0400045A RID: 1114
			Smaller,
			// Token: 0x0400045B RID: 1115
			Subject,
			// Token: 0x0400045C RID: 1116
			Text,
			// Token: 0x0400045D RID: 1117
			To,
			// Token: 0x0400045E RID: 1118
			UID,
			// Token: 0x0400045F RID: 1119
			Unanswered,
			// Token: 0x04000460 RID: 1120
			Undeleted,
			// Token: 0x04000461 RID: 1121
			Undraft,
			// Token: 0x04000462 RID: 1122
			Unflagged,
			// Token: 0x04000463 RID: 1123
			Unkeyword,
			// Token: 0x04000464 RID: 1124
			Unseen
		}
	}
}
