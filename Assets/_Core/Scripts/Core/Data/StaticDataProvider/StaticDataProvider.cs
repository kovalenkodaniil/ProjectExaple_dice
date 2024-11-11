using System;
using System.Collections.Generic;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Core.Data
{
    public interface IStaticDataProvider { }
    
    public static class StaticDataProvider
    {
        private static Dictionary<Type, IStaticDataProvider> datas = new(20);

        public static void Add<T>(T provider) where T : IStaticDataProvider
        {
            datas.TryAdd(typeof(T), provider);
        }
        
        public static void Replace<T>(T provider) where T : class, IStaticDataProvider
        {
            var key = typeof(T);
            
            if (datas.ContainsKey(key))
                datas[key] = provider;
        }

        public static T Get<T>() where T : class, IStaticDataProvider
        {
            datas.TryGetValue(typeof(T), out var data);
            return data as T;
        }

#if UNITY_EDITOR
        public static List<T> FindAssets<T>(string folderName) where T : Object
        {   
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { $"Assets/DataSO/{folderName}" });

            var list = new List<T>(50);

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (obj != null)
                {
                    list.Add(obj);
                }
            }

            return list;
        }
        
        public static List<T> FindAssetsPath<T>(string path) where T : Object
        {   
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { $"Assets/{path}" });

            var list = new List<T>(50);

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (obj != null)
                {
                    list.Add(obj);
                }
            }

            return list;
        }
#endif
    }
}