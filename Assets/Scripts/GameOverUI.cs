using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;

    public static GameOverUI instance;

    /// Called when the script instance is being loaded.
    void Awake()
    {
        gameOverText = GetComponent<TextMeshProUGUI>();
        // Set the initial display text
        gameOverText.text = "";
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Called to display the Game Over screen with potions crafted count
    public void ShowGameOver(float potionsCrafted)
    {
        // displays GameOver including the potion crafted count value on a newline
        gameOverText.text = $"GAME OVER\nPotions Crafted: {potionsCrafted}";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
