using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ReceiptTvUI : MonoBehaviour
{

    public GameObject imagePrefab;

    public static ReceiptTvUI instance;

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


    // hook this up to an event
    public void DisplayRecipe(List<IngredientSO> recipe)
    {

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var ingredient in recipe)
        {
            GameObject imgObj = Instantiate(imagePrefab, transform);
            var imgComponent = imgObj.GetComponent<UnityEngine.UI.Image>();
            if (imgComponent != null && ingredient.ingredientSprite != null)
            {
                imgComponent.sprite = ingredient.ingredientSprite;
            }
        }
    }

}
