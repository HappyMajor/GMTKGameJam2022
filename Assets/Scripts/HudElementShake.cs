using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class HudElementShake : MonoBehaviour
{
  bool shake = false;
  [SerializeField] float amount = 2.0f;
  [SerializeField] float duration = 0.1f;
  RectTransform rectTransform;
  Vector2 originalPos;

  void Start()
  {
    rectTransform = GetComponent<RectTransform>();
    originalPos = rectTransform.localPosition;
  }

  void Update()
  {
    if (!shake)
    {
      // Reset to original position
      rectTransform.localPosition = originalPos;
      return;
    }

    var pos = rectTransform.localPosition;

    // Stupid math that is more complicated than it needs to be.
    pos.x = amount * Mathf.Sin(Time.realtimeSinceStartup * 1.3f + Time.deltaTime * 1.5f + pos.x) + originalPos.x + Random.Range(-1, 1) * amount;
    pos.y = amount * Mathf.Sin(Time.realtimeSinceStartup * 1.5f + Time.deltaTime * 1.2f + pos.y) + originalPos.y + Random.Range(-1, 1) * amount;

    rectTransform.localPosition = pos;
  }

  public void Shake()
  {
    StartCoroutine(ShakeRoutine());
  }

  IEnumerator ShakeRoutine()
  {
    shake = true;
    yield return new WaitForSeconds(duration);
    shake = false;

    yield break;
  }
}
