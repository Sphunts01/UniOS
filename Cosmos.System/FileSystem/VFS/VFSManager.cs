﻿//#define COSMOSDEBUG

using global::System;
using global::System.Collections.Generic;
using global::System.IO;

using Cosmos.System.FileSystem.Listing;

namespace Cosmos.System.FileSystem.VFS
{
    public static class VFSManager
    {
        private static VFSBase mVFS;

        public static void RegisterVFS(VFSBase aVFS)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.RegisterVFS ---");
            if (mVFS != null)
            {
                throw new Exception("Virtual File System Manager already initialized!");
            }

            aVFS.Initialize();
            mVFS = aVFS;
        }

        public static DirectoryEntry CreateFile(string aPath)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.CreateFile ---");

            if (string.IsNullOrEmpty(aPath))
            {
                throw new ArgumentNullException(nameof(aPath));
            }

            Global.mFileSystemDebugger.SendInternal("aPath =");
            Global.mFileSystemDebugger.SendInternal(aPath);

            return mVFS.CreateFile(aPath);
        }

        public static DirectoryEntry GetFile(string aPath)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetFile ---");

            if (string.IsNullOrEmpty(aPath))
            {
                throw new ArgumentNullException(nameof(aPath));
            }

            Global.mFileSystemDebugger.SendInternal("aPath =");
            Global.mFileSystemDebugger.SendInternal(aPath);

            string xFileName = Path.GetFileName(aPath);
            Global.mFileSystemDebugger.SendInternal("xFileName =");
            Global.mFileSystemDebugger.SendInternal(xFileName);

            string xDirectory = aPath.Remove(aPath.Length - xFileName.Length);
            Global.mFileSystemDebugger.SendInternal("xDirectory =");
            Global.mFileSystemDebugger.SendInternal(xDirectory);

            char xLastChar = xDirectory[xDirectory.Length - 1];
            if (xLastChar != Path.DirectorySeparatorChar)
            {
                xDirectory = xDirectory + Path.DirectorySeparatorChar;
            }

            var xList = GetDirectoryListing(xDirectory);
            for (int i = 0; i < xList.Count; i++)
            {
                var xEntry = xList[i];
                if ((xEntry != null) && (xEntry.mEntryType == DirectoryEntryTypeEnum.File)
                    && (xEntry.mName.ToUpper() == xFileName.ToUpper()))
                {
                    return xEntry;
                }
            }

            return null;
        }

        public static DirectoryEntry CreateDirectory(string aPath)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.CreateDirectory ---");

            if (string.IsNullOrEmpty(aPath))
            {
                throw new ArgumentNullException(nameof(aPath));
            }

            Global.mFileSystemDebugger.SendInternal("aPath =");
            Global.mFileSystemDebugger.SendInternal(aPath);

            return mVFS.CreateDirectory(aPath);
        }

        public static DirectoryEntry GetDirectory(string aPath)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetDirectory ---");

            if (string.IsNullOrEmpty(aPath))
            {
                throw new ArgumentNullException(nameof(aPath));
            }

            Global.mFileSystemDebugger.SendInternal("aPath =");
            Global.mFileSystemDebugger.SendInternal(aPath);

            return mVFS.GetDirectory(aPath);
        }

        public static List<DirectoryEntry> GetDirectoryListing(string aPath)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetDirectoryListing ---");

            if (string.IsNullOrEmpty(aPath))
            {
                throw new ArgumentNullException(nameof(aPath));
            }

            Global.mFileSystemDebugger.SendInternal("aPath =");
            Global.mFileSystemDebugger.SendInternal(aPath);

            return mVFS.GetDirectoryListing(aPath);
        }

        public static DirectoryEntry GetVolume(string aVolume)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetVolume ---");

            if (string.IsNullOrEmpty(aVolume))
            {
                throw new ArgumentNullException(nameof(aVolume));
            }

            Global.mFileSystemDebugger.SendInternal("aVolume =");
            Global.mFileSystemDebugger.SendInternal(aVolume);

            return mVFS.GetVolume(aVolume);
        }

        public static List<DirectoryEntry> GetVolumes()
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetVolumes ---");

            return mVFS.GetVolumes();
        }

        public static List<string> GetLogicalDrives()
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetLogicalDrives ---");

            //TODO: Directory.GetLogicalDrives() will call this.
            return null;

            /*
            List<string> xDrives = new List<string>();
            foreach (FilesystemEntry entry in GetVolumes())
                xDrives.Add(entry.Name + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar);

            return xDrives.ToArray();
            */
        }

        public static List<string> InternalGetFileDirectoryNames(
            string path,
            string userPathOriginal,
            string searchPattern,
            bool includeFiles,
            bool includeDirs,
            SearchOption searchOption)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.InternalGetFileDirectoryNames ---");

            return null;

            /*
            //TODO: Add SearchOption functionality
            //TODO: What is userPathOriginal?
            //TODO: Add SearchPattern functionality

            List<string> xFileAndDirectoryNames = new List<string>();

            //Validate input arguments
            if ((searchOption != SearchOption.TopDirectoryOnly) && (searchOption != SearchOption.AllDirectories))
                throw new ArgumentOutOfRangeException("searchOption");

            searchPattern = searchPattern.TrimEnd(new char[0]);
            if (searchPattern.Length == 0)
                return new string[0];

            //Perform search in filesystem
            FilesystemEntry[] xEntries = GetDirectoryListing(path);

            foreach (FilesystemEntry xEntry in xEntries)
            {
                if (xEntry.IsDirectory && includeDirs)
                    xFileAndDirectoryNames.Add(xEntry.Name);
                else if (!xEntry.IsDirectory && includeFiles)
                    xFileAndDirectoryNames.Add(xEntry.Name);
            }

            return xFileAndDirectoryNames.ToArray();

             */
        }

        public static bool FileExists(string aPath)
        {
            Global.mFileSystemDebugger.SendInternal("VFSManager.FileExists");

            if (aPath == null)
            {
                return false;
            }

            try
            {
                Global.mFileSystemDebugger.SendInternal("aPath =");
                Global.mFileSystemDebugger.SendInternal(aPath);

                string xPath = Path.GetFullPath(aPath);
                Global.mFileSystemDebugger.SendInternal("After GetFullPath");
                Global.mFileSystemDebugger.SendInternal("xPath =");
                Global.mFileSystemDebugger.SendInternal(xPath);

                return GetFile(xPath) != null;
            }
            catch (Exception e)
            {
                global::System.Console.Write("Exception occurred: ");
                global::System.Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool FileExists(DirectoryEntry aEntry)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.FileExists ---");

            if (aEntry == null)
            {
                return false;
            }

            try
            {
                Global.mFileSystemDebugger.SendInternal("aEntry.mName =");
                Global.mFileSystemDebugger.SendInternal(aEntry.mName);

                string xPath = GetFullPath(aEntry);
                Global.mFileSystemDebugger.SendInternal("After GetFullPath");
                Global.mFileSystemDebugger.SendInternal("xPath =");
                Global.mFileSystemDebugger.SendInternal(xPath);

                return GetFile(xPath) != null;
            }
            catch (Exception e)
            {
                global::System.Console.Write("Exception occurred: ");
                global::System.Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool DirectoryExists(string aPath)
        {
            if (String.IsNullOrEmpty(aPath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(aPath));
            }

            Global.mFileSystemDebugger.SendInternal("--- VFSManager.DirectoryExists ---");

            try
            {
                Global.mFileSystemDebugger.SendInternal("aPath =");
                Global.mFileSystemDebugger.SendInternal(aPath);

                string xPath = Path.GetFullPath(aPath);
                Global.mFileSystemDebugger.SendInternal("After GetFullPath");
                Global.mFileSystemDebugger.SendInternal("xPath =");
                Global.mFileSystemDebugger.SendInternal(xPath);

                return GetDirectory(xPath) != null;
            }
            catch (Exception e)
            {
                global::System.Console.Write("Exception occurred: ");
                global::System.Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool DirectoryExists(DirectoryEntry aEntry)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.DirectoryExists ---");

            if (aEntry == null)
            {
                throw new ArgumentNullException(nameof(aEntry));
            }

            try
            {
                Global.mFileSystemDebugger.SendInternal("aEntry.mName =");
                Global.mFileSystemDebugger.SendInternal(aEntry.mName);

                string xPath = GetFullPath(aEntry);
                Global.mFileSystemDebugger.SendInternal("After GetFullPath");
                Global.mFileSystemDebugger.SendInternal("xPath =");
                Global.mFileSystemDebugger.SendInternal(xPath);

                return GetDirectory(xPath) != null;
            }
            catch (Exception e)
            {
                global::System.Console.Write("Exception occurred: ");
                global::System.Console.WriteLine(e.Message);
                return false;
            }
        }

        public static string GetFullPath(DirectoryEntry aEntry)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetFullPath ---");

            if (aEntry == null)
            {
                throw new ArgumentNullException(nameof(aEntry));
            }

            Global.mFileSystemDebugger.SendInternal("aEntry.mName =");
            Global.mFileSystemDebugger.SendInternal(aEntry.mName);

            var xParent = aEntry.mParent;
            string xPath = aEntry.mName;

            while (xParent != null)
            {
                xPath = xParent.mName + xPath;
                Global.mFileSystemDebugger.SendInternal("xPath =");
                Global.mFileSystemDebugger.SendInternal(xPath);

                xParent = xParent.mParent;
                Global.mFileSystemDebugger.SendInternal("xParent.mName =");
                Global.mFileSystemDebugger.SendInternal(xParent.mName);
            }

            Global.mFileSystemDebugger.SendInternal("xPath =");
            Global.mFileSystemDebugger.SendInternal(xPath);

            return xPath;
        }

        public static Stream GetFileStream(string aPathname)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetFileStream ---");

            if (string.IsNullOrEmpty(aPathname))
            {
                return null;
            }

            Global.mFileSystemDebugger.SendInternal("aPathName =");
            Global.mFileSystemDebugger.SendInternal(aPathname);

            var xFileInfo = GetFile(aPathname);
            if (xFileInfo == null)
            {
                throw new Exception("File not found: " + aPathname);
            }

            return xFileInfo.GetFileStream();
        }

        #region Helpers

        public static char GetAltDirectorySeparatorChar()
        {
            return '/';
        }

        public static char GetDirectorySeparatorChar()
        {
            return '\\';
        }

        public static char[] GetInvalidFileNameChars()
        {
            char[] xReturn = new char[17];
            xReturn[0] = '"';
            xReturn[1] = '<';
            xReturn[2] = '>';
            xReturn[3] = '|';
            xReturn[4] = '\0';
            xReturn[5] = '\a';
            xReturn[6] = '\b';
            xReturn[7] = '\t';
            xReturn[8] = '\n';
            xReturn[9] = '\v';
            xReturn[10] = '\f';
            xReturn[11] = '\r';
            xReturn[12] = ':';
            xReturn[13] = '*';
            xReturn[14] = '?';
            xReturn[15] = '\\';
            xReturn[16] = '/';
            return xReturn;
        }

        public static char[] GetInvalidPathCharsWithAdditionalChecks()
        {
            char[] xReturn = new char[14];
            xReturn[0] = '"';
            xReturn[1] = '<';
            xReturn[2] = '>';
            xReturn[3] = '|';
            xReturn[4] = '\0';
            xReturn[5] = '\a';
            xReturn[6] = '\b';
            xReturn[7] = '\t';
            xReturn[8] = '\n';
            xReturn[9] = '\v';
            xReturn[10] = '\f';
            xReturn[11] = '\r';
            xReturn[12] = '*';
            xReturn[13] = '?';
            return xReturn;
        }

        public static char GetPathSeparator()
        {
            return ';';
        }

        public static char[] GetRealInvalidPathChars()
        {
            char[] xReturn = new char[12];
            xReturn[0] = '"';
            xReturn[1] = '<';
            xReturn[2] = '>';
            xReturn[3] = '|';
            return xReturn;
        }

        public static char[] GetTrimEndChars()
        {
            char[] xReturn = new char[8];
            xReturn[0] = (char)0x9;
            xReturn[1] = (char)0xA;
            xReturn[2] = (char)0xB;
            xReturn[3] = (char)0xC;
            xReturn[4] = (char)0xD;
            xReturn[5] = (char)0x20;
            xReturn[6] = (char)0x85;
            xReturn[7] = (char)0xA0;
            return xReturn;
        }

        public static char GetVolumeSeparatorChar()
        {
            return ':';
        }

        public static int GetMaxPath()
        {
            return 260;
        }

        //public static bool IsAbsolutePath(this string aPath)
        //{
        //    return ((aPath[0] == VFSBase.DirectorySeparatorChar) || (aPath[0] == VFSBase.AltDirectorySeparatorChar));
        //}

        //public static bool IsRelativePath(this string aPath)
        //{
        //    return (aPath[0] != VFSBase.DirectorySeparatorChar || aPath[0] != VFSBase.AltDirectorySeparatorChar);
        //}

        public static string[] SplitPath(string aPath)
        {
            //TODO: This should call Path.GetDirectoryName() and then loop calling Directory.GetParent(), but those aren't implemented yet.
            return aPath.Split(GetDirectorySeparators(), StringSplitOptions.RemoveEmptyEntries);
        }

        private static char[] GetDirectorySeparators()
        {
            return new[] { GetDirectorySeparatorChar(), GetAltDirectorySeparatorChar() };
        }

        #endregion

        /// <summary>
        /// Gets the parent directory entry from the path.
        /// </summary>
        /// <param name="aPath">The full path to the current directory entry.</param>
        /// <returns>The parent directory entry.</returns>
        /// <exception cref="ArgumentException">Argument is null or empty</exception>
        /// <exception cref="NotImplementedException"></exception>
        public static DirectoryEntry GetParent(string aPath)
        {
            Global.mFileSystemDebugger.SendInternal("--- VFSManager.GetParent ---");

            if (string.IsNullOrEmpty(aPath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(aPath));
            }

            Global.mFileSystemDebugger.SendInternal("aPath =");
            Global.mFileSystemDebugger.SendInternal(aPath);

            if (aPath == Path.GetPathRoot(aPath))
            {
                return null;
            }

            string xFileOrDirectory = Path.HasExtension(aPath) ? Path.GetFileName(aPath) : Path.GetDirectoryName(aPath);
            if (xFileOrDirectory != null)
            {
                string xPath = aPath.Remove(aPath.Length - xFileOrDirectory.Length);
                return GetDirectory(xPath);
            }

            return null;
        }
    }
}
