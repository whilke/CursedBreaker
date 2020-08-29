using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace enBask.curses
{
    public class HomingCurseEffect : BaseCurseEffect
    {
        private List<IQuadTreeObject> objects = new List<IQuadTreeObject>();

        public override CharacterData PostExecute(CharacterData character, out bool modified)
        {
            modified = false;
            Bounds b = new Bounds(character.position, new Vector2(2f, 2f));
            Rect r = new Rect(b.min, b.size);

            this.objects.Clear();
            this.spawner.quadTree
                .RetrieveObjectsInAreaNoAlloc(r, ref this.objects);

            var healthy = this.objects
                              .OfType<CharacterData>()
                              .Where(x => !x.infected)
                              .ToArray();

            var closestToUs = healthy
                              .OrderBy(x => (character.position - x.position).sqrMagnitude)
                              .FirstOrDefault();
            if (closestToUs.renderer != null)
            {
                modified = true;
                character.direction = (closestToUs.position - character.position).normalized;
                var oldSpeed = character.speed;
                character.speed = 1f;
                character = this.spawner.MoveCharacter(character);
                character.speed = oldSpeed;
            }


            return character;
        }

        public override CharacterData Execute(CharacterData character)
        {
            return character;
        }
    }
}