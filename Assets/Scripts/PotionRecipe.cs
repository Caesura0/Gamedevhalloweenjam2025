using UnityEngine;

[CreateAssetMenu(fileName = "PotionRecipe", menuName = "Scriptable Objects/PotionRecipe")] 
public class PotionScriptableObject : ScriptableObject {
    public string potionName;
    public string firstIngredient;
    public string secondIngredient;
    public string thirdIngredient;
}

