using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthAndHunger : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] private float maxHealth = 100f;
    public float health;
    public float maxHealthFromPreviousLevel;

    [Header("Hunger Parameters")]
    [SerializeField] public bool enableHunger = true;
    [SerializeField] private float maxHunger = 100f;
    public float hunger;
    public float maxHungerFromPreviousLevel;

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
    [SerializeField] private Transform levelInitialSpawnPoint;

    [Header("Supply Parameters")]
    [SerializeField] public int maxSupplyAmount = 2;
    public int supplyAmount;
    public int supplyAmountFromPreviousLevel;
    public int maxSupplyAmountFromPreviousLevel;

    private PlayerUI playerUI;
    private InputManager inputManager;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerUI = GetComponent<PlayerUI>();

        if (GameObject.Find("GameManager")) {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            GetParameterFromGameManager();
        } else
        {
            health = maxHealth;
            hunger = maxHunger;
            supplyAmount = 0;
        }
        
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

        if (enableHunger) hunger -= (1f / 12f) * Time.deltaTime;

        if (health <= 0)
        {
            Die();
        }
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

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
    }

    public void Damage(float damage)
    {
        health -= damage;
        transform.position = respawnPoint.position;
        Physics.SyncTransforms();
    }

    public void Die()
    {
        /*health = maxHealth;
        transform.position = levelInitialSpawnPoint.position;
        Physics.SyncTransforms();*/
        SaveParameterForRestartLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveParameterForNextLevel()
    {
        gameManager.setPlayerValues(health, maxHealth, hunger, maxHunger, supplyAmount, maxSupplyAmount);
    }

    public void SaveParameterForRestartLevel()
    {
        gameManager.setPlayerValues(maxHealthFromPreviousLevel, maxHealthFromPreviousLevel, maxHungerFromPreviousLevel, maxHungerFromPreviousLevel, supplyAmountFromPreviousLevel, maxSupplyAmountFromPreviousLevel);
    }

    public void GetParameterFromGameManager()
    {
        health = gameManager.getPlayerHealth();
        maxHealth = gameManager.getPlayerMaxHealth();
        maxHealthFromPreviousLevel = gameManager.getPlayerMaxHealth();


        hunger = gameManager.getPlayerHunger();
        maxHunger = gameManager.getPlayerMaxHunger();
        maxHungerFromPreviousLevel = gameManager.getPlayerMaxHunger();

        supplyAmount = gameManager.getPlayerSupplyAmount();
        maxSupplyAmount = gameManager.getPlayerMaxSupplyAmount();
        supplyAmountFromPreviousLevel = gameManager.getPlayerSupplyAmount();
        maxSupplyAmountFromPreviousLevel = gameManager.getPlayerMaxSupplyAmount();

        playerUI.UpdateSupplyAmountTMP(supplyAmount.ToString());
    }

    //This functions is to add bonuses to player stats after interacting with a POI obj
    //The id functions as follows 0 - no bonuses; 1 - add 1 to max supply amount; 2 - add 20 to max health
    //3 - add 20 to max hunger; 4 - fully heal player; 5 - restore hunger bar to full; 6 - restore both health and hunger to full

    public void RecieveBoonFromPOI(int boonId)
    {
        switch (boonId)
        {
            case 0:
                break; 
            
            case 1:
                maxSupplyAmount++;
                break;

            case 2: 
                maxHealth += 20; 
                break;

            case 3:
                maxHunger += 20;
                break;

            case 4:
                health = maxHealth;
                break;

            case 5:
                hunger = maxHunger;
                break;

            case 6:
                health = maxHealth;
                hunger = maxHunger;
                break;

            default: break;
        }
    }
}
