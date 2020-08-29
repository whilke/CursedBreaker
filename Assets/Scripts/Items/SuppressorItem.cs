using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace enBask.Items
{
    public class SuppressorItem : BaseItemActivator
    {
        private CurseData curse;
        
        protected override bool OnActivator()
        {
            var items = this.GetCharactersInRange()
                            .OfType<CharacterData>()
                            .Where(x => x.infected)
                            .ToArray();

            var groups = items.GroupBy(x => x.curse);
            var bestCurse = groups.OrderByDescending(g => g.Count()).FirstOrDefault()?.Key;
            if (bestCurse == null) return false;

            this.curse = bestCurse;
            if (!this.spawner.CurseSuppresions.TryGetValue(bestCurse, out int count))
            {
                this.spawner.CurseSuppresions[bestCurse] = 1;
            }
            else
            {
                this.spawner.CurseSuppresions[bestCurse] = count +1;
            }

            foreach (var item in items)
            {
                if (item.curse == bestCurse && !item.permanente)
                {
                    var character = this.spawner.HealCharacter(item);
                    this.spawner.UpdateCharacter(character);
                }
            }
            return false;
        }

        protected override bool OnDeactivate()
        {
            if (this.curse == null) return false;

            if (this.spawner.CurseSuppresions.TryGetValue(this.curse, out int count))
            {
                count--;
                if (count <= 0)
                {
                    this.spawner.CurseSuppresions.Remove(this.curse);
                }
                else
                {
                    this.spawner.CurseSuppresions[this.curse] = count;

                }
            }

            return false;
        }
    }
}