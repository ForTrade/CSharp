using System;
using System.IO;
namespace SmartQuant
{
	public static class Installation
	{
		public static DirectoryInfo DataDir
		{
			get
			{
				return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SmartQuant Ltd\\OpenQuant 2014\\data");
			}
		}
		public static DirectoryInfo ConfigDir
		{
			get
			{
				return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SmartQuant Ltd\\OpenQuant 2014\\config");
			}
		}
	}
}
