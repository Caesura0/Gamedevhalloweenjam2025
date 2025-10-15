using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]

public class IngredientSO : ScriptableObject
{

    public string ingredientName;
    public Sprite ingredientSprite;
}
