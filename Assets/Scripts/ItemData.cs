using System;
using System.Reflection;
using enBask.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace enBask
{
    [CreateAssetMenu(fileName = "itemData", menuName = "Data/ItemData", order = 0)]
    public class ItemData : SerializedScriptableObject
    {
        public Sprite Icon;
        public string Name;

        public float Chance;
        public BaseItemActivator Activator;
    }
}