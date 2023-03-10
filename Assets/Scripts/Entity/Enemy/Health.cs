using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public class Health : Entity.Health
    {
        protected override void WhenInitializeHealth()
        {
            //Do nothing
        }

        protected override void WhenKill()
        {
            //Trigger death animation
        }

        protected override void WhenTakeDamage(int damage)
        {
            //Trigger take damage animation
        }

        protected override void WhenApplyHeal(int heal)
        {
            //Trigger heal animation
        }
    }

    
}