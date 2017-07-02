﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace VF_WoWLauncher
{
    [ProtoContract]
    public class RealmInfo
    {
        [ProtoMember(1)]
        public string RealmList = "";
        [ProtoMember(2)]
        public string RealmName = "";
        [ProtoMember(3)]
        public WowVersionEnum WowVersion = WowVersionEnum.Vanilla;

        public string GetRealmListWTF()
        {
            return "set realmlist " + RealmList + "\r\nset realmname \"" + RealmName + "\"";
        }
    }

    [ProtoContract]
    public class LaunchShortcut
    {
        [ProtoMember(1)]
        public string ShortcutName;
        [ProtoMember(2)]
        public string Profile;
        [ProtoMember(3)]
        public string Realm;
    }

    [ProtoContract]
    public class Settings
    {
        private Settings() { }
        [ProtoMember(1)]
        public string DefaultConfig = "Active Wow Config";
        [ProtoMember(2)]
        public string DefaultRealm = "Anathema";
        [ProtoMember(3)]
        public bool ClearWDB = false;
        [ProtoMember(4)]
        public TimeSpan BackupWTFTimeSpan = TimeSpan.FromDays(30);
        [ProtoMember(5)]
        public DateTime BackupWTFNextTime = DateTime.UtcNow.AddDays(30);
        [ProtoMember(6)]
        public string _UserID = "";
        [ProtoMember(7)]
        public string _WowDirectory = ""; //D:\\Program\\World Of Warcraft Classic\\
        [ProtoMember(8)]
        public int LauncherWindow_Left = -1;
        [ProtoMember(9)]
        public int LauncherWindow_Top = -1;
        //[ProtoMember(10)]
        //public bool UseWoWNoDelay = false;
        [ProtoMember(11)]
        public string _WowTBCDirectory = ""; //D:\\Program\\World Of Warcraft Classic\\
        [ProtoMember(12)]//, IsRequired = true
        public bool AutoRefreshNews = true;
        [ProtoMember(13)]
        public List<LaunchShortcut> ShortcutLaunches = new List<LaunchShortcut>();
        [ProtoMember(14)]
        public bool UpdateToBeta = false;
        [ProtoMember(15)]
        public bool RunWoWNotAdmin = false;
        [ProtoMember(16)]
        public bool AutoHideOldNews = true;
        [ProtoMember(17)]
        public bool AutoUpdateVFAddons = false;
        [ProtoMember(18)]
        public Dictionary<string, RealmInfo> RealmLists = new Dictionary<string, RealmInfo>();
        [ProtoMember(19)]
        public bool NewsSources_Feenix = false;
        [ProtoMember(20)]
        public bool NewsSources_Nostalrius = false;
        [ProtoMember(21)]
        public bool NewsSources_Kronos = false;
        [ProtoMember(22)]
        public bool ContributeRealmPlayers = true;
        [ProtoMember(23)]
        public bool ContributeRaidStats = true;
        [ProtoMember(24)]
        public bool Wait5SecondsAfterUpload = true;
        [ProtoMember(25)]
        public List<string> RemovedRealmLists = new List<string>();
        [ProtoMember(26)]
        public string _WowTLKDirectory = "";
        [ProtoMember(27)]
        public string _WowCATADirectory = "";
        [ProtoMember(28)]
        public string _WowMOPDirectory = "";
        [ProtoMember(29)]
        public string _WowWODDirectory = "";
        [ProtoMember(30)]
        public string _WowLEGIONDirectory = ""; 

        public void AddLaunchShortcut(string _Profile, string _Realm)
        {
            var existingShortcutIndex = ShortcutLaunches.FindIndex((_Value) => _Value.Profile == _Profile && _Value.Realm == _Realm);
            if (existingShortcutIndex == -1)
            {
                ShortcutLaunches.Add(new LaunchShortcut { ShortcutName = _Profile + " @ " + _Realm, Profile = _Profile, Realm = _Realm });
            }
            else
            {
                var newItem = ShortcutLaunches[existingShortcutIndex];
                ShortcutLaunches.Remove(newItem);
                ShortcutLaunches.Add(newItem);
            }
            if (ShortcutLaunches.Count > 5)
                ShortcutLaunches.RemoveAt(0);
        }
        public static string UserID
        {
            get
            {
                return Instance._UserID;
            }
        }
        private static string WowDirectory
        {
            get
            {
                return Instance._WowDirectory;
            }
        }
        private static string WowTBCDirectory
        {
            get
            {
                return Instance._WowTBCDirectory;
            }
        }
        public static bool HaveClassic
        {
            get
            {
                return Instance._WowTBCDirectory != Instance._WowDirectory;
            }
        }
        public static bool HaveTBC
        {
            get
            {
                return Instance._WowTBCDirectory != "";
            }
        }
        public static bool HaveTLK 
        {
            get
            {
                return Instance._WowTLKDirectory != "";
            }
        }
        public static bool HaveCATA
        {
            get
            {
                return Instance._WowCATADirectory != "";
            }
        } 
        public static bool HaveMOP
        {
            get
            {
                return Instance._WowMOPDirectory != "";
            }
        }
        public static bool HaveWOD
        {
            get
            {
                return Instance._WowWODDirectory != "";
            }
        }
        public static bool HaveLEGION // IMPLEMENTED
        {
            get
            {
                return Instance._WowLEGIONDirectory != "";
            }
        }
        public static bool DebugMode
        {
            get
            {
                return Instance._UserID.StartsWith("Dilatazu") && (StaticValues.LauncherWorkDirectory.ToLower().Contains("release") || StaticValues.LauncherWorkDirectory.ToLower().Contains("debug"));
            }
        }
        public static bool FirstTimeRunning = false;
        private static Settings sm_Settings = Instance;
        public static Settings Instance
        {
            get
            {
                if (sm_Settings == null)
                {
                    if (System.IO.File.Exists(StaticValues.LauncherSettingsDirectory + "Settings.cfg") == true)
                    {
                        FirstTimeRunning = false;
                        Settings settingsData = null;
                        Utility.LoadSerialize(StaticValues.LauncherSettingsDirectory + "Settings.cfg", out settingsData);
                        sm_Settings = settingsData;
                    }
                    else
                    {
                        FirstTimeRunning = true;
                        sm_Settings = new Settings();
                    }
                }
                return sm_Settings;
            }
        }
        public static Dictionary<string, RealmInfo> CreateDefaultRealmLists()
        {
            return new Dictionary<string, RealmInfo>{
                //{"Emerald Dream", new RealmInfo{RealmList = "vanillafeenix.servegame.org", RealmName = "Emerald Dream [1x] Blizzlike", WowVersion = WowVersionEnum.Vanilla}},
                //{"Warsong", new RealmInfo{RealmList = "vanillafeenix.servegame.org", RealmName = "Warsong [12x] Blizzlike", WowVersion = WowVersionEnum.Vanilla}},
                //{"Al'Akir", new RealmInfo{RealmList = "vanillafeenix.servegame.org", RealmName = "Al'Akir [instant 60] Blizzlike", WowVersion = WowVersionEnum.Vanilla}},
                {"Archangel(TBC)", new RealmInfo{RealmList = "vanillafeenix.servegame.org", RealmName = "Archangel [14x] Blizzlike", WowVersion = WowVersionEnum.TBC}},
                {"VanillaGaming", new RealmInfo{RealmList = "logon.vanillagaming.org", RealmName = "VanillaGaming", WowVersion = WowVersionEnum.Vanilla}},
                {"Valkyrie", new RealmInfo{RealmList = "logon.valkyrie-wow.com", RealmName = "Valkyrie", WowVersion = WowVersionEnum.Vanilla}},
                {"Rebirth", new RealmInfo{RealmList = "wow.therebirth.net", RealmName = "Rebirth", WowVersion = WowVersionEnum.Vanilla}},
                {"Elysium", new RealmInfo{RealmList = "logon.elysium-project.org", RealmName = "Elysium", WowVersion = WowVersionEnum.Vanilla}},
                {"Zeth'Kur", new RealmInfo{RealmList = "logon.elysium-project.org", RealmName = "Zeth'Kur", WowVersion = WowVersionEnum.Vanilla}},
                {"Anathema", new RealmInfo{RealmList = "logon.elysium-project.org", RealmName = "Anathema", WowVersion = WowVersionEnum.Vanilla}},
                {"Darrowshire", new RealmInfo{RealmList = "logon.elysium-project.org", RealmName = "Darrowshire", WowVersion = WowVersionEnum.Vanilla}},
                {"Kronos", new RealmInfo{RealmList = "wow.twinstar.cz", RealmName = "Kronos", WowVersion = WowVersionEnum.Vanilla}},
                {"Kronos II", new RealmInfo{RealmList = "wow.twinstar.cz", RealmName = "Kronos II", WowVersion = WowVersionEnum.Vanilla}},
                {"Nefarian(DE)", new RealmInfo{RealmList = "logon.classic-wow.org", RealmName = "Nefarian", WowVersion = WowVersionEnum.Vanilla}},
                {"NostalGeek(FR)", new RealmInfo{RealmList = "auth.nostalgeek-serveur.com", RealmName = "NostalGeek 1.12", WowVersion = WowVersionEnum.Vanilla}},
                {"Stonetalon", new RealmInfo{RealmList = "logon.vengeancewow.com", RealmName = "Stonetalon", WowVersion = WowVersionEnum.TBC}},
                {"ExcaliburTBC", new RealmInfo{RealmList = "exwow-serv.exnw.com", RealmName = "ExcaliburTBC", WowVersion = WowVersionEnum.TBC}},
                {"Hellfire", new RealmInfo{RealmList = "logon.looking4group.eu", RealmName = "Hellfire", WowVersion = WowVersionEnum.TBC}},
                {"RetroWoW", new RealmInfo{RealmList = "logon.retro-wow.com", RealmName = "RetroWoW 1.12.1", WowVersion = WowVersionEnum.Vanilla}},
                {"MagicWoW", new RealmInfo{RealmList = "magicwow.co.uk", RealmName = "MagicWoW", WowVersion = WowVersionEnum.Vanilla}},
                {"SiegeWoW", new RealmInfo{RealmList = "logon.siegewow.com", RealmName = "SiegeWoW", WowVersion = WowVersionEnum.Vanilla}},
                {"BlackRock", new RealmInfo{RealmList = "blackrock.firestorm-servers.com", RealmName = "Blackrock", WowVersion = WowVersionEnum.Vanilla}},
            };
        }
        public static void Initialize()
        {
            var triggerLoad = Instance;
            if (Instance.RealmLists == null || Instance.RealmLists.Count == 0)
            {
                Instance.RealmLists = CreateDefaultRealmLists();
            }
        }
        public static void Uninitialize()
        {
            sm_Settings = null;
        }
        public static void Save()
        {
            Utility.AssertDirectory(StaticValues.LauncherSettingsDirectory);
            Utility.SaveSerialize(StaticValues.LauncherSettingsDirectory + "Settings.cfg", sm_Settings, false);
        }

        internal static string GetWowDirectory(WowVersionEnum _WowVersion)
        {
            if (_WowVersion == WowVersionEnum.TBC && HaveTBC == true)
                return WowTBCDirectory;
            else
                return WowDirectory;
        }
    }
}
