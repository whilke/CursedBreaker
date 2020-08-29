namespace enBask.curses
{
    public class FastCurseEffect : BaseCurseEffect
    {
        public override CharacterData Execute(CharacterData character)
        {
            float orgSpeed = character.speed;
            character.speed *= 2f;

            character = this.spawner.RandomCharacterMove(character);

            character.speed = orgSpeed;
            return character;
        }
    }
}