using System;
using System.Collections.Generic;

namespace enBask.curses
{
    public static class CurseEffectManager
    {
        private static Dictionary<Type, BaseCurseEffect> EFFECTS = new Dictionary<Type, BaseCurseEffect>();

        static CurseEffectManager()
        {
            CurseEffectManager.RegisterEffect<NoCurseEffect>();
            CurseEffectManager.RegisterEffect<SlowCurseEffect>();
            CurseEffectManager.RegisterEffect<FastCurseEffect>();
            CurseEffectManager.RegisterEffect<HomingCurseEffect>();
        }

        public static void RegisterEffect<T>() where T : BaseCurseEffect, new()
        {
            CurseEffectManager.EFFECTS[typeof(T)] = new T();
        }

        public static CharacterData Execute(Type effectType, CharacterData character)
        {
            return CurseEffectManager.EFFECTS.TryGetValue(effectType, out BaseCurseEffect effect)
                       ? effect.Execute(character) : character;
        }

        public static CharacterData PostExecute(Type effectType, CharacterData character, out bool modified)
        {
            modified = false;
            return CurseEffectManager.EFFECTS.TryGetValue(effectType, out BaseCurseEffect effect)
                       ? effect.PostExecute(character, out modified) : character;
        }

        public static int AttackWall(CharacterData character, LightCollider wall)
        {
            return CurseEffectManager.EFFECTS.TryGetValue(character.curse.Effect, out BaseCurseEffect effect)
                       ? effect.AttackWall(character, wall) : 0;
        }
    }
}