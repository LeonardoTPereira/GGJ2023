using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using MyBox;
using Spriter2UnityDX;
using UnityEngine;

namespace Boss
{
    
    public class MadMantisHealthController : Entity.Health
    {
        [field: SerializeField] public int EnragedHP { get; private set; }
        [field: SerializeField] public int FlyingHP { get; private set; }
        
        private MadMantisManager _mantisManager;
        private EntityRenderer _spriteRenderer;

        private float _changingFormInvincibilityTime;
        private float _normalInvincibilityTime;
        
        protected override void WhenInitializeHealth()
        {
            _mantisManager = GetComponent<MadMantisManager>();
            _spriteRenderer = GetComponent<EntityRenderer>();

            _normalInvincibilityTime = invincibilityCooldown;
            _changingFormInvincibilityTime = 5 * invincibilityCooldown;
        }

        protected override void WhenKill()
        {
            _mantisManager.StartDeath();
        }

        protected override void WhenTakeDamage(int damage)
        {
            if (_health < EnragedHP && !_mantisManager.IsEnraged)
            {
                _mantisManager.StartRage();
                invincibilityCooldown = _changingFormInvincibilityTime;
            }
            else if (_health < FlyingHP && !_mantisManager.IsFlying)
            {
                _mantisManager.StartFly();
                invincibilityCooldown = _changingFormInvincibilityTime;
            }
            else
            {
                invincibilityCooldown = _normalInvincibilityTime;
            }

            StartCoroutine(StartBlinking());
        }

        protected override void WhenApplyHeal(int heal)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerator StartBlinking()
        {
            var currentTime = 0f;
            var blinkTime = 0.05f;
            var blinkingTime = _normalInvincibilityTime;
            
            while (currentTime < blinkingTime)
            {
                var originalColor = _spriteRenderer.Color;
                _spriteRenderer.Color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
                yield return new WaitForSeconds(blinkTime);
                currentTime += blinkTime;
                _spriteRenderer.Color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
                yield return new WaitForSeconds(blinkTime);
                currentTime += blinkTime;
            }
        }
        
#if UNITY_EDITOR

        [ButtonMethod]
        public void OneDamage()
        {
            TakeDamage(1);
        }

#endif
    }

    
}
