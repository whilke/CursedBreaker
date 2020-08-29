using System;
using Unity.Mathematics;
using UnityEngine;

namespace enBask.Items
{
    public class WallItem : BaseItemActivator
    {
        public SpriteRenderer[] Renderers;
        public Vector2 Offset;
        public Vector2 Collider;
        public int Health;

        private LightCollider lightCollider;
        private Destructable destructable;

        private bool deleteMe = false;

        

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(this.Offset, this.Collider);
        }

        protected override bool OnActivator()
        {
            this.destructable = this.GetComponent<Destructable>();
            Vector3 pos = this.transform.TransformPoint(this.Offset);
            this.lightCollider = new LightCollider(pos, this.Collider);
            this.lightCollider.item = this.GetComponent<Destructable>();
            this.lightCollider.item.TotalHealth = this.lightCollider.item.CurrentHealth = this.Health;

            this.spawner.AddLightCollider(this.lightCollider);
            return true;
        }

        protected override bool OnDeactivate()
        {
            this.deleteMe = true;
            return false;
        }

        public void OnHit()
        {
            var p = this.destructable.CurrentHealth / (float)this.destructable.TotalHealth;

            foreach (var renderer in this.Renderers)
            {
                var c = renderer.color;
                c.a = math.lerp(0, 1, p);
                renderer.color = c;
            }
        }

        private void Update()
        {
            if (this.deleteMe)
            {
                this.spawner.RemoveLightCollider(this.lightCollider);
                this.deleteMe = false;
            }
        }
    }
}