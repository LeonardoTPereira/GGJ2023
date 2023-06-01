using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Health : Entity.Health
    {
        [SerializeField] private ParticleSystem explosionDeathParticle;
        [SerializeField] private ParticleSystem damageBloodParticle;
        [SerializeField] private Collider2D damageCollider;
        [SerializeField] private DamageEffect damageEffect;
        protected override void WhenInitializeHealth()
        {
            //Do nothing
        }

        protected override void WhenKill()
        {
            //Trigger death animation
            damageCollider.enabled = false;
            var explosionDeathObject = Instantiate(explosionDeathParticle, transform.position, Quaternion.identity);
            explosionDeathObject.Play();
            Destroy(gameObject, timeToDestroyObject);
        }

        protected override void WhenTakeDamage(int damage)
        {
            //Trigger take damage animation
            damageBloodParticle.Play();
            damageEffect.BlinkDamage();
        }

        protected override void WhenApplyHeal(int heal)
        {
            //Trigger heal animation
        }
    }

    
}