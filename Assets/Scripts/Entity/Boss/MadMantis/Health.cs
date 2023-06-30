using System.Collections;
using MyBox;
using Spriter2UnityDX;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Boss.MadMantis
{
    public class Health : Entity.Health
    {
        [field: SerializeField] public int EnragedHP { get; private set; }
        [field: SerializeField] public int FlyingHP { get; private set; }
        //[SerializeField] protected float timeToDestroyObject = 0.5f;

        private Manager _mantisManager;
        private EntityRenderer _spriteRenderer;

        private float _changingFormInvincibilityTime;
        private float _normalInvincibilityTime;

        protected override void WhenInitializeHealth()
        {
            _mantisManager = GetComponent<Manager>();
            _spriteRenderer = GetComponent<EntityRenderer>();

            _normalInvincibilityTime = invincibilityCooldown;
            _changingFormInvincibilityTime = 5 * invincibilityCooldown;
        }

        protected override void WhenKill()
        {
            _mantisManager.PlayDeathParticle();
            _mantisManager.StartDeath();
        }

        protected override void WhenTakeDamage(int damage)
        {
            _mantisManager.PlayDamageSound();

            if (_health < EnragedHP && !_mantisManager.IsEnraged)
            {
                _mantisManager.StartRage();
                invincibilityCooldown = _changingFormInvincibilityTime;
            }
            else if (_health < FlyingHP && !_mantisManager.IsFlying)
            {
                _mantisManager.StartFinalStageTransition();
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

        public void DestroyBoss()
        {
            Destroy(gameObject, timeToDestroyObject);
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