using System;
using System.Collections.Generic;
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

        public static void AddFiles(this string filePath, List<(string, string)> list, bool overwrite = true)
        {
            using var file = new FileStream(filePath, FileMode.Open);
            using var archive = new ZipArchive(file, ZipArchiveMode.Update);
            for (var i = 0; i < list.Count; i++)
            {
                if (overwrite)
                {
                    var oldEntry = archive.GetEntry(list[i].Item2);
                    oldEntry?.Delete();
                }
                archive.CreateEntryFromFile(list[i].Item1, list[i].Item2);
            }
            archive.Dispose();
            file.Dispose();
        }
    }
}