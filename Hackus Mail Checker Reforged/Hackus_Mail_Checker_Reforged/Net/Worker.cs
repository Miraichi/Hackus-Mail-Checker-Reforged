using System;
using System.Linq;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000B7 RID: 183
	internal static class Worker
	{
		// Token: 0x060005D9 RID: 1497 RVA: 0x00027060 File Offset: 0x00025260
		public static void Run()
		{
			/*
An exception occurred when decompiling this method (060005D9)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void Hackus_Mail_Checker_Reforged.Net.Worker::Run()

 ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.IntroducePropertyAccessInstructions(ILExpression expr, ILExpression parentExpr, Int32 posInParent) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1587
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.IntroducePropertyAccessInstructions(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1579
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 244
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00027364 File Offset: 0x00025564
		private static bool IsBelongsTo(string[] domainsGroup, string domain)
		{
			return domainsGroup.Any((string d) => d.EqualsIgnoreCase(domain));
		}

		// Token: 0x0400030D RID: 781
		private static readonly string[] _onetDomains = new string[]
		{
			<Module>.smethod_4<string>(-78265922),
			<Module>.smethod_5<string>(508818119),
			<Module>.smethod_6<string>(274806776),
			<Module>.smethod_4<string>(-514074667),
			<Module>.smethod_2<string>(-1000828844),
			<Module>.smethod_2<string>(215036795),
			<Module>.smethod_2<string>(1879710567),
			<Module>.smethod_4<string>(-431721237),
			<Module>.smethod_5<string>(-1960549639),
			<Module>.smethod_6<string>(1461951603),
			<Module>.smethod_3<string>(180290520),
			<Module>.smethod_6<string>(-463915394)
		};

		// Token: 0x0400030E RID: 782
		private static readonly string[] _unitedInternetDomains = new string[]
		{
			<Module>.smethod_4<string>(51034421),
			<Module>.smethod_6<string>(-1797074870),
			<Module>.smethod_3<string>(-1224425190),
			<Module>.smethod_5<string>(1379494766),
			<Module>.smethod_5<string>(683669559),
			<Module>.smethod_2<string>(130713464),
			<Module>.smethod_6<string>(862324942)
		};

		// Token: 0x0400030F RID: 783
		private static readonly string[] _mailRuDomains = new string[]
		{
			<Module>.smethod_2<string>(-1667243174),
			<Module>.smethod_2<string>(-418737845),
			<Module>.smethod_6<string>(-469104749),
			<Module>.smethod_2<string>(2078272813),
			<Module>.smethod_5<string>(1532489071),
			<Module>.smethod_2<string>(280316175),
			<Module>.smethod_2<string>(1528821504),
			<Module>.smethod_2<string>(1912350257)
		};

		// Token: 0x04000310 RID: 784
		private static readonly string[] _yandexDomains = new string[]
		{
			<Module>.smethod_3<string>(20577627),
			<Module>.smethod_4<string>(-1564823974),
			<Module>.smethod_3<string>(1184760278),
			<Module>.smethod_3<string>(622873994)
		};

		// Token: 0x04000311 RID: 785
		private static readonly string[] _ramblerDomains = new string[]
		{
			<Module>.smethod_4<string>(-160619352),
			<Module>.smethod_6<string>(1161273380),
			<Module>.smethod_3<string>(-781841716),
			<Module>.smethod_6<string>(-1652790006),
			<Module>.smethod_2<string>(-250091183)
		};

		// Token: 0x04000312 RID: 786
		private static readonly string[] _protonDomains = new string[]
		{
			<Module>.smethod_4<string>(1137190245),
			<Module>.smethod_3<string>(-1022374775),
			<Module>.smethod_3<string>(-1584261059),
			<Module>.smethod_2<string>(-1765162244)
		};

		// Token: 0x04000313 RID: 787
		private static readonly string[] _outlookDomains = new string[]
		{
			<Module>.smethod_4<string>(1839292556),
			<Module>.smethod_4<string>(1050028496)
		};

		// Token: 0x04000314 RID: 788
		private static readonly string[] _interiaDomains = new string[]
		{
			<Module>.smethod_3<string>(463161101),
			<Module>.smethod_4<string>(1747322488),
			<Module>.smethod_5<string>(1199875522),
			<Module>.smethod_6<string>(-1065271840),
			<Module>.smethod_5<string>(440070058),
			<Module>.smethod_2<string>(-1781482089),
			<Module>.smethod_6<string>(1305558244),
			<Module>.smethod_6<string>(-27601232),
			<Module>.smethod_5<string>(-1807555334),
			<Module>.smethod_5<string>(434903997),
			<Module>.smethod_2<string>(-1183071245),
			<Module>.smethod_4<string>(91249257)
		};

		// Token: 0x04000315 RID: 789
		private static readonly string[] _seznamDomains = new string[]
		{
			<Module>.smethod_5<string>(-704804199),
			<Module>.smethod_5<string>(1157752400),
			<Module>.smethod_4<string>(619028070),
			<Module>.smethod_5<string>(-2034463761),
			<Module>.smethod_2<string>(-1033468534)
		};
	}
}
