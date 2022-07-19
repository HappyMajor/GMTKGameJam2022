using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devilchest : MonoBehaviour
{
    private bool isOpen = false;
    private AudioSource audioSource;
    [SerializeField] public AudioClip sound;
    public RollableField rollableField;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (!isOpen)
            {
                PlaySound();
                isOpen = true;
                GetComponent<Animator>().SetTrigger("Open");
                StartCoroutine(Routines.DoLater(3, () =>
                {
                    rollableField.SpawnSkeletons(UnityEngine.Random.Range(1, 10), true);
                    rollableField.SpawnRandomUpgrades(UnityEngine.Random.Range(1, 3));
                    rollableField.SpawnMoney(UnityEngine.Random.Range(5, 30));
                }));
            }
        }
    }






    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //PlaySound();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(sound);
    }
}
