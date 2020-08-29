using enBask.Items;
using UnityEngine;

namespace enBask
{
    public class LightCollider : IQuadTreeObject
    {
        public Rect Area;
        public Destructable item;

        public LightCollider(Vector2 center, Vector2 size)
        {
            Bounds b = new Bounds(center, size);
            this.Area = new Rect(b.min, b.size);
        }

        public bool Collides(Vector2 pos, float radius=1f)
        {
            var minPos = new Vector2(pos.x - radius / 2, pos.y - radius / 2);
            var temp = new Rect(minPos, new Vector2(radius*2, radius*2));
            return this.Area.Overlaps(temp);
        }

        public void Hit(int damage=1)
        {
            if (this.item)
            {
                this.item.Hit(damage);
            }
        }


        public Vector2 GetPosition()
        {
            return this.Area.center;
        }
    }
}