using System.Collections;
using System.Collections.Generic;
using System.IO;
// using JsonReadAndWrite;
using UnityEngine;

namespace Thai.Lib
{
    public class JsonHandle<T>
    {
        public string folderName;
        public string fileName;
        public string pathName;

        public JsonHandle(string folderName, string fileName)
        {
            this.folderName = folderName;
            this.fileName = fileName;
        }

        public virtual T Read(string folderPath)
        {
            if (!Application.isEditor)
            {
                folderPath = Application.persistentDataPath;
            }

            string path = Path.Combine(folderPath, folderName, fileName + ".json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Debug.Log("Read from file: " + path);
                T data = JsonUtility.FromJson<T>(json);
                return data;
            }
            else
            {
                Debug.Log("NoData at: " + path);
                return default;
            }
        }

        public virtual void Write(T data, string folderPath)
        {
            if (!Application.isEditor)
            {
                folderPath = Application.persistentDataPath;
            }

            string dir = Path.Combine(folderPath, folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string path = Path.Combine(dir, fileName + ".json");
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            Debug.Log("Player data saved to: " + path);
        }
    }
}

