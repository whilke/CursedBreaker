namespace enBask.curses
{
    public class SlowCurseEffect : BaseCurseEffect
    {
        public override CharacterData Execute(CharacterData character)
        {
            float orgSpeed = character.speed;
            character.speed /= 4f;

            character = this.spawner.RandomCharacterMove(character);
            character.speed = orgSpeed;
            return character;
        }
    }
}