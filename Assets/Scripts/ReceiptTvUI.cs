using UnityEngine;

public class ReceiptTvUI : MonoBehaviour
{

    public GameObject imagePrefab;



    // hook this up to an event
    public void DisplayRecipe(PotionRecipe recipe)
    {

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var ingredient in recipe.ingredientList)
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
