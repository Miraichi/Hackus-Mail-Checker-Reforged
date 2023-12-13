using System;
using System.Windows.Input;

namespace Hackus_Mail_Checker_Reforged.UI.Models
{
	// Token: 0x02000062 RID: 98
	public class RelayCommand : ICommand
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002C9 RID: 713 RVA: 0x00007D51 File Offset: 0x00005F51
		// (remove) Token: 0x060002CA RID: 714 RVA: 0x00007D59 File Offset: 0x00005F59
		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00007D61 File Offset: 0x00005F61
		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00007D77 File Offset: 0x00005F77
		public bool CanExecute(object parameter)
		{
			return this.canExecute == null || this.canExecute(parameter);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00007D8F File Offset: 0x00005F8F
		public void Execute(object parameter)
		{
			this.execute(parameter);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00007D9D File Offset: 0x00005F9D
		public static void Validate()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x040001A3 RID: 419
		private Action<object> execute;

		// Token: 0x040001A4 RID: 420
		private Func<object, bool> canExecute;
	}
}
