using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{

  static SceneControl instance;

  void Awake()
  {
    // Don't destroy this gameObject when loading different scenes
    DontDestroyOnLoad(this.gameObject);

    // Initialize the singleton from this object if it wasn't alredy
    if (instance == null)
    {
      instance = this;
    }
    // destroy this object if the singleton was already loaded
    else
    {
      // Destroy this extra gameObject
      Destroy(gameObject);
    }
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      SceneManager.LoadScene("SampleScene");
      Destroy(gameObject);
    }
  }
}
