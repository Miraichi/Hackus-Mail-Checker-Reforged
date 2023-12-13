using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Hackus_Mail_Checker_Reforged.Services.Background;
using Newtonsoft.Json;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001AD RID: 429
	internal static class Authenticator
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x00042A54 File Offset: 0x00040C54
		public static Task Login(string username, string password)
		{
			Authenticator.<Login>d__0 <Login>d__;
			<Login>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Login>d__.username = username;
			<Login>d__.password = password;
			<Login>d__.<>1__state = -1;
			<Login>d__.<>t__builder.Start<Authenticator.<Login>d__0>(ref <Login>d__);
			return <Login>d__.<>t__builder.Task;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00042AA0 File Offset: 0x00040CA0
		private static Task<EncryptionKeyResponse> GetEncryptionKeyAsync(string username)
		{
			Authenticator.<GetEncryptionKeyAsync>d__1 <GetEncryptionKeyAsync>d__;
			<GetEncryptionKeyAsync>d__.<>t__builder = AsyncTaskMethodBuilder<EncryptionKeyResponse>.Create();
			<GetEncryptionKeyAsync>d__.username = username;
			<GetEncryptionKeyAsync>d__.<>1__state = -1;
			<GetEncryptionKeyAsync>d__.<>t__builder.Start<Authenticator.<GetEncryptionKeyAsync>d__1>(ref <GetEncryptionKeyAsync>d__);
			return <GetEncryptionKeyAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00042AE4 File Offset: 0x00040CE4
		private static EncryptionKeyResponse GetEncryptionKey(string username)
		{
			/*
An exception occurred when decompiling this method (06000C8C)

ICSharpCode.Decompiler.DecompilerException: Error decompiling Hackus_Mail_Checker_Reforged.Components.Startup.EncryptionKeyResponse Hackus_Mail_Checker_Reforged.Components.Startup.Authenticator::GetEncryptionKey(System.String)

 ---> System.Exception: Inconsistent stack size at IL_A2
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 443
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 269
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 112
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00042C14 File Offset: 0x00040E14
		private static Task<string> GetLoginResponseAsync(string credentials, string guid)
		{
			Authenticator.<GetLoginResponseAsync>d__3 <GetLoginResponseAsync>d__;
			<GetLoginResponseAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<GetLoginResponseAsync>d__.credentials = credentials;
			<GetLoginResponseAsync>d__.guid = guid;
			<GetLoginResponseAsync>d__.<>1__state = -1;
			<GetLoginResponseAsync>d__.<>t__builder.Start<Authenticator.<GetLoginResponseAsync>d__3>(ref <GetLoginResponseAsync>d__);
			return <GetLoginResponseAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00042C60 File Offset: 0x00040E60
		private static string GetLoginResponse(string credentials, string guid)
		{
			/*
An exception occurred when decompiling this method (06000C8E)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String Hackus_Mail_Checker_Reforged.Components.Startup.Authenticator::GetLoginResponse(System.String,System.String)

 ---> System.Exception: Inconsistent stack size at IL_D1
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 443
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 269
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 112
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00042DBC File Offset: 0x00040FBC
		private static LoginResponse DecryptLoginResponse(string encryptedString, string serializedCredentials)
		{
			/*
An exception occurred when decompiling this method (06000C8F)

ICSharpCode.Decompiler.DecompilerException: Error decompiling Hackus_Mail_Checker_Reforged.Components.Startup.LoginResponse Hackus_Mail_Checker_Reforged.Components.Startup.Authenticator::DecryptLoginResponse(System.String,System.String)

 ---> System.Exception: Inconsistent stack size at IL_20
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 443
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 269
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 112
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00042DFC File Offset: 0x00040FFC
		private static Task<string> GetHWID()
		{
			Authenticator.<GetHWID>d__6 <GetHWID>d__;
			<GetHWID>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<GetHWID>d__.<>1__state = -1;
			<GetHWID>d__.<>t__builder.Start<Authenticator.<GetHWID>d__6>(ref <GetHWID>d__);
			return <GetHWID>d__.<>t__builder.Task;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00042E38 File Offset: 0x00041038
		public static Task<VersionResponse> GetLastVersionAsync()
		{
			Authenticator.<GetLastVersionAsync>d__7 <GetLastVersionAsync>d__;
			<GetLastVersionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<VersionResponse>.Create();
			<GetLastVersionAsync>d__.<>1__state = -1;
			<GetLastVersionAsync>d__.<>t__builder.Start<Authenticator.<GetLastVersionAsync>d__7>(ref <GetLastVersionAsync>d__);
			return <GetLastVersionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00042E74 File Offset: 0x00041074
		public static VersionResponse GetLastVersion()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			string text = string.Format(<Module>.smethod_3<string>(-483683895), version.Major, version.Minor, version.Build);
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					httpRequest.IgnoreProtocolErrors = true;
					httpRequest.Authorization = BackgroundAuthenticator.Instance.Token;
					httpRequest.AddUrlParam(<Module>.smethod_3<string>(-1045570179), text);
					HttpResponse httpResponse = httpRequest.Get(new Uri(BackgroundAuthenticator.Instance.BaseUri, <Module>.smethod_4<string>(327644369)), null);
					string text2 = httpResponse.ToString();
					if (httpResponse.StatusCode == 200)
					{
						VersionResponse versionResponse = JsonConvert.DeserializeObject<VersionResponse>(text2);
						if (versionResponse != null)
						{
							return versionResponse;
						}
					}
				}
			}
			catch
			{
			}
			return new VersionResponse
			{
				IsLastVersion = true
			};
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00042F74 File Offset: 0x00041174
		private static string EncryptRsa(string message, string key)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider(4096);
			StringReader textReader = new StringReader(key);
			RSAParameters parameters = (RSAParameters)new XmlSerializer(typeof(RSAParameters)).Deserialize(textReader);
			rsacryptoServiceProvider.ImportParameters(parameters);
			byte[] bytes = Encoding.Unicode.GetBytes(message);
			return Convert.ToBase64String(rsacryptoServiceProvider.Encrypt(bytes, false));
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00042FCC File Offset: 0x000411CC
		public static string EncryptAes(string message, string key, string IV = "H9CMLkbATaXW8Sgu")
		{
			byte[] key2 = Authenticator.CreateMD5(key);
			string result;
			using (Aes aes = Aes.Create())
			{
				aes.KeySize = 128;
				aes.BlockSize = 128;
				aes.Padding = PaddingMode.ISO10126;
				aes.Key = key2;
				aes.IV = Encoding.ASCII.GetBytes(IV);
				ICryptoTransform cryptoTransform = aes.CreateEncryptor();
				byte[] bytes = Encoding.UTF8.GetBytes(message);
				result = Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length));
			}
			return result;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0004305C File Offset: 0x0004125C
		public static string DecryptAes(string message, string key, string IV = "H9CMLkbATaXW8Sgu")
		{
			byte[] buffer = Convert.FromBase64String(message);
			byte[] key2 = Authenticator.CreateMD5(key);
			string result;
			using (Aes aes = Aes.Create())
			{
				aes.KeySize = 128;
				aes.BlockSize = 128;
				aes.Padding = PaddingMode.ISO10126;
				aes.Mode = CipherMode.CBC;
				aes.Key = key2;
				aes.IV = Encoding.ASCII.GetBytes(IV);
				ICryptoTransform transform = aes.CreateDecryptor();
				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader(cryptoStream))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0004314C File Offset: 0x0004134C
		private static byte[] CreateMD5(string message)
		{
			byte[] result;
			using (MD5 md = MD5.Create())
			{
				byte[] bytes = Encoding.ASCII.GetBytes(message);
				result = md.ComputeHash(bytes);
			}
			return result;
		}
	}
}
