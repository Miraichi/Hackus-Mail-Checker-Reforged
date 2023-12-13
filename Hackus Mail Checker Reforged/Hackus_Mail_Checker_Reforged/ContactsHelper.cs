using System;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x02000012 RID: 18
	public static class ContactsHelper
	{
		// Token: 0x0600005F RID: 95 RVA: 0x0001050C File Offset: 0x0000E70C
		public static void AddContact(string contact)
		{
			/*
An exception occurred when decompiling this method (0600005F)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void Hackus_Mail_Checker_Reforged.ContactsHelper::AddContact(System.String)

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

		// Token: 0x0400003C RID: 60
		private static object _locker = new object();

		// Token: 0x0400003D RID: 61
		private static string[] _filters = new string[]
		{
			<Module>.smethod_2<string>(582270494),
			<Module>.smethod_4<string>(930432528),
			<Module>.smethod_2<string>(-1215686144),
			<Module>.smethod_2<string>(-832157391),
			<Module>.smethod_5<string>(-1861201589),
			<Module>.smethod_5<string>(1355010),
			<Module>.smethod_4<string>(49198400),
			<Module>.smethod_6<string>(1119045831),
			<Module>.smethod_5<string>(-568499088),
			<Module>.smethod_4<string>(-386610345),
			<Module>.smethod_5<string>(-1328304552),
			<Module>.smethod_6<string>(83105008),
			<Module>.smethod_6<string>(-2138250857),
			<Module>.smethod_3<string>(-987548601),
			<Module>.smethod_6<string>(-1700206910),
			<Module>.smethod_2<string>(334748713),
			<Module>.smethod_5<string>(-637247149),
			<Module>.smethod_3<string>(1340816701),
			<Module>.smethod_2<string>(-214702596),
			<Module>.smethod_6<string>(-662536302),
			<Module>.smethod_5<string>(-1776955345),
			<Module>.smethod_2<string>(-347985462),
			<Module>.smethod_5<string>(-294301478)
		};
	}
}
