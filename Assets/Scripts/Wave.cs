using Sirenix.OdinInspector;
using UnityEngine;

namespace enBask
{
    [CreateAssetMenu(fileName = "wave", menuName = "Data/Wave", order = 0)]
    public class Wave : ScriptableObject
    {
        public Vector2 Position = Vector2.zero;
        public Vector2 Size = Vector2.one;

        public int MinCount;
        public int MaxCount;

        [PropertyRange(0f, 1f)]
        public float CurseChance;

        public float StartDelay;
        public float MaxWaveCount;
        public float Delay;
        public bool Repeat;
        public bool SpawnAtStart;
        public CurseData[] Curses;
    }
}