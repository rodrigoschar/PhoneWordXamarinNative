using System;
using Core;
using System.IO;
namespace PhoneWordAndroid.DB
{
	public class PathManager : Core.DB.IPathManager
	{
        private static string sqliteFilename = "MyDatabase.db3";
        private static string libraryPath;

        public PathManager()
		{
		}

        public string GetPath()
        {
            libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(libraryPath, sqliteFilename);
            return path;
        }
    }
}

