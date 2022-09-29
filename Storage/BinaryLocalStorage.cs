using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// TO DO:
// - Make async

namespace NorskaLib.Storage
{
    public class BinaryLocalStorage : LocalStorage
    {
        public static BinaryLocalStorage Instance { get; private set; }

        public BinaryLocalStorage()
        {
            Instance = this;
        }

        private const string FileFormat = "bin";

        private Dictionary<string, string> filesPathes = new Dictionary<string, string>(3);
        private string GetPath(string filename)
        {
            if (filesPathes.TryGetValue(filename, out var path))
                return path;
            else
            {
                path = $"{Application.persistentDataPath}/{filename}.{FileFormat}";
                filesPathes.Add(filename, path);
                return path;
            }
        }

        private string loadedSlotName;
        public override string LoadedSlotName => loadedSlotName;

        private List<string> existingSlots = new List<string>(3);
        public override IEnumerable<string> ExistingSlots => existingSlots;

        private void LoadModules(string filename,IStorageModule[] collection)
        {
            var dataExist = TryRead(filename, out var modulesData);
            var wantUpdateFile = false;

            foreach (var module in collection)
                if (dataExist)
                {
                    if (modulesData.TryGetValue(module.GetType(), out var data))
                    {
                        module.SetSerializedState(data);
                        Debug.Log($"Setting serialized state for module {module.GetType()} from file...");
                    }
                    else
                    {
                        module.CreateDefaultState();
                        wantUpdateFile |= true;
                        Debug.Log($"Creating default state for module {module.GetType()}...");
                    }
                }
                else
                {
                    module.CreateDefaultState();
                    wantUpdateFile |= true;
                    Debug.Log($"Creating default state for module {module.GetType()}...");
                }

            if (wantUpdateFile)
                Write(filename, collection);
        }

        private void Write(string filename, IEnumerable<IStorageModule> modules)
        {
            var path = GetPath(filename);
            Debug.Log($"Writing to file '{path}'...");

            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Create);

            var modulesData = modules.ToDictionary(m => m.GetType(), m => m.GetSerializedState());
            formatter.Serialize(stream, modulesData);
            stream.Close();
        }

        private bool TryRead(string filename, out Dictionary<Type, byte[]> data)
        {
            var path = GetPath(filename);

            if (!File.Exists(path))
            {
                data = null;
                return false;
            }

            Debug.Log($"Reading from file '{path}'...");

            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as Dictionary<Type, byte[]>;
            stream.Close();

            return true;
        }

        protected override void DetectSlots()
        {
            existingSlots = Directory
                .GetFiles(Application.persistentDataPath, $"*.{FileFormat}", SearchOption.TopDirectoryOnly)
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .ToList();
        }

        public override void LoadShared(string name)
        {
            if (!IsInitialized)
            {
                Debug.LogError($"Storage is not initialized!");
                return;
            }

            Debug.Log($"Loading shared modules '{name}'");
            LoadModules(name, modulesShared);
        }

        public override void LoadSlot(string name)
        {
            if (!IsInitialized)
            {
                Debug.LogError($"Storage is not initialized!");
                return;
            }

            Debug.Log($"Loading substituted modules of save slot '{name}'.");
            LoadModules(name, modulesSlot);

            loadedSlotName = name;
        }

        public override void SaveShared(string name)
        {
            if (!IsInitialized)
            {
                Debug.LogError($"Storage is not initialized!");
                return;
            }

            Write(name, modulesShared);
        }

        public override void SaveSlot(string name)
        {
            if (!IsInitialized)
            {
                Debug.LogError($"Storage is not initialized!");
                return;
            }

            Write(name, modulesSlot);
        }
    }
}