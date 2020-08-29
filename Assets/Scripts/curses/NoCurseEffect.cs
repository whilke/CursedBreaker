namespace enBask.curses
{
    public class NoCurseEffect : BaseCurseEffect
    {
        public override CharacterData Execute(CharacterData character)
        {
            return this.spawner.RandomCharacterMove(character);
        }
    }
}