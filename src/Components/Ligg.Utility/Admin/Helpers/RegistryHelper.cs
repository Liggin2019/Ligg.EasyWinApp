using System;
using Microsoft.Win32;

namespace Ligg.Utility.Admin.Helpers
{
    public static class RegistryHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        //#add
        //#get
        public static string GetValue(string regPath)
        {
            try
            {
                var regePathDetail = GetRegistryPathDetail(regPath);
                var result = GetValue(regePathDetail.RootKeyShortName, regePathDetail.KeyString, regePathDetail.ValueName);
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetValue Error: " + ex.Message);
            }

        }

        public static string GetValue(string regPath, int viewBitNo)
        {
            var regePathDetail = GetRegistryPathDetail(regPath);
            if (viewBitNo == 32)
            {
                return GetView32Value(regePathDetail.RootKeyShortName, regePathDetail.KeyString, regePathDetail.ValueName);
            }
            else if (viewBitNo == 64)
            {
                return GetView64Value(regePathDetail.RootKeyShortName, regePathDetail.KeyString, regePathDetail.ValueName);
            }
            else return GetValue(regePathDetail.RootKeyShortName, regePathDetail.KeyString, regePathDetail.ValueName);
        }

        private static string GetValue(string rootKeyStName, string keyPath, string valName)
        {
            try
            {
                var rootKey = GetRootKeyByShortName(rootKeyStName);
                using (var key = rootKey.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        var obj = key.GetValue(valName);
                        if (obj != null)
                        {
                            return obj.ToString();
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetValue Error: " + ex.Message);
            }

        }

        //only for net4 above
        private static string GetView32Value(string rootKeyStName, string keyPath, string valName)
        {
            try
            {
                var inHave = GetRootKeyInHaveByShortName(rootKeyStName);
                using (var view32 = RegistryKey.OpenBaseKey(inHave, RegistryView.Registry32))
                {
                    using (var clsid32 = view32.OpenSubKey(keyPath, false))
                    {
                        if (clsid32 != null)
                        {
                            var val = clsid32.GetValue(valName);
                            if (val != null)
                            {
                                return val.ToString();
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetView32Value Error: " + ex.Message);
            }
        }

        //only for net4 above
        private static string GetView64Value(string rootKeyStName, string keyPath, string valName)
        {
            try
            {
                var result = "";
                var inHave = GetRootKeyInHaveByShortName(rootKeyStName);
                using (var view64 = RegistryKey.OpenBaseKey(inHave, RegistryView.Registry64))
                {
                    using (var clsid64 = view64.OpenSubKey(keyPath, false))
                    {
                        if (clsid64 != null)
                        {
                            var val = clsid64.GetValue(valName);
                            if (val != null)
                            {
                                return val.ToString();
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetView64Value Error: " + ex.Message);
            }
        }



        private static RegistryHive GetRootKeyInHaveByShortName(string rootKeyShortName)
        {
            var hKeyShortNameInLow = rootKeyShortName.ToLower();
            switch (hKeyShortNameInLow)
            {
                case "hkey_current_user":
                    return RegistryHive.CurrentUser;
                case "hkcu":
                    return RegistryHive.CurrentUser;
                case "cu":
                    return RegistryHive.CurrentUser;


                case "hkey_local_machine":
                    return RegistryHive.LocalMachine;
                case "hklm":
                    return RegistryHive.LocalMachine;
                case "lm":
                    return RegistryHive.LocalMachine;


                case "hkey_users":
                    return RegistryHive.Users;
                case "hkus":
                    return RegistryHive.Users;
                case "us":
                    return RegistryHive.Users;


                case "hkey_classes_root":
                    return RegistryHive.ClassesRoot;
                case "hkcr":
                    return RegistryHive.ClassesRoot;
                case "cr":
                    return RegistryHive.ClassesRoot;


                case "hkey_current_config":
                    return RegistryHive.CurrentConfig;
                case "hkcc":
                    return RegistryHive.CurrentConfig;
                case "cc":
                    return RegistryHive.CurrentConfig;
            }
            return RegistryHive.LocalMachine;
        }

        public static RegistryKey GetRootKeyByShortName(string rootKeyShortName)
        {
            var hKeyShortNameInLow = rootKeyShortName.ToLower();
            switch (hKeyShortNameInLow)
            {

                case "hkey_current_user":
                    return Registry.CurrentUser;
                case "hkcu":
                    return Registry.CurrentUser;
                case "cu":
                    return Registry.CurrentUser;

                case "hkey_local_machine":
                    return Registry.LocalMachine;
                case "hklm":
                    return Registry.CurrentUser;
                case "lm":
                    return Registry.LocalMachine;

                case "hkey_users":
                    return Registry.Users;
                case "hkus":
                    return Registry.CurrentUser;
                case "us":
                    return Registry.Users;

                case "hkey_classes_root":
                    return Registry.ClassesRoot;
                case "hkcr":
                    return Registry.CurrentUser;
                case "cr":
                    return Registry.ClassesRoot;

                case "hkey_current_config":
                    return Registry.CurrentConfig;
                case "hkcc":
                    return Registry.CurrentConfig;
                case "cc":
                    return Registry.CurrentConfig;

            }
            return Registry.LocalMachine;
        }

        public static RegRootKeyType GetRootKeyType(string rootKeyShortName)
        {
            var hKeyShortNameInLow = rootKeyShortName.ToLower();
            switch (hKeyShortNameInLow)
            {

                case "hkey_current_user":
                    return RegRootKeyType.User;
                case "hkcu":
                    return RegRootKeyType.User;
                case "cu":
                    return RegRootKeyType.User;

                case "hkey_local_machine":
                    return RegRootKeyType.Machine;
                case "hklm":
                    return RegRootKeyType.Machine;
                case "lm":
                    return RegRootKeyType.Machine;

                case "hkey_classes_root":
                    return RegRootKeyType.Root;
                case "hkcr":
                    return RegRootKeyType.Root;
                case "cr":
                    return RegRootKeyType.Root;

            }
            return RegRootKeyType.Machine;
        }

        public static RegistryPathDetail GetRegistryPathDetail(string regPath)
        {
            try
            {
                var regPathArray = regPath.Split('\\');
                var rootKeyStName = regPathArray[0];
                var valName = regPathArray[regPathArray.Length - 1];
                var keyString = "";
                for (int i = 1; i < regPathArray.Length - 1; i++)
                {
                    if (i == 1)
                        keyString = regPathArray[i];
                    else
                        keyString = keyString + "\\" + regPathArray[i];
                }
                return new RegistryPathDetail(rootKeyStName, keyString, valName);

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetRegistryPathDetail Error: " + ex.Message);
            }
        }

        public static RegistryValueKind GetValueKind(string valueTypeName)
        {
            try
            {
                var valKind = RegistryValueKind.String;
                var valKindName = valueTypeName.ToLower();
                switch (valKindName)
                {
                    case "qword":
                        valKind = RegistryValueKind.QWord;
                        break;
                    case "multistring":
                        valKind = RegistryValueKind.MultiString;
                        break;
                    case "expandstring":
                        valKind = RegistryValueKind.ExpandString;
                        break;
                    case "dword":
                        valKind = RegistryValueKind.DWord;
                        break;
                    case "binary":
                        valKind = RegistryValueKind.Binary;
                        break;
                }
                return valKind;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetValueTypeName Error: " + ex.Message);
            }
        }

        //#check

        public static bool IfKeyValueExits(string regPath)
        {
            try
            {
                var regPathArray = regPath.Split('\\');
                var rootKeyStName = regPathArray[0];
                var valName = regPathArray[regPathArray.Length - 1];
                var keyPath = "";
                for (int i = 1; i < regPathArray.Length - 1; i++)
                {
                    if (i == 1)
                        keyPath = regPathArray[i];
                    else
                        keyPath = keyPath + "\\" + regPathArray[i];
                }

                RegistryKey rootKey = GetRootKeyByShortName(rootKeyStName);
                using (var key = rootKey.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        if (IfKeyHasValue(key, valName))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".IfKeyValueExits Error: " + ex.Message);
            }
        }


        private static bool IfKeyHasValue(RegistryKey key, string valName)
        {
            try
            {
                if (key == null) return false;
                string[] valNames = key.GetValueNames();
                foreach (string valName1 in valNames)
                {
                    if (valName1.Trim() == valName.Trim())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".IfKeyHasValue Error: " + ex.Message);
            }
        }

       

    }

    //#common
    public enum RegSAM
    {
        QueryValue = 0x0001,
        SetValue = 0x0002,
        CreateSubKey = 0x0004,
        EnumerateSubKeys = 0x0008,
        Notify = 0x0010,
        CreateLink = 0x0020,
        WOW64_32Key = 0x0200,
        WOW64_64Key = 0x0100,
        WOW64_Res = 0x0300,
        Read = 0x00020019,
        Write = 0x00020006,
        Execute = 0x00020019,
        AllAccess = 0x000f003f
    }

    public enum RegRootKeyType
    {
        Root = 0,
        User = 1,
        Machine = 2,
    }

    public enum RegistryActionType
    {
        DoNothing = 0,
        DeleteValue = 1,
        SetValue = 2,
        //DeleteValueByGp = 11,
        SetValueByGp =3,
    }

    public class RegistryPathDetail
    {
        public RegistryPathDetail(string rootKeyShortName, string keyString, string valueName)
        {
            RootKeyShortName = rootKeyShortName;
            RegRootKeyType = RegistryHelper.GetRootKeyType(rootKeyShortName);
            RootRegistryKey =RegistryHelper.GetRootKeyByShortName(rootKeyShortName);
            KeyString = keyString;
            ValueName = valueName;
        }

        public string RootKeyShortName { get; set; }
        public RegRootKeyType RegRootKeyType { get; set; }
        public RegistryKey RootRegistryKey { get; set; }
        public string KeyString { get; set; }
        public string ValueName { get; set; }
    }



}


