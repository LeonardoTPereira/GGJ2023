using System;
using System.Collections;
using Gameplay.Events;
using UnityEngine;
using Gameplay.Bullets;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static event InitializeHealthEventHandler InitializePlayerHealthEventHandler;

        public static event EventHandler PlayerDiedEventHandler;

        public static event TakeDamageEventHandler PlayerTakeDamageEventHandler;

        [SerializeField] private bool canTakeDamage;
        [SerializeField] private int health = 5;
        [SerializeField] private int maxHealth = 5;
        private readonly float invincibilityCooldown = 0.5f;

        private void Awake()
        {
            CanTakeDamage = true;
        }

        private void Start()
        {
            InitializePlayerHealthEventHandler?.Invoke(this, new InitializeHealthEventArgs(maxHealth));
        }

        private void OnEnable()
        {
            BulletController.PlayerHitEventHandler += TakeDamage;
        }

        private void OnDisable()
        {
            BulletController.PlayerHitEventHandler -= TakeDamage;
        }

        private void TakeDamage(object sender, BulletHitEventArgs eventArgs)
        {
            if (!CanTakeDamage) return;
            Health -= eventArgs.Bullet.Damage;
            CheckDeathAndKill();
            PlayerTakeDamageEventHandler?.Invoke(this, new TakeDamageEventArgs(eventArgs.Bullet.Damage));
            StartCoroutine(CountInvincibilityCooldown());
        }

        private void CheckDeathAndKill()
        {
            if (Health > 0) return;
            //TODO Play Death Sound and Spawn Particle
            PlayerDiedEventHandler?.Invoke(null, EventArgs.Empty);
            Destroy(gameObject);
        }

        private IEnumerator CountInvincibilityCooldown()
        {
            CanTakeDamage = false;
            yield return new WaitForSeconds(invincibilityCooldown);
            CanTakeDamage = true;
        }

        public bool CanTakeDamage
        {
            get => canTakeDamage;
            set => canTakeDamage = value;
        }

        public int Health
        {
            get => health;
            set => health = value;
        }
    }
}