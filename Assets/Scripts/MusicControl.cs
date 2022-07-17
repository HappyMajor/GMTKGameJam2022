using UnityEngine;

public class MusicControl : MonoBehaviour
{
  static MusicControl instance;

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
    // the reason we do this is so that there is only one music
    // instance running even if we change levels. See this video
    // for more info https://www.youtube.com/watch?v=1Y6suVBaBK8
    else
    {
      // Destroy this extra gameObject
      Destroy(gameObject);
    }
  }
}
