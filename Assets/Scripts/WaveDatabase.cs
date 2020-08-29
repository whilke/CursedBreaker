using UnityEngine;

namespace enBask
{
    [CreateAssetMenu(fileName = "waveDatabase", menuName = "Data/WaveDatabase", order = 0)]
    public class WaveDatabase : ScriptableObject
    {
        public Wave[] Items;
    }
}