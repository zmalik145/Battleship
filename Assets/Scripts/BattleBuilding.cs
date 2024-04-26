using TMPro;
using UnityEngine;

public class BattleBuilding : MonoBehaviour
{
    [SerializeField] private GameObject building; 
    [SerializeField] private GameObject cross; 
    [SerializeField] private GameObject missed;
    [SerializeField] private TextMeshPro healthText;
    [SerializeField] private int maxHealth = 3;

    public bool HasBuilding { get; set; } //Indicates whether this cell has a building placed on it.
    public bool IsDead { get; set; } //Indicates whether the building on this cell has been destroyed
    public bool IsShaking { get; set; }

    private int currentHealth; //Tracks the building's current health

    private Vector3 originalPosition;
    private float shakeTimer; //Manages a timer for the shaking animation 

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        originalPosition = transform.position;
    }

    // Handles the shaking animation if triggered
    private void Update()
    {
        if (IsShaking)
        {
            if (shakeTimer > 0)
            {
                Vector3 randomOffset = Random.insideUnitSphere * 0.1f;
                transform.position = originalPosition + randomOffset;
                shakeTimer -= Time.deltaTime;
            }
            else
            {
                transform.position = originalPosition;
                IsShaking = false;
            }
        }
    }

    //Starts the shaking animation.
    public void Shake()
    {
        shakeTimer = 0.3f;
        IsShaking = true;
    }

    // Reduces the building's health, updates health text, and triggers the death effect if health reaches zero.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthText();

        if (currentHealth <= 0)
            DeathEffect();
    }

    //Updates the displayed health value
    private void UpdateHealthText()
    {
        healthText.text = $"Health: {currentHealth}";
    }

    //Shows the building image, a "cross" sprite (presumably signifying destruction), and sets the IsDead flag.
    private void DeathEffect()
    {
        building.SetActive(true);
        cross.SetActive(true);
        IsDead = true;
    }

    //Shows a "missed" text when an attack misses a building on this cell.
    public void HitMissed()
    {
        missed.SetActive(true);
    }

    //Controls the visibility of the building image based on the provided state.
    public void BuildingImage(bool state)
    {
        building.SetActive(state);
    }
}
