using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Managers
{
	// Token: 0x02000077 RID: 119
	internal class StatisticsManager : BindableObject
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00009090 File Offset: 0x00007290
		public static StatisticsManager Instance
		{
			get
			{
				StatisticsManager result;
				if ((result = StatisticsManager._instance) == null)
				{
					result = (StatisticsManager._instance = new StatisticsManager());
				}
				return result;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x000090A6 File Offset: 0x000072A6
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x000090AE File Offset: 0x000072AE
		public ObservableCollection<RequestResult> RequestValues
		{
			get
			{
				return this._requestValues;
			}
			set
			{
				this._requestValues = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-622673930));
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x000090C7 File Offset: 0x000072C7
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x000090CF File Offset: 0x000072CF
		public ObservableCollection<KeyValuePair<string, string>> BadDetails
		{
			get
			{
				return this._badDetails;
			}
			set
			{
				this._badDetails = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-524153502));
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x000090E8 File Offset: 0x000072E8
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x000090F0 File Offset: 0x000072F0
		public ObservableCollection<KeyValuePair<string, string>> BlockedDetails
		{
			get
			{
				return this._blockedDetails;
			}
			set
			{
				this._blockedDetails = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1857312978));
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00009109 File Offset: 0x00007309
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x00009111 File Offset: 0x00007311
		public ObservableCollection<KeyValuePair<string, string>> ErrorDetails
		{
			get
			{
				return this._errorDetails;
			}
			set
			{
				this._errorDetails = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1081938445));
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0000912A File Offset: 0x0000732A
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00009132 File Offset: 0x00007332
		public int LoadedStrings
		{
			get
			{
				return this._loadedStrings;
			}
			set
			{
				this._loadedStrings = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1721576548));
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000914B File Offset: 0x0000734B
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x00009153 File Offset: 0x00007353
		public int CheckedStrings
		{
			get
			{
				return this._checkedStrings;
			}
			set
			{
				this._checkedStrings = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(359265667));
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0000916C File Offset: 0x0000736C
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x00009174 File Offset: 0x00007374
		public int LoadedProxy
		{
			get
			{
				return this._loadedProxy;
			}
			set
			{
				this._loadedProxy = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1854859414));
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0000918D File Offset: 0x0000738D
		public int GoodMailsCount
		{
			get
			{
				return this._goodMailsCount;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00009195 File Offset: 0x00007395
		public int FoundMailsCount
		{
			get
			{
				return this._foundMailsCount;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0000919D File Offset: 0x0000739D
		public int BadMailsCount
		{
			get
			{
				return this._badMailsCount;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x000091A5 File Offset: 0x000073A5
		public int TwoFactorMailsCount
		{
			get
			{
				return this._twoFactorMailsCount;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x000091AD File Offset: 0x000073AD
		public int MultipasswordMailsCount
		{
			get
			{
				return this._multipasswordMailsCount;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x000091B5 File Offset: 0x000073B5
		public int BlockedMailsCount
		{
			get
			{
				return this._blockedMailsCount;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x000091BD File Offset: 0x000073BD
		public int CaptchaMailsCount
		{
			get
			{
				return this._captchaMailsCount;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x000091C5 File Offset: 0x000073C5
		public int ErrorMailsCount
		{
			get
			{
				return this._errorMailsCount;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x000091CD File Offset: 0x000073CD
		public int NoHostMailsCount
		{
			get
			{
				return this._noHostMailsCount;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x000091D5 File Offset: 0x000073D5
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x000091DD File Offset: 0x000073DD
		public int MaxSpeed
		{
			get
			{
				return this._maxSpeed;
			}
			set
			{
				this._maxSpeed = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(2066578999));
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x000091F6 File Offset: 0x000073F6
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x000091FE File Offset: 0x000073FE
		public int Speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				this._speed = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-974305944));
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00009217 File Offset: 0x00007417
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x0000921F File Offset: 0x0000741F
		public int LastCheckedStrings
		{
			get
			{
				return this._lastCheckedStrings;
			}
			set
			{
				this._lastCheckedStrings = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1123165704));
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00009238 File Offset: 0x00007438
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00009240 File Offset: 0x00007440
		public string EstimatedCompletionTime
		{
			get
			{
				return this._estimatedCompletionTime;
			}
			set
			{
				this._estimatedCompletionTime = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(302291023));
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00009259 File Offset: 0x00007459
		public void IncrementChecked()
		{
			Interlocked.Increment(ref this._checkedStrings);
			base.OnPropertyChanged(<Module>.smethod_5<string>(1501890962));
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00009277 File Offset: 0x00007477
		public void IncrementGood()
		{
			Interlocked.Increment(ref this._goodMailsCount);
			base.OnPropertyChanged(<Module>.smethod_4<string>(1619333896));
			this.IncrementChecked();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000929B File Offset: 0x0000749B
		public void IncrementFound()
		{
			Interlocked.Increment(ref this._foundMailsCount);
			base.OnPropertyChanged(<Module>.smethod_3<string>(-947204654));
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000092B9 File Offset: 0x000074B9
		public void IncrementBad()
		{
			Interlocked.Increment(ref this._badMailsCount);
			base.OnPropertyChanged(<Module>.smethod_2<string>(-141226107));
			this.IncrementChecked();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000092DD File Offset: 0x000074DD
		public void IncrementTwoFactor()
		{
			Interlocked.Increment(ref this._twoFactorMailsCount);
			base.OnPropertyChanged(<Module>.smethod_3<string>(1943046932));
			this.IncrementChecked();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00009301 File Offset: 0x00007501
		public void IncrementMultipassword()
		{
			Interlocked.Increment(ref this._multipasswordMailsCount);
			base.OnPropertyChanged(<Module>.smethod_4<string>(-927590101));
			this.IncrementChecked();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00009325 File Offset: 0x00007525
		public void IncrementBlocked()
		{
			Interlocked.Increment(ref this._blockedMailsCount);
			base.OnPropertyChanged(<Module>.smethod_4<string>(389452772));
			this.IncrementChecked();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00009349 File Offset: 0x00007549
		public void IncrementError()
		{
			Interlocked.Increment(ref this._errorMailsCount);
			base.OnPropertyChanged(<Module>.smethod_6<string>(657801970));
			this.IncrementChecked();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000936D File Offset: 0x0000756D
		public void IncrementNoHost()
		{
			Interlocked.Increment(ref this._noHostMailsCount);
			base.OnPropertyChanged(<Module>.smethod_3<string>(-1428270772));
			this.IncrementChecked();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00009391 File Offset: 0x00007591
		public void IncrementCaptcha()
		{
			Interlocked.Increment(ref this._captchaMailsCount);
			base.OnPropertyChanged(<Module>.smethod_3<string>(1109854701));
			this.IncrementChecked();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001985C File Offset: 0x00017A5C
		public void Increment(OperationResult result)
		{
			switch (result)
			{
			case OperationResult.Ok:
				this.IncrementGood();
				return;
			case OperationResult.Bad:
				this.IncrementBad();
				return;
			case OperationResult.Error:
				this.IncrementError();
				return;
			case OperationResult.HttpError:
			case OperationResult.ReCaptcha:
				break;
			case OperationResult.Blocked:
				this.IncrementBlocked();
				return;
			case OperationResult.TwoFactor:
				this.IncrementTwoFactor();
				return;
			case OperationResult.Multipassword:
				this.IncrementMultipassword();
				return;
			case OperationResult.Captcha:
				this.IncrementCaptcha();
				break;
			case OperationResult.HostNotFound:
				this.IncrementNoHost();
				return;
			default:
				return;
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000198D0 File Offset: 0x00017AD0
		public void AddRequestValue(RequestResult result)
		{
			Func<RequestResult, bool> <>9__1;
			Application.Current.Dispatcher.Invoke(delegate()
			{
				IEnumerable<RequestResult> requestValues = this.RequestValues;
				Func<RequestResult, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((RequestResult r) => StatisticsManager.<>c__DisplayClass86_0.smethod_0(r.Request, result.Request)));
				}
				RequestResult requestResult = requestValues.FirstOrDefault(predicate);
				if (requestResult != null)
				{
					requestResult.Count += result.Count;
					return;
				}
				this.RequestValues.Add(result);
			});
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001990C File Offset: 0x00017B0C
		public void AddBadDetails(string address, string message)
		{
			Func<KeyValuePair<string, string>, bool> <>9__1;
			Application.Current.Dispatcher.Invoke(delegate()
			{
				IEnumerable<KeyValuePair<string, string>> badDetails = this.BadDetails;
				Func<KeyValuePair<string, string>, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((KeyValuePair<string, string> p) => StatisticsManager.<>c__DisplayClass87_0.smethod_0(p.Key, address)));
				}
				if (badDetails.Any(predicate))
				{
					return;
				}
				this.BadDetails.Add(new KeyValuePair<string, string>(address, message));
			});
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00019950 File Offset: 0x00017B50
		public void AddBlockedDetails(string address, string message)
		{
			Func<KeyValuePair<string, string>, bool> <>9__1;
			Application.Current.Dispatcher.Invoke(delegate()
			{
				IEnumerable<KeyValuePair<string, string>> blockedDetails = this.BlockedDetails;
				Func<KeyValuePair<string, string>, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((KeyValuePair<string, string> p) => StatisticsManager.<>c__DisplayClass88_0.smethod_0(p.Key, address)));
				}
				if (blockedDetails.Any(predicate))
				{
					return;
				}
				this.BlockedDetails.Add(new KeyValuePair<string, string>(address, message));
			});
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00019994 File Offset: 0x00017B94
		public void AddErrorDetails(string address, string message)
		{
			Func<KeyValuePair<string, string>, bool> <>9__1;
			Application.Current.Dispatcher.Invoke(delegate()
			{
				IEnumerable<KeyValuePair<string, string>> errorDetails = this.ErrorDetails;
				Func<KeyValuePair<string, string>, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((KeyValuePair<string, string> p) => StatisticsManager.<>c__DisplayClass89_0.smethod_0(p.Key, address)));
				}
				if (errorDetails.Any(predicate))
				{
					return;
				}
				this.ErrorDetails.Add(new KeyValuePair<string, string>(address, message));
			});
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000199D8 File Offset: 0x00017BD8
		public void ClearResults()
		{
			this._goodMailsCount = 0;
			this._foundMailsCount = 0;
			this._badMailsCount = 0;
			this._twoFactorMailsCount = 0;
			this._multipasswordMailsCount = 0;
			this._blockedMailsCount = 0;
			this._errorMailsCount = 0;
			this._noHostMailsCount = 0;
			this._captchaMailsCount = 0;
			this.CheckedStrings = 0;
			this.Speed = 0;
			this.MaxSpeed = 0;
			this.LastCheckedStrings = 0;
			this.RequestValues = new ObservableCollection<RequestResult>();
			this.BadDetails = new ObservableCollection<KeyValuePair<string, string>>();
			this.BlockedDetails = new ObservableCollection<KeyValuePair<string, string>>();
			this.ErrorDetails = new ObservableCollection<KeyValuePair<string, string>>();
			base.OnPropertyChanged(<Module>.smethod_2<string>(-7943241));
			base.OnPropertyChanged(<Module>.smethod_2<string>(2072898974));
			base.OnPropertyChanged(<Module>.smethod_5<string>(1839670572));
			base.OnPropertyChanged(<Module>.smethod_6<string>(212838883));
			base.OnPropertyChanged(<Module>.smethod_4<string>(-927590101));
			base.OnPropertyChanged(<Module>.smethod_3<string>(-2030567139));
			base.OnPropertyChanged(<Module>.smethod_5<string>(-439745820));
			base.OnPropertyChanged(<Module>.smethod_5<string>(1232859413));
			base.OnPropertyChanged(<Module>.smethod_3<string>(1109854701));
		}

		// Token: 0x04000245 RID: 581
		private static StatisticsManager _instance;

		// Token: 0x04000246 RID: 582
		private object _locker = new object();

		// Token: 0x04000247 RID: 583
		private ObservableCollection<RequestResult> _requestValues = new ObservableCollection<RequestResult>();

		// Token: 0x04000248 RID: 584
		private ObservableCollection<KeyValuePair<string, string>> _badDetails = new ObservableCollection<KeyValuePair<string, string>>();

		// Token: 0x04000249 RID: 585
		private ObservableCollection<KeyValuePair<string, string>> _blockedDetails = new ObservableCollection<KeyValuePair<string, string>>();

		// Token: 0x0400024A RID: 586
		private ObservableCollection<KeyValuePair<string, string>> _errorDetails = new ObservableCollection<KeyValuePair<string, string>>();

		// Token: 0x0400024B RID: 587
		private int _loadedStrings;

		// Token: 0x0400024C RID: 588
		private int _checkedStrings;

		// Token: 0x0400024D RID: 589
		private int _loadedProxy;

		// Token: 0x0400024E RID: 590
		private int _goodMailsCount;

		// Token: 0x0400024F RID: 591
		private int _foundMailsCount;

		// Token: 0x04000250 RID: 592
		private int _badMailsCount;

		// Token: 0x04000251 RID: 593
		private int _twoFactorMailsCount;

		// Token: 0x04000252 RID: 594
		private int _multipasswordMailsCount;

		// Token: 0x04000253 RID: 595
		private int _blockedMailsCount;

		// Token: 0x04000254 RID: 596
		private int _captchaMailsCount;

		// Token: 0x04000255 RID: 597
		private int _errorMailsCount;

		// Token: 0x04000256 RID: 598
		private int _noHostMailsCount;

		// Token: 0x04000257 RID: 599
		private int _maxSpeed;

		// Token: 0x04000258 RID: 600
		private int _speed;

		// Token: 0x04000259 RID: 601
		private int _lastCheckedStrings;

		// Token: 0x0400025A RID: 602
		private string _estimatedCompletionTime;
	}
}
