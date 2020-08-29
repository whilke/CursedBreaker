using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace enBask.Items
{
    public class CurseCollectorItem : BaseItemActivator
    {
        public float Speed;

        protected override bool OnActivator()
        {
            bool didMove = false;
            List<CharacterData> characters = this.GetCharactersInRange()
                                                 .OfType<CharacterData>()
                                                 .ToList();

            foreach (CharacterData c in characters.Where(c => c.infected))
            {
                CharacterData character = c;

                didMove = true;
                var d = ((Vector2) this.transform.position - character.position).normalized;
                var move = d * this.Speed * Time.deltaTime;
                var pos = character.position + move;


                Debug.DrawLine(character.position, pos, Color.magenta);

                character.position = pos;
                character.renderer.transform.position = pos;
                this.UpdateCharacter(character);

            }

            return didMove;
        }
    }
}