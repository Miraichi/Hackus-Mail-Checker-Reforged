using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Hackus_Mail_Checker_Reforged.Properties
{
	// Token: 0x02000026 RID: 38
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0000619C File Offset: 0x0000439C
		internal Resources()
		{
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00006948 File Offset: 0x00004B48
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager(<Module>.smethod_6<string>(710408229), typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000016 RID: 22
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00006979 File Offset: 0x00004B79
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x04000072 RID: 114
		private static ResourceManager resourceMan;

		// Token: 0x04000073 RID: 115
		private static CultureInfo resourceCulture;
	}
}
