using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devilchest : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public GameObject goldPrefab;
    public GameObject[] potions;

    private bool isOpen = false;
    private AudioSource audioSource;
    [SerializeField] public AudioClip sound;

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
                    SpawnSkeletons();
                    SpawnGold();
                    SpawnPotions();
                }));
            }
        }
    }

    private void SpawnSkeletons()
    {
        for (int i = 0; i < UnityEngine.Random.Range(1, 10); i++)
        {
            Vector3 randomPosition = Util.GetRandomPositionOfRectangle(transform.parent.position, 10, 10);
            Instantiate(skeletonPrefab, randomPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }


    private void SpawnGold()
    {
        for (int i = 0; i < UnityEngine.Random.Range(1, 15); i++)
        {
            Vector3 randomPosition = Util.GetRandomPositionOfRectangle(transform.parent.position, 10, 10);
            Instantiate(goldPrefab, randomPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }

    private void SpawnPotions()
    {
        for (int i = 0; i < UnityEngine.Random.Range(1, 4); i++)
        {
            Vector3 randomPosition = Util.GetRandomPositionOfRectangle(transform.parent.position, 10, 10);
            Instantiate(potions[UnityEngine.Random.Range(0,potions.Length)], randomPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
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
