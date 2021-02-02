using System;
using System.IO;
using System.IO.Compression;

namespace Tools_Injector_Mod_Menu
{
    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (var zipArchiveEntry in archive.Entries)
            {
                var text = Path.Combine(destinationDirectoryName, zipArchiveEntry.FullName);
                var directoryName = Path.GetDirectoryName(text);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName ?? throw new InvalidOperationException());
                }
                if (zipArchiveEntry.Name != "")
                {
                    zipArchiveEntry.ExtractToFile(text, true);
                }
            }
        }
    }
}