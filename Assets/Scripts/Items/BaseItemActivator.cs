using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enBask.Items
{
    public abstract class BaseItemActivator : MonoBehaviour
    {
        public float DestroyTime = 10f;
        public float ActivatorTime;
        public float Duration;
        public Vector2 Range;
        public bool OneTime;
        public bool RunAtStart;

        protected float destroyAccumulator;
        protected float accumulator;
        protected CharacterSpawner spawner;

        private bool running;
        private List<IQuadTreeObject> characterCache = new List<IQuadTreeObject>();

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = this.running ? Color.red : Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, Range);
        }

        private void Awake()
        {
            this.spawner = GameObject.FindObjectOfType<CharacterSpawner>();
            this.StartCoroutine(this.ActivatorLoop());

            if (this.DestroyTime > 0f)
            {
                this.StartCoroutine(this.DestroyAfterTime(this.DestroyTime));
            }
        }

        protected void UpdateCharacter(CharacterData character)
        {
            this.spawner.UpdateCharacter(character);
        }

        protected List<IQuadTreeObject> GetCharactersInRange()
        {
            this.characterCache.Clear();

            var b = new Bounds(this.transform.position, this.Range);
            var r = new Rect(b.min, b.size);
            this.spawner.quadTree.RetrieveObjectsInAreaNoAlloc(r, ref this.characterCache);
            return this.characterCache;
        }

        IEnumerator ActivatorLoop()
        {
            if (this.RunAtStart)
            {
                this.accumulator = this.ActivatorTime - Time.deltaTime;
            }

            while (true)
            {
                this.accumulator += Time.deltaTime;

                if (this.accumulator >= this.ActivatorTime)
                {

                    yield return this.RunActivator();
                    this.accumulator -= this.ActivatorTime;
                }

                yield return null;
            }
        }


        IEnumerator RunActivator()
        {
            float a = this.Duration;
            this.running = true;
            while (a >= 0)
            {
                yield return new WaitForFixedUpdate();
                bool  modified = this.OnActivator();
                
                if (modified)
                {
                    this.spawner.RebuildQuadTree();
                }

                if (this.OneTime)
                {
                    while (true)
                    {
                        yield return null;
                    }
                }
                a -= Time.deltaTime;
            }

            this.running = false;
        }

        private IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            this.Die();
        }

        protected abstract bool OnActivator();

        protected virtual bool OnDeactivate()
        {
            return false;
        }

        public virtual void Die()
        {
            bool changed = this.OnDeactivate();
            if (changed)
            {
                this.spawner.RebuildQuadTree();
            }
            Destroy(this.gameObject, 0.1f);
        }
    }
}