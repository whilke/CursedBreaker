using UnityEngine;

namespace enBask.curses
{
    public abstract class BaseCurseEffect
    {
        protected CharacterSpawner spawner;

        protected BaseCurseEffect()
        {
            this.spawner = GameObject.FindObjectOfType<CharacterSpawner>();
        }

        public abstract CharacterData Execute(CharacterData character);

        public virtual CharacterData PostExecute(CharacterData character, out bool modified)
        {
            modified = false;
            return character;
        }

        public virtual int AttackWall(CharacterData character, LightCollider wall)
        {
            return 1;
        }
    }
}