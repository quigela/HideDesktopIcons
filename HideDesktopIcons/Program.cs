using System;
using System.IO;

namespace HideDesktopIcons
{
    class Program
    {
        static void Main(string[] args)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ToggleItems(Directory.GetFiles(desktop));
            ToggleItems(Directory.GetDirectories(desktop));
        }

        public static void ToggleItems(string[] items)
        {
            string[] BlockList = LoadBlocklist();
            foreach (string s in items)
            {
                if (OnBlocklist(s, BlockList))
                {
                    continue;
                }
                FileAttributes a = File.GetAttributes(s);
                bool isHidden = (a & FileAttributes.Hidden) == FileAttributes.Hidden;

                if (isHidden)
                {
                    FileInfo fi = new FileInfo(s);
                    fi.Attributes = FileAttributes.Normal;
                }
                else
                {
                    FileInfo fi = new FileInfo(s);
                    fi.Attributes = FileAttributes.Hidden;
                }

            }
        }

        private static bool OnBlocklist(string s, string[] BlockList)
        {
            bool fexists = Array.Exists(
                BlockList,
                delegate (string _) { return _.Equals(Path.GetFileName(s)); }
            );

            bool dexists = Array.Exists(
                BlockList,
                delegate (string _) { return _.Equals(Path.GetDirectoryName(s)); }
            );

            if (fexists || dexists)
            {
                return true;
            }
            return false;
        }

        private static string[] LoadBlocklist()
        {
            if (File.Exists("blocklist.txt"))
            {
                return File.ReadAllLines("blocklist.txt");
            }
            else
            {
                // return default list of only desktop.ini
                return new string[] { "desktop.ini" };
            }
        }
    }
}
