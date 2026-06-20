using System;
using System.Collections.Generic;
//using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace VFX_FBF
{
    public class EffectName : MonoBehaviour
    {
        //public List<Transform> effects = new List<Transform>();
        public Color nameColor = Color.white;
        private float fontSize = 30;
        public TMP_FontAsset fontAsset;
        
        public int columns = 5; 
        public float spacing = 2f; 

        //[Button]
        private void GenerateNameFront()
        {
            List<Transform> effects = new List<Transform>();
            foreach (Transform child in transform)
            {
                effects.Add(child);
            }
            DeleteFront(effects);
            for (int i = 0; i < effects.Count; i++)
            {
                Transform effect = effects[i];
                GameObject go = new GameObject("Effect TMP");
                TextMeshPro nameTMP = go.AddComponent<TextMeshPro>();
                nameTMP.font = fontAsset;
                nameTMP.UpdateFontAsset();
                nameTMP.text = char.ToUpper(effect.gameObject.name[0]) + effect.gameObject.name.Substring(1);
                nameTMP.color = nameColor;
                nameTMP.fontSize = fontSize;
                nameTMP.alignment = TextAlignmentOptions.Center;
                nameTMP.alignment = TextAlignmentOptions.Midline;
                nameTMP.enableWordWrapping = true;

                go.transform.SetParent(effect);
                RectTransform goRect = go.GetComponent<RectTransform>();
                goRect.anchoredPosition3D = new Vector3(0, 0, -8f);

                // Calculate grid position
                int row = i / columns;
                int col = i % columns;
                effect.localPosition = new Vector3(col * spacing, -row * spacing, effect.position.z);
            }
        }

        void DeleteFront(List<Transform> effects)
        {
            foreach (var effect in effects)
            {
                foreach (Transform child in effect)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}