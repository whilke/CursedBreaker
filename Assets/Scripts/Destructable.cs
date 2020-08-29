using UnityEngine;
using UnityEngine.Events;

namespace enBask
{
    public class Destructable : MonoBehaviour
    {
        public UnityEvent OnDie;
        public UnityEvent OnHit;

        public int TotalHealth;
        public int CurrentHealth;

        public void Die()
        {
            this.OnDie?.Invoke();
        }

        public void Hit(int damage)
        {
            this.CurrentHealth -= damage;
            if (this.CurrentHealth <= 0)
            {
                this.CurrentHealth = 0;
                this.Die();
            }
            this.OnHit?.Invoke();
        }
    }
}