using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace enBask.Items
{
    public class StickyBombItem : BaseItemActivator
    {
        Dictionary<SpriteRenderer, float> oldSpeeds = new Dictionary<SpriteRenderer, float>();
        protected override bool OnActivator()
        {
            var items = this.GetCharactersInRange()
                            .OfType<CharacterData>()
                            .Where(x => x.infected && x.incubated)
                            .ToArray();


            foreach (var item in items)
            {
                CharacterData character = item;
                this.oldSpeeds[character.renderer] = character.speed;
                character.speed = 0.01f;
                this.spawner.UpdateCharacter(character);
            }
            return false;
        }

        protected override bool OnDeactivate()
        {
            foreach (var kvp in this.oldSpeeds)
            {
                var character = this.spawner.FindCharacter(kvp.Key);
                if (character.renderer == kvp.Key)
                {
                    character.speed = kvp.Value;
                    this.spawner.UpdateCharacter(character);
                }
            }

            return false;
        }
    }
}