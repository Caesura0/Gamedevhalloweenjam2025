using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionRecipe", menuName = "Scriptable Objects/PotionRecipe")] 
public class PotionRecipe : ScriptableObject {
    public string potionName;

    public List<IngredientSO> ingredientList = new List<IngredientSO>();

    //public IngredientSO firstIngredient;
    //public IngredientSO secondIngredient;
    //public IngredientSO thirdIngredient;
}

