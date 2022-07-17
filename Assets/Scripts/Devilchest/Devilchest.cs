using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devilchest : MonoBehaviour
{
    private bool isOpen = false;
    public RollableField rollableField;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("DEVILCHEST COLLISION!");
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("DEVILCHEST COLLISION COLLISION! PLAYER");
            if (!isOpen)
            {
                isOpen = true;
                GetComponent<Animator>().SetTrigger("Open");
                PlaySound();
                StartCoroutine(Routines.DoLater(3, () =>
                {
                    rollableField.SpawnSkeletons(UnityEngine.Random.Range(1, 10), true);
                    rollableField.SpawnRandomUpgrades(UnityEngine.Random.Range(1, 3));
                    rollableField.SpawnMoney(UnityEngine.Random.Range(5, 30));
                }));
            }
        }
    }


    private AudioSource audioSource;
    [SerializeField] public AudioClip sound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(sound);
    }
}
