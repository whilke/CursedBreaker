using UnityEngine;

namespace enBask
{
    [CreateAssetMenu(fileName = "itemDatabase", menuName = "Data/ItemDatabase", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        public ItemData[] Items;
    }
}