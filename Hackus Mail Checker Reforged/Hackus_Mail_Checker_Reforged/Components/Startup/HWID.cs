using System;
using System.Management;
using System.Threading.Tasks;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001B5 RID: 437
	internal static class HWID
	{
		// Token: 0x06000CAD RID: 3245 RVA: 0x00043778 File Offset: 0x00041978
		public static string Value()
		{
			/*
An exception occurred when decompiling this method (06000CAD)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String Hackus_Mail_Checker_Reforged.Components.Startup.HWID::Value()

 ---> System.ArgumentOutOfRangeException: Non-negative number required. (Parameter 'length')
   at System.Array.Copy(Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length, Boolean reliable)
   at System.Array.Copy(Array sourceArray, Array destinationArray, Int32 length)
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackSlot.ModifyStack(StackSlot[] stack, Int32 popCount, Int32 pushCount, ByteCode pushDefinition) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 48
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 387
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 269
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 112
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0000D88B File Offset: 0x0000BA8B
		public static void Start(Action callback)
		{
			Task.Run(delegate()
			{
				HWID.Value();
				callback();
			});
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x000437F4 File Offset: 0x000419F4
		private static string GetHexString(byte[] bt)
		{
			string text = string.Empty;
			for (int i = 0; i < bt.Length; i++)
			{
				byte b = bt[i];
				int num = (int)(b & 15);
				int num2 = b >> 4 & 15;
				if (num2 > 9)
				{
					text += ((char)(num2 - 10 + 65)).ToString();
				}
				else
				{
					text += num2.ToString();
				}
				if (num > 9)
				{
					text += ((char)(num - 10 + 65)).ToString();
				}
				else
				{
					text += num.ToString();
				}
				if (i + 1 != bt.Length && (i + 1) % 2 == 0)
				{
					text += <Module>.smethod_4<string>(-1365234867);
				}
			}
			return text;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x000438A8 File Offset: 0x00041AA8
		private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(wmiClass).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (managementObject[wmiMustBeTrue].ToString() == <Module>.smethod_4<string>(409997799) && text == "")
				{
					try
					{
						text = managementObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00043948 File Offset: 0x00041B48
		private static string identifier(string wmiClass, string wmiProperty)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(wmiClass).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (text == "")
				{
					try
					{
						text = managementObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x000439CC File Offset: 0x00041BCC
		private static string cpuId()
		{
			string text = HWID.identifier(<Module>.smethod_5<string>(1097749073), <Module>.smethod_4<string>(-292104512));
			if (text == "")
			{
				text = HWID.identifier(<Module>.smethod_3<string>(-2128932664), <Module>.smethod_3<string>(1042262064));
				if (text == "")
				{
					text = HWID.identifier(<Module>.smethod_5<string>(1097749073), <Module>.smethod_6<string>(844923117));
					if (text == "")
					{
						text = HWID.identifier(<Module>.smethod_5<string>(1097749073), <Module>.smethod_2<string>(-1574573988));
					}
					text += HWID.identifier(<Module>.smethod_6<string>(731774383), <Module>.smethod_3<string>(-81510504));
				}
			}
			return text;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x00043A8C File Offset: 0x00041C8C
		private static string biosId()
		{
			return HWID.identifier(<Module>.smethod_5<string>(870840646), <Module>.smethod_6<string>(-1192362829)) + HWID.identifier(<Module>.smethod_6<string>(1324481904), <Module>.smethod_2<string>(285962715)) + HWID.identifier(<Module>.smethod_3<string>(1363615289), <Module>.smethod_6<string>(-1290960574)) + HWID.identifier(<Module>.smethod_5<string>(870840646), <Module>.smethod_3<string>(490012975));
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00043B04 File Offset: 0x00041D04
		private static string diskId()
		{
			return HWID.identifier(<Module>.smethod_6<string>(337687770), <Module>.smethod_4<string>(-1143876719)) + HWID.identifier(<Module>.smethod_5<string>(122956572), <Module>.smethod_6<string>(-1192362829)) + HWID.identifier(<Module>.smethod_3<string>(-71873309), <Module>.smethod_6<string>(-253289966)) + HWID.identifier(<Module>.smethod_4<string>(-1497332034), <Module>.smethod_2<string>(-1778559655));
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00043B7C File Offset: 0x00041D7C
		private static string baseId()
		{
			return HWID.identifier(<Module>.smethod_6<string>(1375358378), <Module>.smethod_3<string>(1373252484)) + HWID.identifier(<Module>.smethod_6<string>(1375358378), <Module>.smethod_3<string>(480375780)) + HWID.identifier(<Module>.smethod_4<string>(1659724206), <Module>.smethod_4<string>(1843052335)) + HWID.identifier(<Module>.smethod_2<string>(-113885883), <Module>.smethod_3<string>(1051899259));
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0000D8AA File Offset: 0x0000BAAA
		private static string macId()
		{
			return HWID.identifier(<Module>.smethod_6<string>(-548778834), <Module>.smethod_2<string>(-413091305), <Module>.smethod_2<string>(1251582467));
		}

		// Token: 0x040006E4 RID: 1764
		public static string fingerPrint = string.Empty;
	}
}
