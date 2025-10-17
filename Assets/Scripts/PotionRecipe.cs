using UnityEngine;

[CreateAssetMenu(fileName = "PotionRecipe", menuName = "Scriptable Objects/PotionRecipe")] 
public class PotionScriptableObject : ScriptableObject {
    public string potionName;
    public IngredientSO firstIngredient;
    public IngredientSO secondIngredient;
    public IngredientSO thirdIngredient;
}

