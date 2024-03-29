using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Gameplay.Bullets;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        private struct BulletData
        {
            public GameObject BulletObject;
            public BulletSO BulletSo;
        }

        [SerializeField] private GameObject primaryBullet;
        [SerializeField] private GameObject secondaryBullet;
        [SerializeField] private Transform[] primaryWeapon;
        [SerializeField] private Transform[] secondaryWeapon;

        [SerializeField] private ParticleSystem usedBulletsParticle;
        [SerializeField] private ParticleSystem muzzleEffectParticle;
        [SerializeField] private ParticleSystem smokeParticle;
        [SerializeField] private UnityEngine.Animator _anim;

        private bool _canShoot;
        private bool _inputEnabled;
        private bool _isHoldingShoot;
        private BulletData _primaryBulletData;
        private BulletData _secondaryBulletData;
        [SerializeField] private float cooldownBonus = 1;

        private void Awake()
        {
            _canShoot = true;
            _isHoldingShoot = true;
            _inputEnabled = true;
        }

        private void Start()
        {
            _primaryBulletData.BulletObject = PrimaryBullet;
            _primaryBulletData.BulletSo = PrimaryBullet.GetComponent<BulletController>().Bullet;
            _secondaryBulletData.BulletObject = SecondaryBullet;
            _secondaryBulletData.BulletSo = SecondaryBullet.GetComponent<BulletController>().Bullet;
        }

        private void OnEnable()
        {
            global::Player.Health.OnPlayerDied += DisableInput;
        }

        private void OnDisable()
        {
            global::Player.Health.OnPlayerDied -= DisableInput;
        }

        public void ShootPrimaryWeapon(InputAction.CallbackContext context)
        {
            if (!_inputEnabled) return;
            if (context.performed)
            {
                _isHoldingShoot = true;
            }
            else if (context.canceled)
            {
                _isHoldingShoot = false;
            }
            StartCoroutine(Shoot(_primaryBulletData, PrimaryWeapon));
        }

        public void ShootSecondaryWeapon(InputAction.CallbackContext context)
        {
            if (!_inputEnabled) return;
            if (context.performed)
            {
                _isHoldingShoot = true;
            }
            else if (context.canceled)
            {
                _isHoldingShoot = false;
            }
            StartCoroutine(Shoot(_secondaryBulletData, SecondaryWeapon));
        }

        private IEnumerator Shoot(BulletData bullet, Transform[] spawnPoints)
        {
            while (_isHoldingShoot)
            {
                yield return null;
                if (!_canShoot) continue;
                _anim.SetBool("isShooting", true);
                AudioManager.Instance.PlaySFX("Player_Tiro");
                foreach (var spawnPoint in spawnPoints)
                {
                    Quaternion spawnRotation = new Quaternion(Quaternion.AngleAxis(0, Vector3.right).x, spawnPoint.rotation.y, 0, spawnPoint.rotation.w);
                    Instantiate(bullet.BulletObject, spawnPoint.position, spawnRotation);
                    usedBulletsParticle.Play();
                    //muzzleEffectParticle.Play();
                }
                StartCoroutine(CountCooldown(bullet.BulletSo.Cooldown));
            }
            smokeParticle.Play();
            _anim.SetBool("isShooting", false);
        }

        private IEnumerator CountCooldown(float bulletCooldown)
        {
            _canShoot = false;
            yield return new WaitForSeconds(bulletCooldown / CooldownBonus);
            _canShoot = true;
        }

        public GameObject PrimaryBullet => primaryBullet;
        public GameObject SecondaryBullet => secondaryBullet;
        public Transform[] PrimaryWeapon => primaryWeapon;
        public Transform[] SecondaryWeapon => secondaryWeapon;

        public float CooldownBonus
        {
            get => cooldownBonus;
            set => cooldownBonus = value;
        }

        private void DisableInput()
        {
            _inputEnabled = false;
        }
    }
}