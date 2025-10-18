using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] List<PotionRecipe> potionRecipes = new();   // assign in Inspector
    [SerializeField] bool avoidImmediateRepeat = true;

    [Header("Events")]
    public UnityEvent<PotionRecipe> OnNewPotion;   // e.g., update CRT with recipe hint
    public UnityEvent<PotionRecipe> OnBrewSuccess; // e.g., success SFX/VFX
    public UnityEvent<List<IngredientSO>> OnBrewFail; // e.g., error SFX/VFX, show what was added

    public float timerDuration = 60f; // Duration of the game timer in seconds

    public static event System.Action OnPotionDone;

    PotionRecipe currentPotion;
    PotionRecipe lastPotion;

    // The player's current thrown-in ingredients
    List<IngredientSO> currentIngredientList = new();


    ReceiptTvUI receiptTvUI;

    int successfulPotions;


    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("GameManager started.");
        // Optionally choose a starting recipe
        SetRandomPotion();
        receiptTvUI = FindAnyObjectByType<ReceiptTvUI>();

        Invoke(nameof(StartGame), 2f);
    }

    // Called by your drag/drop or button system when an ingredient is added
    public void AddIngredient(IngredientSO ingredient)
    {
        if (ingredient == null)
        {
            Debug.LogWarning("Attempted to add a null ingredient.");
            return;
        }

        if(currentIngredientList.Contains(ingredient))
        {
            Debug.LogWarning($"Ingredient {ingredient.ingredientName} is already in the cauldron.");
            currentIngredientList.Remove(ingredient);
            ReceiptTvUI.instance.DisplayRecipe(currentIngredientList); // Update display
        }


        if (currentIngredientList.Count <= 0)
        {
            CheckPotion();                // evaluate success/fail
            currentIngredientList.Clear(); // reset cauldron
            OnPotionDone?.Invoke();        // notify listeners
            SetRandomPotion();            // pick the next order
        }
    }

    public void SetRandomPotion()
    {
        if (potionRecipes == null || potionRecipes.Count == 0)
        {
            Debug.LogWarning("No potion recipes available.");
            currentPotion = null;
            return;
        }

        int attempts = 0;
        PotionRecipe pick;
        do
        {
            int randomIndex = Random.Range(0, potionRecipes.Count);
            pick = potionRecipes[randomIndex];
            attempts++;
            // small guard to avoid infinite loop if only 1 recipe
            if (attempts > 10) break;
        }
        while (avoidImmediateRepeat && potionRecipes.Count > 1 && pick == lastPotion);

        currentPotion = pick;
        lastPotion = pick;

        Debug.Log($"New Potion: {currentPotion.potionName}");
        OnNewPotion?.Invoke(currentPotion);
        //reset timer
        //reset game visual
        currentIngredientList = new List<IngredientSO>(currentPotion.ingredientList);
        ReceiptTvUI.instance.DisplayRecipe(currentIngredientList);
    
}

    void CheckPotion()
    {

    }


    public void StartGame()
    {
        
        TimerUI.instance.BeginCountdown(timerDuration);
    }


    // Optional helpers if you want manual control
    public void ClearCauldron() => currentIngredientList.Clear();
    public PotionRecipe GetCurrentPotion() => currentPotion;
    public IReadOnlyList<IngredientSO> GetCurrentIngredients() => currentIngredientList;
}
