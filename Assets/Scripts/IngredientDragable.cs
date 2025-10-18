using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class IngredientDragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("ID must match recipe entries")]
    public IngredientSO ingredient;

    [Header("Optional: visuals")]
    public AudioClip pickUpSfx;
    public AudioClip dropSfx;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private Vector3 originalPosition;
    private CauldronDropZone cauldron;
    private Canvas rootCanvas;
    private AudioSource audioSource;


    Image ingredientImage;

 

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Find the top-most canvas so dragging uses Screen Space canvas delta correctly
        cauldron = FindAnyObjectByType<CauldronDropZone>();
        rootCanvas = GameObject.FindGameObjectWithTag("RootCanvas")?.GetComponent<Canvas>();
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        ingredientImage = GetComponent<Image>();
        if (ingredientImage != null && ingredient != null && ingredient.ingredientSprite != null)
        {
            ingredientImage.sprite = ingredient.ingredientSprite;
        }

    }
    private void Start()
    {
        GameManager.OnPotionDone += GameManager_OnPotionDone; 
    }

    private void OnDisable()
    {
        GameManager.OnPotionDone -= GameManager_OnPotionDone;
    }



    private void GameManager_OnPotionDone()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f; 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        // Allow raycasts to pass through while dragging (so drop zones can receive pointer)
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.9f;

        transform.SetParent(rootCanvas.transform, true); // drag on top

        if (pickUpSfx) audioSource.PlayOneShot(pickUpSfx);
        // Small scale pop
        rectTransform.localScale = Vector3.one * 1.05f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move by pointer delta in canvas space
        if (rootCanvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
        transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;
        //canvasGroup.blocksRaycasts = true;
        //canvasGroup.alpha = 1f;
        rectTransform.localScale = Vector3.one;

        if (dropSfx) audioSource.PlayOneShot(dropSfx);
    }

    /// <summary>
    /// Called by CauldronDropZone when the ingredient is successfully accepted.
    /// Moves this object back to its shelf and resets pos (acts like it’s “consumed”).
    /// </summary>
    public void ConsumeBackToShelf()
    {
        //this happens before end drag, might need to pass a delegate to end drag?
        Debug.Log("Ingredient " + ingredient.ingredientName + " consumed back to shelf.");
        transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.3f; // visually indicate it’s used up
    }


    public void OnPointerEnter(PointerEventData e)
    => Debug.Log($"[DropProbe] Pointer ENTER: {name}");

    public void OnPointerExit(PointerEventData e)
        => Debug.Log($"[DropProbe] Pointer EXIT: {name}");
}
