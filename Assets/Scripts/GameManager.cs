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

    PotionRecipe currentPotion;
    PotionRecipe lastPotion;

    // The player's current thrown-in ingredients
    readonly List<IngredientSO> currentIngredientList = new();

    void Start()
    {
        // Optionally choose a starting recipe
        SetRandomPotion();
    }

    // Called by your drag/drop or button system when an ingredient is added
    public void AddIngredient(IngredientSO ingredient)
    {
        if (ingredient == null)
        {
            Debug.LogWarning("Attempted to add a null ingredient.");
            return;
        }

        currentIngredientList.Add(ingredient);
        Debug.Log($"Added Ingredient: {ingredient.ingredientName}");

        if (currentPotion == null)
        {
            Debug.LogWarning("No active recipe. Selecting one now.");
            SetRandomPotion();
        }

        // If we've reached the exact number needed, evaluate
        int needed = currentPotion != null ? currentPotion.ingredientList.Count : 3;

        if (currentIngredientList.Count >= needed)
        {
            CheckPotion();                // evaluate success/fail
            currentIngredientList.Clear(); // reset cauldron
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
    }

    void CheckPotion()
    {
        if (currentPotion == null)
        {
            Debug.LogWarning("Tried to check a potion without an active recipe.");
            return;
        }

        bool match = MultisetMatches(currentIngredientList, currentPotion.ingredientList);
        if (match)
        {
            Debug.Log($"SUCCESS: Brewed {currentPotion.potionName}!");
            OnBrewSuccess?.Invoke(currentPotion);
        }
        else
        {
            Debug.Log("FAIL: Ingredients did not match the recipe.");
            OnBrewFail?.Invoke(new List<IngredientSO>(currentIngredientList));
        }
    }

    // Order-independent comparison that supports duplicates
    static bool MultisetMatches(List<IngredientSO> a, List<IngredientSO> b)
    {
        if (a == null || b == null) return false;
        if (a.Count != b.Count) return false;

        // Count occurrences using IDs (stable across domain reloads)
        Dictionary<string, int> counts = new();
        foreach (var ing in a)
        {
            if (ing == null) return false;
            string key = string.IsNullOrEmpty(ing.ingredientName) ? ing.name : ing.ingredientName;
            counts.TryGetValue(key, out int c);
            counts[key] = c + 1;
        }

        foreach (var ing in b)
        {
            if (ing == null) return false;
            string key = string.IsNullOrEmpty(ing.ingredientName) ? ing.name : ing.ingredientName;
            if (!counts.TryGetValue(key, out int c)) return false;
            c--;
            if (c == 0) counts.Remove(key);
            else counts[key] = c;
        }

        return counts.Count == 0;
    }

    // Optional helpers if you want manual control
    public void ClearCauldron() => currentIngredientList.Clear();
    public PotionRecipe GetCurrentPotion() => currentPotion;
    public IReadOnlyList<IngredientSO> GetCurrentIngredients() => currentIngredientList;
}
