using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Thai.Lib
{
    public class EffectPool : MonoBehaviour
    {
        public static EffectPool Instance;

        private Dictionary<string, Queue<ParticleSystem>> pool = new();
        public EffectGame[] effects;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public ParticleSystem GetEffect(string name)
        {
            if (!pool.ContainsKey(name))
            {
                pool[name] = new Queue<ParticleSystem>();
            }

            if (pool[name].Count > 0)
            {
                ParticleSystem ps = pool[name].Dequeue();
                ps.gameObject.SetActive(true);
                return ps;
            }
            else
            {
                EffectGame prefab = effects.FirstOrDefault(e => e.name == name);
                if (prefab == null)
                {
                    Debug.LogError("Effect Not Found: " + name);
                    return null;
                }

                return Instantiate(prefab.effect);
            }
        }

        public void ReturnEffect(string name, ParticleSystem ps)
        {
            ps.gameObject.SetActive(false);
            pool[name].Enqueue(ps);
        }
    }
}


