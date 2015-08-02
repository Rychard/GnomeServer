using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Game;
using Newtonsoft.Json;

namespace GnomeServer
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class Configuration
    {
        private static readonly Object LockerObject = new Object();
        private static readonly String _filePath;
        private static List<Setting> _settings;

        static Configuration()
        {
            _filePath = GetSettingsFilePath();
            LoadSettings();
        }

        public static String GetUserDataPath()
        {
            try
            {
                String savePath = GnomanEmpire.SaveFolderPath();
                return savePath;
            }
            catch (Exception)
            {
                var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var myGames = Path.Combine(myDocuments, "My Games");
                var gameFolder = Path.Combine(myGames, "Gnomoria");
                return gameFolder;
            }
        }

        private static String GetFilePath(String fileName)
        {
            var gameFolder = GetUserDataPath();
            var filePath = Path.Combine(gameFolder, fileName);
            return CanAccess(filePath) ? filePath : fileName;
        }

        public static String GetSettingsFilePath()
        {
            const String fileName = "GnomeServer.json";
            return GetFilePath(fileName);
        }

        internal static String GetLogFilePath()
        {
            const String fileName = "GnomeServer.log";
            return GetFilePath(fileName);
        }

        private static Boolean CanAccess(String filePath)
        {
            try
            {
                // Using exceptions for program flow, not the best thing in the world, but it remains until I implement a better solution.
                // ReSharper disable once UnusedVariable
                using (var fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void LoadSettings()
        {
            lock (LockerObject)
            {
                List<Setting> settings = null;
                if (File.Exists(_filePath))
                {
                    using (StreamReader file = File.OpenText(_filePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        var deserialized = serializer.Deserialize(file, typeof (List<Setting>));
                        settings = deserialized as List<Setting>;
                    }
                }
                _settings = settings ?? new List<Setting>();
            }
        }

        public static void SaveSettings()
        {
            lock (LockerObject)
            {
                // No settings to save?  Don't save anything.
                if (_settings == null) { return; }

                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }

                using (StreamWriter file = File.CreateText(_filePath))
                {
                    JsonSerializer serializer = new JsonSerializer
                    {
                        Formatting = Formatting.Indented
                    };
                    serializer.Serialize(file, _settings);
                }
            }
        }

        private static String GetSettingRaw(String key)
        {
            lock (LockerObject)
            {
                String raw;
                var matches = _settings.Where(obj => obj.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (matches.Any())
                {
                    raw = matches.First().Value;
                }
                else
                {
                    raw = null;
                }
                return raw;
            }
        }

        private static void SetSettingRaw(String key, String value, String type)
        {
            lock (LockerObject)
            {
                var matches = _settings.Where(obj => obj.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (matches.Any())
                {
                    matches.First().Value = value;
                }
                else
                {
                    _settings.Add(new Setting
                    {
                        Key = key,
                        Value = value,
                        Type = type
                    });
                }
            }
        }

        public static Boolean HasSetting(String key)
        {
            lock (LockerObject)
            {
                if (_settings == null) { return false; }
                var matches = _settings.Where(obj => obj.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).ToList();
                return (matches.Any());
            }
        }

        public static Type GetSettingType(String key)
        {
            lock (LockerObject)
            {
                var matches = _settings.Where(obj => obj.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (matches.Any())
                {
                    var t = matches.First().Type;
                    switch (t)
                    {
                        case "string":
                            return typeof(String);

                        case "int":
                            return typeof(Int32);

                        case "float":
                            return typeof(Single);

                        case "double":
                            return typeof(Double);

                        default:
                            return typeof(Object);
                    }
                }
                return null;
            }
        }

        #region String

        public static String GetString(String key)
        {
            var raw = GetSettingRaw(key);
            return raw;
        }

        public static void SetString(String key, String value)
        {
            SetSettingRaw(key, value, "string");
        }

        #endregion String

        #region Integer

        public static Int32 GetInt(String key)
        {
            var raw = GetSettingRaw(key);
            Int32 i;
            if (Int32.TryParse(raw, out i))
            {
                return i;
            }
            return default(Int32);
        }

        public static void SetInt(String key, Int32 value)
        {
            SetSettingRaw(key, value.ToString(CultureInfo.InvariantCulture), "int");
        }

        #endregion Integer

        #region Float

        public static Single GetFloat(String key)
        {
            var raw = GetSettingRaw(key);
            Single f;
            if (Single.TryParse(raw, out f))
            {
                return f;
            }
            return default(Single);
        }

        public static void SetFloat(String key, Single value)
        {
            SetSettingRaw(key, value.ToString(CultureInfo.InvariantCulture), "float");
        }

        #endregion Float

        #region Double

        public static Double GetDouble(String key)
        {
            var raw = GetSettingRaw(key);
            Double d;
            if (Double.TryParse(raw, out d))
            {
                return d;
            }
            return default(Double);
        }

        public static void SetDouble(String key, Double value)
        {
            SetSettingRaw(key, value.ToString(CultureInfo.InvariantCulture), "double");
        }

        #endregion Double
    }

    public class Setting
    {
        public String Key { get; set; }

        public String Value { get; set; }

        public String Type { get; set; }
    }
}
