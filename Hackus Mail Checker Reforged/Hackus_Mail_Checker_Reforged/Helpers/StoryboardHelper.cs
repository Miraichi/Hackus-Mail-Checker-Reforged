using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Hackus_Mail_Checker_Reforged.Helpers
{
	// Token: 0x02000159 RID: 345
	public static class StoryboardHelper
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x0000C24C File Offset: 0x0000A44C
		public static void SetCompletedCommand(DependencyObject o, ICommand value)
		{
			o.SetValue(StoryboardHelper.CompletedCommandProperty, value);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0000C25A File Offset: 0x0000A45A
		public static ICommand GetCompletedCommand(DependencyObject o)
		{
			return (ICommand)o.GetValue(StoryboardHelper.CompletedCommandProperty);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0000C26C File Offset: 0x0000A46C
		public static void SetCompletedCommandParameter(DependencyObject o, object value)
		{
			o.SetValue(StoryboardHelper.CompletedCommandParameterProperty, value);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0000C27A File Offset: 0x0000A47A
		public static object GetCompletedCommandParameter(DependencyObject o)
		{
			return o.GetValue(StoryboardHelper.CompletedCommandParameterProperty);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0003B154 File Offset: 0x00039354
		private static void OnCompletedCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Storyboard sb = sender2 as Storyboard;
			if (sb != null)
			{
				sb.Completed += delegate(object sender, EventArgs e)
				{
					ICommand completedCommand = StoryboardHelper.GetCompletedCommand(sb);
					if (completedCommand != null && StoryboardHelper.<>c__DisplayClass6_0.smethod_0(completedCommand, StoryboardHelper.GetCompletedCommandParameter(sb)))
					{
						StoryboardHelper.<>c__DisplayClass6_0.smethod_1(completedCommand, StoryboardHelper.GetCompletedCommandParameter(sb));
					}
				};
			}
		}

		// Token: 0x0400051D RID: 1309
		public static readonly DependencyProperty CompletedCommandProperty = DependencyProperty.RegisterAttached(<Module>.smethod_3<string>(-529876295), typeof(ICommand), typeof(StoryboardHelper), new PropertyMetadata(null, new PropertyChangedCallback(StoryboardHelper.OnCompletedCommandChanged)));

		// Token: 0x0400051E RID: 1310
		public static readonly DependencyProperty CompletedCommandParameterProperty = DependencyProperty.RegisterAttached(<Module>.smethod_2<string>(-73098666), typeof(object), typeof(StoryboardHelper), new PropertyMetadata(null));
	}
}
