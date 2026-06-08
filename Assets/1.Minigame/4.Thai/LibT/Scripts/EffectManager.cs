using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thai.Lib
{
    public class Option
    {
        public bool isDontDestroy = false;
        public bool isTransform = false;
        public Vector3 offset = Vector3.zero;
        public float timeDestroy = 2;
        public float scaleRate = 1;
        public Vector3? startScale;
        public Vector3? position;
    }

    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;
        public EffectGame[] effects;

        private Dictionary<string, Queue<ParticleSystem>> pool = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private ParticleSystem GetFromPool(string name)
        {
            if (!pool.ContainsKey(name))
            {
                pool[name] = new Queue<ParticleSystem>();
            }

            if (pool[name].Count > 0)
            {
                var effect = pool[name].Dequeue();
                effect.gameObject.SetActive(true);
                effect.Play();
                return effect;
            }

            EffectGame e = Array.Find(effects, x => x.name == name);
            if (e == null)
            {
                Debug.LogError("Effect Not Found: " + name);
                return null;
            }

            return Instantiate(e.effect);
        }

        private void ReturnToPool(string name, ParticleSystem ps)
        {
            ps.gameObject.SetActive(false);
            pool[name].Enqueue(ps);
        }

        public ParticleSystem CreatedEffect(string name, Transform t, Option option = null)
        {
            option ??= new();

            ParticleSystem e1 = GetFromPool(name);
            if (!e1) return null;

            if (option.isTransform)
            {
                e1.transform.SetParent(t);
            }

            if (t != null)
            {
                e1.transform.position = option.position ?? (t.position + option.offset);
            }

            e1.transform.localScale = option?.startScale ?? e1.transform.localScale * option.scaleRate;
            e1.Play();

            if (!option.isDontDestroy)
            {
                StartCoroutine(ReturnAfterTime(name, e1, option.timeDestroy));
            }

            return e1;
        }

        public void CreatedEffect(string name, Vector3 position, Option option = null)
        {
            option ??= new();

            ParticleSystem e1 = GetFromPool(name);
            if (e1 == null) return;

            e1.transform.position = position + option.offset;
            e1.Play();

            if (!option.isDontDestroy)
            {
                StartCoroutine(ReturnAfterTime(name, e1, option.timeDestroy));
            }
        }

        public ParticleSystem CreatedEffect(ParticleSystem prefab, Transform t, Option option = null)
        {
            option ??= new();

            if (prefab == null)
            {
                Debug.LogError("Effect is null");
                return null;
            }

            ParticleSystem e1 = Instantiate(prefab);

            if (option.isTransform)
            {
                e1.transform.SetParent(t);
            }

            e1.transform.position = option.position ?? (t.position + option.offset);
            e1.transform.localScale = option?.startScale ?? e1.transform.localScale * option.scaleRate;

            e1.Play();

            if (!option.isDontDestroy)
            {
                Destroy(e1.gameObject, option.timeDestroy); // Still destroy if using external prefab
            }

            return e1;
        }

        private IEnumerator ReturnAfterTime(string name, ParticleSystem ps, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnToPool(name, ps);
        }
    }

    [Serializable]
    public class EffectGame
    {
        public string name;
        public ParticleSystem effect;
    }
}
