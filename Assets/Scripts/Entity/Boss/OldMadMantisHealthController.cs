using MyBox;
using Spriter2UnityDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Boss
{
    public class OldMadMantisHealthController : Enemy.OldHealth
    {
        [field: SerializeField] public int EnragedHP { get; private set; }
        [field: SerializeField] public int FlyingHP { get; private set; }
        [field: SerializeField] public float InvincibilityTime { get; private set; }
        private bool _isInvincible;

        private MadMantisManager _mantisManager;
        private EntityRenderer _spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            _isInvincible = false;
            _mantisManager = GetComponent<MadMantisManager>();
            _spriteRenderer = GetComponent<EntityRenderer>();
        }

        public override void TakeDamage(int amount)
        {
            if (_isInvincible) return;
            currentHealth -= amount;

            if (currentHealth < EnragedHP && !_mantisManager.IsEnraged)
            {
                _mantisManager.StartRage();
                StartCoroutine(InvincibilityCooldown(InvincibilityTime * 5, false));
            }
            else if (currentHealth < FlyingHP && !_mantisManager.IsFlying)
            {
                _mantisManager.StartFly();
                StartCoroutine(InvincibilityCooldown(InvincibilityTime * 5, true));
            }
            else if (currentHealth <= 0)
            {
                _mantisManager.StartDeath();
            }
            else
            {
                StartCoroutine(InvincibilityCooldown(InvincibilityTime, true));
            }
        }

        private IEnumerator InvincibilityCooldown(float time, bool flicker)
        {
            var currentTime = 0f;
            var blinkTime = 0.05f;
            _isInvincible = true;
            if (flicker)
            {
                while (currentTime < time)
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
            else
            {
                yield return new WaitForSeconds(time);
            }

            _isInvincible = false;
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