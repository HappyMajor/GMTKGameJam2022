using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class TextFlash : MonoBehaviour
{
  [Tooltip("Color to switch to during the flash.")]
  [SerializeField] Color flashColor;

  [Tooltip("Duration of the flash.")]
  [SerializeField] float duration;

  // The text that should flash.
  TMPro.TMP_Text text;

  // The color that was in use, when the script started.
  Color originalColor;

  // The currently running coroutine.
  Coroutine flashRoutine;

  void Start()
  {
    // Get the SpriteRenderer to be used,
    // alternatively you could set it from the inspector.
    text = GetComponent<TMPro.TMP_Text>();

    // Get the color that the text uses, 
    // so we can switch back to it after the flash ended.
    originalColor = text.color;
  }

  public void Flash()
  {
    // If the flashRoutine is not null, then it is currently running.
    if (flashRoutine != null)
    {
      // In this case, we should stop it first.
      // Multiple FlashRoutines the same time would cause bugs.
      StopCoroutine(flashRoutine);
    }

    // Start the Coroutine, and store the reference for it.
    flashRoutine = StartCoroutine(FlashRoutine());
  }

  IEnumerator FlashRoutine()
  {
    // Swap to the flash color.
    text.color = flashColor;

    // Pause the execution of this function for "duration" seconds.
    yield return new WaitForSeconds(duration);

    // After the pause, swap back to the original color.
    text.color = originalColor;

    // Set the routine to null, signaling that it's finished.
    flashRoutine = null;
  }
}