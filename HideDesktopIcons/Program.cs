using System;
using System.IO;

namespace HideDesktopIcons
{
    class Program
    {
        private static string[] BlockList = { "desktop.ini" };
        static void Main(string[] args)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ToggleItems(Directory.GetFiles(desktop));
            ToggleItems(Directory.GetDirectories(desktop));
        }

        public static void ToggleItems(string[] items)
        {
            foreach (string s in items)
            {
                if (blocklist(s))
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

        private static bool blocklist(string s)
        {
            bool exists = Array.Exists(
                BlockList,
                delegate (string _) { return _.Equals(Path.GetFileName(s)); }
            );
            
            if (exists)
            {
                return true;
            }
            return false;
        }
    }
}
