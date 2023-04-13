using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using Mono.Cecil;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace VRPatcher
{
    public class Patcher
    {
        // code successfully borrowed from https://github.com/Raicuparta/two-forks-vr/blob/main/TwoForksVrPatcher/Patcher.cs

        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
        }

        public static void Initialize()
        {
            string InstallerPath = Assembly.GetExecutingAssembly().Location,
                   GameExePath = Process.GetCurrentProcess().MainModule.FileName,
                   PatcherPath = Path.GetDirectoryName(InstallerPath),

                   GamePath = Path.GetDirectoryName(GameExePath),
                   GameName = Path.GetFileNameWithoutExtension(GameExePath),
                   DataPath = Path.Combine(GamePath, $"{GameName}_Data/"),

                   GameManagersPath = Path.Combine(DataPath, $"globalgamemanagers"),
                   GameManagersBackupPath = CreateGameManagersBackup(GameManagersPath),

                   ClassDataPath = Path.Combine(PatcherPath, "classdata.tpk");

            Console.WriteLine("Begin epic conversion from ULTRAKILL to VRTRAKILL...");
            PatchVR(GameManagersBackupPath, GameManagersPath, ClassDataPath);
            Console.WriteLine("Epic conversion from ULTRAKILL to VRTRAKILL is complete.");
        }

        private static string CreateGameManagersBackup(string GameManagersPath)
        {
            Console.WriteLine($"Backing up '{GameManagersPath}'...");
            string BackupPath = GameManagersPath + ".bak";
            if (File.Exists(BackupPath))
            {
                Console.WriteLine($"Backup already exists.");
                return BackupPath;
            }
            File.Copy(GameManagersPath, BackupPath);
            Console.WriteLine($"Created backup in '{BackupPath}'");
            return BackupPath;
        }

        private static void PatchVR(string GameManagersBackupPath, string GameManagersPath, string ClassDataPath)
        {
            Console.WriteLine($"Using ClassData file from '{ClassDataPath}'");

            AssetsManager AM = new AssetsManager();
            AM.LoadClassPackage(ClassDataPath);
            AssetsFileInstance GGM = AM.LoadAssetsFile(GameManagersBackupPath, false);
            AssetsFile GGMFile = GGM.file;
            AssetsFileTable GGMTable = GGM.table;
            AM.LoadClassDatabaseFromPackage(GGMFile.typeTree.unityVersion);

            List<AssetsReplacer> Replacers = new List<AssetsReplacer>();

            AssetFileInfoEx BuildSettings = GGMTable.GetAssetInfo(11);
            AssetTypeValueField BuildSettingsBase = AM.GetTypeInstance(GGMFile, BuildSettings).GetBaseField();
            AssetTypeValueField EnabledVRDevices = BuildSettingsBase.Get("enabledVRDevices").Get("Array");
            AssetTypeTemplateField StringTemplate = EnabledVRDevices.templateField.children[1];
            AssetTypeValueField[] VRDevicesList = new[] { StringField("OpenVR", StringTemplate) };
            EnabledVRDevices.SetChildrenList(VRDevicesList);

            Replacers.Add(new AssetsReplacerFromMemory(0, BuildSettings.index, (int)BuildSettings.curFileType, 0xffff,
                                                       BuildSettingsBase.WriteToByteArray()));

            using (var writer = new AssetsFileWriter(File.OpenWrite(GameManagersPath)))
                GGMFile.Write(writer, 0, Replacers, 0);
        }

        private static AssetTypeValueField StringField(string str, AssetTypeTemplateField template)
        {
            return new AssetTypeValueField()
            {
                children = null,
                childrenCount = 0,
                templateField = template,
                value = new AssetTypeValue(EnumValueTypes.ValueType_String, str)
            };
        }
    }
}
