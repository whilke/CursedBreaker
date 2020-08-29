using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace enBask.Items
{
    public class CureCurseItem : BaseItemActivator
    {
        [PropertyRange(0,1)]
        public float Chance;

        protected override bool OnActivator()
        {
            bool didMove = false;
            List<CharacterData> characters = this.GetCharactersInRange()
                                                 .OfType<CharacterData>()
                                                 .ToList();

            foreach (CharacterData c in characters.Where(c => c.infected))
            {
                CharacterData character = c;

                bool healed = false;
                var r = UnityEngine.Random.Range(0f, 01f);
                if (r <= this.Chance)
                {
                    if (character.infected && character.incubated && !character.permanente)
                    {
                        character = this.spawner.HealCharacter(character);
                        this.UpdateCharacter(character);
                        healed = true;
                    }
                }

                if (!healed)
                {
                    var d = ((Vector2)this.transform.position - character.position).normalized;
                    var move = -d * 15f * Time.deltaTime;
                    var pos = character.position + move;


                    character.position = pos;
                    character.renderer.transform.position = pos;
                    this.UpdateCharacter(character);
                    didMove = true;
                }
            }

            return didMove;
        }
    }
}