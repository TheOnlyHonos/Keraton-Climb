using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAndHunger : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] private float maxHealth = 100f;
    private float health;

    [Header("Hunger Parameters")]
    [SerializeField] private float maxHunger = 100f;
    private float hunger;

    [Header("Bar Animation Parameters")]
    [SerializeField] private float chipSpeed = 2f;
    private float lerpTimerHealth;
    private float lerpTimerHunger;

    [Header("HealthBar Images")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;

    [Header("HungerBar Images")]
    [SerializeField] private Image frontHungerBar;
    [SerializeField] private Image backHungerBar;

    [Header("Respawn Point")]
    [SerializeField] private Transform respawnPoint;

    [Header("Supply Parameters")]
    [SerializeField] public int maxSupplyAmount = 2;
    public int supplyAmount;

    private PlayerUI playerUI;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerUI = GetComponent<PlayerUI>();

        health = maxHealth;
        hunger = maxHunger;

        supplyAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        hunger = Mathf.Clamp(hunger, 0, maxHunger);

        UpdateHealthAndHungerUI();

        if (inputManager.onFoot.UseSupply.triggered)
        {
            UseSupply();
        }

        hunger -= (1f / 12f) * Time.deltaTime;

        if (health <= 0)
        {
            Die(.3f);
        }

        Debug.Log(health + " " + hunger);
    }

    public void UpdateHealthAndHungerUI()
    {
        float fillHealthFront = frontHealthBar.fillAmount;
        float fillHealthBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;

        float fillHungerFront = frontHungerBar.fillAmount;
        float fillHungerBack = backHungerBar.fillAmount;
        float hungerFraction = hunger / maxHunger;


        //Healthbar animation when taking damage
        if (fillHealthBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            lerpTimerHealth += Time.deltaTime;
            float percentComplete = lerpTimerHealth / chipSpeed;
            percentComplete *= percentComplete;

            backHealthBar.fillAmount = Mathf.Lerp(fillHealthBack, healthFraction, percentComplete);
        }

        //Healthbar animation when getting healed
        if (fillHealthFront < healthFraction)
        {
            backHealthBar.fillAmount = healthFraction;
            lerpTimerHealth += Time.deltaTime;
            float percentComplete = lerpTimerHealth / chipSpeed;
            percentComplete *= percentComplete;

            frontHealthBar.fillAmount = Mathf.Lerp(fillHealthFront, backHealthBar.fillAmount, percentComplete);
        }

        //Hungerbar animation when hunger is reduced
        if (fillHungerBack > hungerFraction)
        {
            frontHungerBar.fillAmount = hungerFraction;
            lerpTimerHunger += Time.deltaTime;
            float percentComplete = lerpTimerHunger / chipSpeed;
            percentComplete *= percentComplete;

            backHungerBar.fillAmount = Mathf.Lerp(fillHungerBack, hungerFraction, percentComplete);
        }

        //Hungerbar animation when hunger is restored
        if (fillHungerFront < hungerFraction)
        {
            backHungerBar.fillAmount = hungerFraction;
            lerpTimerHunger += Time.deltaTime;
            float percentComplete = lerpTimerHunger / chipSpeed;
            percentComplete *= percentComplete;

            frontHungerBar.fillAmount = Mathf.Lerp(fillHungerFront, backHungerBar.fillAmount, percentComplete);
        }
    }

    public void UseSupply()
    {
        if (supplyAmount > 0)
        {
            supplyAmount--;
            RestoreHealth(30);
            RestoreHunger(30);
            playerUI.UpdateSupplyAmountTMP(supplyAmount.ToString());
        }
    }

    public void AddSupply()
    {
        supplyAmount++;
        playerUI.UpdateSupplyAmountTMP(supplyAmount.ToString());
    }

    public void CantAddMoreSupplyTMP()
    {
        playerUI.ShowCantAddMoreSupplyTMP();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimerHealth = 0f;
    }

    public void RestoreHealth(float heal)
    {
        health += heal;
        lerpTimerHealth = 0f;
    }

    public void ReduceHunger(float damage)
    {
        hunger -= damage;
        lerpTimerHunger = 0f;
    }

    public void RestoreHunger(float heal)
    {
        hunger += heal;
        lerpTimerHunger = 0f;
    }

    public void Die(float healthRatioAtRespawn)
    {
        health = maxHealth * healthRatioAtRespawn;
        transform.position = respawnPoint.position;
        Physics.SyncTransforms();
    }
}
