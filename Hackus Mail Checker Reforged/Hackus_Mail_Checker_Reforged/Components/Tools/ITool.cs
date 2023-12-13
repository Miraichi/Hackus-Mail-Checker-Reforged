using System;
using System.Threading.Tasks;

namespace Hackus_Mail_Checker_Reforged.Components.Tools
{
	// Token: 0x02000198 RID: 408
	public interface ITool
	{
		// Token: 0x06000C36 RID: 3126
		Task<bool> Run();

		// Token: 0x06000C37 RID: 3127
		void OpenDirectory();
	}
}
