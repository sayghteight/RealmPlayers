﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VF_WoWLauncher
{
    public enum WowVersionEnum
    {
        Vanilla = 1,
        TBC = 2,
        TLK = 3,
        /* Implemented support versions */
        CATA = 4,
        MOP = 5,
        WOD = 6,
        LEGION = 7,
    }
    class WowUtility
    {
        public static string GetPerCharacterSavedVariableFilePath(string _Account, string _Realm, string _Character, string _AddonName, WowVersionEnum _WowVersion)
        {
            return Settings.GetWowDirectory(_WowVersion) + "WTF\\Account\\" + _Account + "\\" + _Realm + "\\" + _Character + "\\SavedVariables\\" + _AddonName + ".lua";
        }
        public static string GetSavedVariableFilePath(string _Account, string _AddonName, WowVersionEnum _WowVersion)
        {
            return Settings.GetWowDirectory(_WowVersion) + "WTF\\Account\\" + _Account + "\\SavedVariables\\" + _AddonName + ".lua";
        }
        public static List<string> GetPerCharacterSavedVariableFilePaths(string _AddonName, WowVersionEnum _WowVersion)
        {
            List<string> returnPaths = new List<string>();
            var accounts = GetAccounts(_WowVersion);
            foreach (var account in accounts)
            {
                var realms = GetRealms(account, _WowVersion);
                foreach(var realm in realms)
                {
                    var characters = GetCharacters(account, realm, _WowVersion);
                    foreach(var character in characters)
                    {
                        string savedVariableFilePath = GetPerCharacterSavedVariableFilePath(account, realm, character, _AddonName, _WowVersion);
                        if (System.IO.File.Exists(savedVariableFilePath))
                        {
                            returnPaths.Add(savedVariableFilePath);
                        }
                    }
                }
            }
            return returnPaths;
        }
        public static List<string> GetSavedVariableFilePaths(string _AddonName, WowVersionEnum _WowVersion)
        {
            List<string> returnPaths = new List<string>();
            var accounts = GetAccounts(_WowVersion);
            foreach (var account in accounts)
            {
                string savedVariableFilePath = GetSavedVariableFilePath(account, _AddonName, _WowVersion);
                if (System.IO.File.Exists(savedVariableFilePath))
                {
                    returnPaths.Add(savedVariableFilePath);
                }
            }
            return returnPaths;
        }
        public static bool IsAddonInstalled(string _AddonName, WowVersionEnum _WowVersion)
        {
            return System.IO.Directory.Exists(Settings.GetWowDirectory(_WowVersion) + "Interface\\Addons\\" + _AddonName);
        }
        public static List<string> GetAccounts(WowVersionEnum _WowVersion)
        {
            return Utility.GetDirectoriesInDirectory(Settings.GetWowDirectory(_WowVersion) + "WTF\\Account\\");
        }
        public static List<string> GetRealms(string _Account, WowVersionEnum _WowVersion)
        {
            return Utility.GetDirectoriesInDirectory(Settings.GetWowDirectory(_WowVersion) + "WTF\\Account\\" + _Account + "\\");
        }
        public static List<string> GetCharacters(string _Account, string _Realm, WowVersionEnum _WowVersion)
        {
            return Utility.GetDirectoriesInDirectory(Settings.GetWowDirectory(_WowVersion) + "WTF\\Account\\" + _Account + "\\" + _Realm + "\\");
        }
        public static bool ExtractLuaVariableFromFile(string _EntireFileData, string _LuaVariableName, out string _ResultFileData, out string _VariableData)
        {
            _ResultFileData = "";
            _VariableData = "";
            string[] beforeAndDuring = _EntireFileData.SplitVF("\r\n" + _LuaVariableName + " = {", 2);
            if(beforeAndDuring.Length != 2)
                return false;
            _ResultFileData = beforeAndDuring[0];
            string[] duringAndAfter = beforeAndDuring[1].SplitVF("\r\n}", 2);
            if (duringAndAfter.Length != 2)
                return false;
            _VariableData = "\r\n" + _LuaVariableName + " = {\r\n" + duringAndAfter[0] + "\r\n}";
            _ResultFileData += duringAndAfter[1];
            return true;
        }

        public static bool IsValidWowDirectory(string _Directory)
        {
            return Utility.ContainsFilesAndDirectories(_Directory, new string[] { "WTF", "Interface", "Data", "WoW.exe" });
        }
        public static bool IsWowDirectoryClassic(string _Directory)
        {
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(_Directory + "\\WoW.exe");
            return fileVersionInfo.FileVersion == "1, 12, 1, 5875"
                || (fileVersionInfo.FileMajorPart == 1 && fileVersionInfo.FileMinorPart == 12 && fileVersionInfo.FileBuildPart == 1);
        }
        public static bool IsWowDirectoryTBC(string _Directory)
        {
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(_Directory + "\\WoW.exe");
            return fileVersionInfo.FileVersion == "2, 4, 3, 8606" 
                || (fileVersionInfo.FileMajorPart == 2 && fileVersionInfo.FileMinorPart == 4 && fileVersionInfo.FileBuildPart == 3);
        }
        /*
         * UPDATE NEW EXPANSIONS
         */
        public static bool IsWowDirectoryTLK(string _Directory)
        {
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(_Directory + "\\WoW.exe");
            return fileVersionInfo.FileVersion == "3, 3, 5, 12340"
                || (fileVersionInfo.FileMajorPart == 3 && fileVersionInfo.FileMinorPart == 3 && fileVersionInfo.FileBuildPart == 5);
        }
        public static bool IsWowDirectoryCATA(string _Directory)
        {
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(_Directory + "\\WoW.exe");
            return fileVersionInfo.FileVersion == "4, 3, 4, 15595"
                || (fileVersionInfo.FileMajorPart == 4 && fileVersionInfo.FileMinorPart == 3 && fileVersionInfo.FileBuildPart == 4);
        }
        public static bool IsWowDirectoryMOP(string _Directory)
        {
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(_Directory + "\\WoW.exe");
            return fileVersionInfo.FileVersion == "5, 4, 8, 18291"
                || (fileVersionInfo.FileMajorPart == 5 && fileVersionInfo.FileMinorPart == 4 && fileVersionInfo.FileBuildPart == 8);
        }
        public static bool IsWowDirectoryWOD(string _Directory)
        {
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(_Directory + "\\WoW.exe");
            return fileVersionInfo.FileVersion == "6, 2, 4, 21742" /* BUILD HOTFIX6 */
                || (fileVersionInfo.FileMajorPart == 6 && fileVersionInfo.FileMinorPart == 2 && fileVersionInfo.FileBuildPart == 4);
        }
        public static bool IsWowDirectoryLEGION(string _Directory)
        {
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(_Directory + "\\WoW.exe");
            return fileVersionInfo.FileVersion == "7, 2, 0, 23937" /* BUILD 7.2.0 TUMB OF SARGERAS */
                || (fileVersionInfo.FileMajorPart == 7 && fileVersionInfo.FileMinorPart == 2 && fileVersionInfo.FileBuildPart == 0);
        }

        public static short[] GetFileStatus(string _LuaFile, Dictionary<string, DateTime> _BufferedFileStatuses = null)
        {
            if (_BufferedFileStatuses == null)
                _BufferedFileStatuses = new Dictionary<string, DateTime>();
            List<short> relations = new List<short>();
            if (_BufferedFileStatuses.ContainsKey(_LuaFile) == false)
                _BufferedFileStatuses.Add(_LuaFile, System.IO.File.GetLastWriteTime(_LuaFile));
            DateTime lastWriteTime = _BufferedFileStatuses[_LuaFile];
            string savedVarDir = System.IO.Path.GetDirectoryName(_LuaFile);
            string luaFilename = System.IO.Path.GetFileName(_LuaFile).ToLower();

            List<string> checkFiles = new List<string>(System.IO.Directory.GetFiles(savedVarDir, "*.lua"));
            if (_LuaFile.Contains("WTF") == true && _LuaFile.Contains("Account") == true)
            {
                var wowDir = _LuaFile.Substring(0, _LuaFile.IndexOf("WTF"));
                checkFiles.Add(wowDir + "WTF\\Config.wtf");
                checkFiles.Add(wowDir + "WDB\\creaturecache.wdb");
                checkFiles.Add(wowDir + "WDB\\itemnamecache.wdb");
                checkFiles.Add(wowDir + "WDB\\gameobjectcache.wdb");
                checkFiles.Add(wowDir + "WDB\\npccache.wdb");
                checkFiles.Add(wowDir + "WDB\\wowcache.wdb");
            }
            else
            {
                relations.Add(short.MinValue);
                relations.Add(short.MinValue);
                relations.Add(short.MinValue);
                relations.Add(short.MinValue);
                relations.Add(short.MinValue);
                relations.Add(short.MinValue);
            }

            foreach (string file in checkFiles)
            {
                if (file.Contains("lua.bak")) //skip lua.bak files
                    continue;
                if (file.ToLower().EndsWith(luaFilename)) //skip myself
                    continue;

                if (_BufferedFileStatuses.ContainsKey(file) == false)
                    _BufferedFileStatuses.Add(file, System.IO.File.GetLastWriteTime(file));
                DateTime currWriteTime = _BufferedFileStatuses[file];
                double totalSeconds = (currWriteTime - lastWriteTime).TotalSeconds;
                if (totalSeconds > (double)short.MaxValue)
                    relations.Add(short.MaxValue);
                else if (totalSeconds < (double)short.MinValue)
                    relations.Add(short.MinValue);
                else
                    relations.Add((short)totalSeconds);
            }
            //First x files is other lua files, Last 6 is config files, 
            //If this is ever changed, remember to change on server aswell

            //Negative values means file is OLDER than Addon Data File
            //Positive values means file is NEWER than Addon Data File
            return relations.ToArray();
        }

        public static string ConvertNoColor(string _FormattedString)
        {
            try
            {
                string retString = "";
                string[] splitData = _FormattedString.Split('|');
                if(splitData.Length == 1)
                    return splitData.First();
                retString = splitData.First();
                for (int i = 1; i < splitData.Length; ++i)
                {
                    if (splitData[i].StartsWith("c"))
                    {
                        retString += splitData[i].Substring(9);
                    }
                    else if (splitData[i].StartsWith("r"))
                    {
                        retString += splitData[i].Substring(1);
                    }
                    else
                    {
                        retString += "|" + splitData[i];
                    }
                }
                return retString;
            }
            catch (Exception)
            {
                return _FormattedString;
            }
        }
    }
}
