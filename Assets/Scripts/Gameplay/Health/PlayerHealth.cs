using Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : HealthController
{
    [SerializeField] private Animator _anim;
    public static PlayerHealth Instance;
    public static event Action<int> InitializePlayerHealthEvent;
    public static event Action PlayerDiedEvent;
    public static event Action<int> PlayerTakeDamageEvent;
    public static event Action<int> PlayerTakeHealEvent;

    [SerializeField] private ParticleSystem damageParticle;
    [SerializeField] private ParticleSystem deathParticle;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }

    protected override void InitializeHealth()
    {
        base.InitializeHealth();
        InitializePlayerHealthEvent?.Invoke(maxHealth);
        this.transform.position = SpawnManager.Instance.GetSpawnPoint().position;
    }

    protected override void Kill()
    {
        Debug.Log("GAME OVER");
        deathParticle.Play();
        _anim.SetTrigger("Death");
        PlayerDiedEvent?.Invoke();
        base.Kill();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        if (base.GetCanTakeDamage())
        {
            PlayerTakeDamageEvent?.Invoke(damage);
            _anim.SetTrigger("Damage");
            damageParticle.Play();
            base.TakeDamage(damage);
        }
    }

    public override void ApplyHeal(int heal)
    {
        base.ApplyHeal(heal);
        PlayerTakeHealEvent?.Invoke(heal);
    }

    // Talvez precise melhorar para adaptar com diferentes tipos de dano
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Enemy":
                TakeDamage(1);
                break;
            case "Heal":
                ApplyHeal(1);
                break;
            //TEMPORARY ##############################################################
            case "Win":
                WinMenu.Instance.DisplayWinMenu();
                break;
        }
    }
}
