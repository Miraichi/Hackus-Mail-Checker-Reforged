using System;
using System.Collections.Generic;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x02000018 RID: 24
	internal interface IConfigurationProvider
	{
		// Token: 0x0600006D RID: 109
		void Open();

		// Token: 0x0600006E RID: 110
		Server Find(string domain);

		// Token: 0x0600006F RID: 111
		Server Find(string domain, ProtocolType protocol);

		// Token: 0x06000070 RID: 112
		IEnumerable<Server> FindAll(string searchQuery, SearchServerType searchType);

		// Token: 0x06000071 RID: 113
		void Add(Server server);

		// Token: 0x06000072 RID: 114
		void Update(Server original, Server updated);

		// Token: 0x06000073 RID: 115
		void Remove(Server server);

		// Token: 0x06000074 RID: 116
		void Close();
	}
}
