using System;
using System.Text;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000FE RID: 254
	public static class EncodingHelper
	{
		// Token: 0x060007D4 RID: 2004 RVA: 0x0002F744 File Offset: 0x0002D944
		public static Encoding ParseEncodingName(string name)
		{
			name = EncodingHelper.CleanEncodingName(name);
			Encoding result;
			try
			{
				if (!name.Contains(<Module>.smethod_2<string>(38391352)) && !name.Contains(<Module>.smethod_3<string>(-131491646)))
				{
					if (!name.Contains(<Module>.smethod_4<string>(1333282784)) && !name.Contains(<Module>.smethod_2<string>(-927228400)))
					{
						if (!name.Contains(<Module>.smethod_4<string>(810312290)))
						{
							if (!name.Contains(<Module>.smethod_4<string>(897474039)))
							{
								if (name.Contains(<Module>.smethod_4<string>(984635788)))
								{
									result = Encoding.GetEncoding(1252);
								}
								else if (!name.Contains(<Module>.smethod_6<string>(270226155)) && !name.Contains(<Module>.smethod_5<string>(-376161940)))
								{
									if (name.Contains(<Module>.smethod_3<string>(-1214854131)))
									{
										result = Encoding.GetEncoding(949);
									}
									else if (name.Contains(<Module>.smethod_4<string>(892665720)))
									{
										result = Encoding.GetEncoding(850);
									}
									else if (!name.Contains(<Module>.smethod_4<string>(-1736611707)))
									{
										if (name.Contains(<Module>.smethod_4<string>(-1649449958)))
										{
											result = Encoding.GetEncoding(874);
										}
										else if (!name.Contains(<Module>.smethod_4<string>(-1562288209)) && !name.Contains(<Module>.smethod_4<string>(544018724)))
										{
											if (!name.Contains(<Module>.smethod_2<string>(304957084)) && !name.Contains(<Module>.smethod_2<string>(1553462413)))
											{
												if (!name.Contains(<Module>.smethod_4<string>(-1910935205)) && !name.Contains(<Module>.smethod_5<string>(689632134)))
												{
													if (!name.Contains(<Module>.smethod_6<string>(1012407895)) && !name.Contains(<Module>.smethod_5<string>(-228333696)))
													{
														if (!name.Contains(<Module>.smethod_4<string>(1778708167)) && !name.Contains(<Module>.smethod_4<string>(1865869916)))
														{
															if (!name.Contains(<Module>.smethod_5<string>(1064368805)) && !name.Contains(<Module>.smethod_5<string>(-1368041892)))
															{
																if (!name.Contains(<Module>.smethod_5<string>(114611975)) && !name.Contains(<Module>.smethod_4<string>(1517222920)))
																{
																	if (!name.Contains(<Module>.smethod_4<string>(-1112054507)) && !name.Contains(<Module>.smethod_6<string>(1154962974)))
																	{
																		if (!name.Contains(<Module>.smethod_3<string>(953798278)) && !name.Contains(<Module>.smethod_2<string>(1852667835)))
																		{
																			if (!name.Contains(<Module>.smethod_4<string>(-1204024575)) && !name.Contains(<Module>.smethod_3<string>(1837037787)))
																			{
																				if (!name.Contains(<Module>.smethod_5<string>(2130162879)) && !name.Contains(<Module>.smethod_2<string>(-1743245441)))
																				{
																					result = Encoding.GetEncoding(name);
																				}
																				else
																				{
																					result = Encoding.GetEncoding(28591);
																				}
																			}
																			else
																			{
																				result = Encoding.GetEncoding(28592);
																			}
																		}
																		else
																		{
																			result = Encoding.GetEncoding(28593);
																		}
																	}
																	else
																	{
																		result = Encoding.GetEncoding(28594);
																	}
																}
																else
																{
																	result = Encoding.GetEncoding(28595);
																}
															}
															else
															{
																result = Encoding.GetEncoding(28596);
															}
														}
														else
														{
															result = Encoding.GetEncoding(28597);
														}
													}
													else
													{
														result = Encoding.GetEncoding(28598);
													}
												}
												else
												{
													result = Encoding.GetEncoding(28599);
												}
											}
											else
											{
												result = Encoding.GetEncoding(28603);
											}
										}
										else
										{
											result = Encoding.GetEncoding(28605);
										}
									}
									else
									{
										result = Encoding.GetEncoding(936);
									}
								}
								else
								{
									result = Encoding.UTF8;
								}
							}
							else
							{
								result = Encoding.ASCII;
							}
						}
						else
						{
							result = Encoding.GetEncoding(1250);
						}
					}
					else
					{
						result = Encoding.GetEncoding(932);
					}
				}
				else
				{
					result = Encoding.GetEncoding(20127);
				}
			}
			catch
			{
				result = Encoding.UTF8;
			}
			return result;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002FB78 File Offset: 0x0002DD78
		private static string CleanEncodingName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return string.Empty;
			}
			if (name.StartsWith(<Module>.smethod_3<string>(-1574690000)))
			{
				name = name.Replace(<Module>.smethod_4<string>(723150541), "");
			}
			if (name.EndsWith(<Module>.smethod_3<string>(151378935)))
			{
				name = name.Replace(<Module>.smethod_4<string>(-1465509822), "");
			}
			foreach (string text in EncodingHelper._replacements)
			{
				if (name.Contains(text))
				{
					name.Replace(text, string.Empty);
				}
			}
			return name.ToLower();
		}

		// Token: 0x040003F3 RID: 1011
		private static readonly string[] _replacements = new string[]
		{
			<Module>.smethod_6<string>(-324211151),
			<Module>.smethod_2<string>(204313908),
			<Module>.smethod_2<string>(-2045323715),
			<Module>.smethod_4<string>(1338091103),
			<Module>.smethod_5<string>(1111656108),
			<Module>.smethod_6<string>(820097393)
		};
	}
}
