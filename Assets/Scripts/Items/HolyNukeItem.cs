using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace enBask.Items
{
    public class HolyNukeItem : BaseItemActivator
    {
        public float Radius = 20f;

        [PropertyRange(0, 1)] public float KillChance;

        protected override bool OnActivator()
        {
            var b = new Bounds(this.transform.position, new Vector2(this.Radius, this.Radius));
            var r = new Rect(b.min, b.size);
            var items = this.spawner.quadTree.RetrieveObjectsInArea(r);

            foreach (var item in items)
            {
                if (item is CharacterData character)
                {
                    if (character.infected && character.incubated && !character.permanente)
                    {
                        character = this.spawner.HealCharacter(character);
                        this.spawner.UpdateCharacter(character);
                    }

//                     if (!character.infected)
//                     {
//                         if (UnityEngine.Random.value <= this.KillChance)
//                         {
//                         }
//                     }
                }
//                 else if (item is LightCollider collider)
//                 {
//                     collider.Hit(500000000);
//                 }
            }
            return false;
        }
    }
}