using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPS
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        private static object _lock = new object();

        public static T S
        {
            get
            {
                if (applicationIsQuitting) return null;

                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                            return instance;

                        if (instance == null)
                        {
                            GameObject singletonPrefab = null;
                            GameObject singleton = null;

                            singletonPrefab = (GameObject)Resources.Load(typeof(T).ToString(), typeof(GameObject));

                            if (singletonPrefab != null)
                            {
                                singleton = Instantiate(singletonPrefab);
                                instance = singleton.GetComponent<T>();
                            }
                            else
                            {
                                singleton = new GameObject();
                                instance = singleton.AddComponent<T>();
                            }

                            singleton.name = typeof(T).ToString();
                            DontDestroyOnLoad(singleton);
                        }
                    }

                    return instance;
                }
            }
        }

        private static bool applicationIsQuitting = false;
        public virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }

}