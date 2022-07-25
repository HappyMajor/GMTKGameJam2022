using UnityEngine;

public class OneShotAudio
{

  public static void Play(string eventName, Transform transform, Rigidbody2D rigidBody = null)
  {
    // Create sound
    var sound = FMODUnity.RuntimeManager.CreateInstance(eventName);

    // Attach it to the transform
    if (rigidBody)
    {
      FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, transform, rigidBody);
    }
    else
    {
      FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, transform);
    }

    // Play sound
    // Must immediately release the event instance after start() to prevent memory leaks
    sound.start();
    sound.release();
  }
}
