using Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace Player
{
    public class Health : Entity.Health
    {
        [SerializeField] private UnityEngine.Animator _anim;
        [SerializeField] private DamageEffect damageEffect;

        public static event Action<int> OnInitializePlayerHealth;

        public static event Action OnPlayerDied;

        public static event Action<int> OnPlayerTakeDamage;

        public static event Action<int> OnPlayerApplyHeal;

        [SerializeField] private ParticleSystem damageParticle;
        [SerializeField] private ParticleSystem deathParticle;

        protected override void WhenInitializeHealth()
        {
            OnInitializePlayerHealth?.Invoke(maxHealth);
            transform.parent.transform.position = SpawnManager.Instance.GetSpawnPoint().position; //CHANGE THAT
            if (SpawnManager.Instance.CameraBound == null) return;
            var _virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera");
            var _confiner = _virtualCamera.GetComponent<CinemachineConfiner2D>();
            _confiner.m_BoundingShape2D = SpawnManager.Instance.CameraBound;
        }

        protected override void WhenKill()
        {
            Debug.Log("GAME OVER");
            deathParticle.Play();
            _anim.SetTrigger("Death");
            OnPlayerDied?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        protected override void WhenTakeDamage(int damage)
        {
            OnPlayerTakeDamage?.Invoke(damage);
            _anim.SetTrigger("Damage");
            damageParticle.Play();
            damageEffect.BlinkDamage();
            AudioManager.Instance.PlaySFX("Player_Tomando_Dano");
        }

        protected override void WhenApplyHeal(int heal)
        {
            OnPlayerApplyHeal?.Invoke(heal);
        }
    }
}