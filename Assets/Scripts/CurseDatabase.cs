using System.Collections.Generic;
using UnityEngine;

namespace enBask
{
    [CreateAssetMenu(fileName = "curseDatabase", menuName = "Data/CurseDatabase", order = 0)]
    public class CurseDatabase : ScriptableObject
    {
        public CurseData[] Curses;
    }
}