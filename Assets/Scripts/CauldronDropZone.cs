using UnityEngine;
using UnityEngine.EventSystems;

public class CauldronDropZone : MonoBehaviour, IDropHandler
{
    [Header("Optional: splash fx")]
    public ParticleSystem splashFx;
    public AudioClip splashSfx;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        Debug.Log("Dropped on cauldron: " + eventData.pointerDrag.name);

        var ingredient = eventData.pointerDrag.GetComponent<IngredientDragable>();
        if (ingredient == null)
        {
            Debug.Log("null ingredient");
            return;
        }


        // Add ingredient to the cauldron (game logic)


        // Visual feedback
        if (splashFx) splashFx.Play();
        if (splashSfx) audioSource.PlayOneShot(splashSfx);

       // Put the ingredient back to its shelf (treated as "consumed")
       ingredient.ConsumeBackToShelf();
    }
}
