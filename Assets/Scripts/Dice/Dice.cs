using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dice : MonoBehaviour
{
    public float rollSpeed;
    private bool isRolling = false;
    private Vector3 targetPosition;
    private Action<int> callback;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject skillFxPrefab;

    public Sprite[] eyesOneToSix;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Update()
    {
        if(isRolling)
        {
            if(Vector3.Distance(transform.position, targetPosition) <= 0.2f)
            {
                //We reached the destination close enough
                this.animator.SetBool("Roll", false);
                transform.position = targetPosition;
                isRolling = false;
                RollResult();

            } else
            {
                this.animator.SetBool("Roll", true);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f * rollSpeed * Time.deltaTime);
            }
        }
    }

    public void RollResult()
    {
        int result = UnityEngine.Random.Range(1, 7);
        animator.speed = 0f;
        animator.enabled = false;

        // Create FX behind the die
        GameObject skillFx = Instantiate(skillFxPrefab, transform);
        skillFx.transform.position = transform.position;

        // Play die FX sound
        OneShotAudio.Play("event:/sfx/die cast", skillFx.transform);

        spriteRenderer.sprite = eyesOneToSix[result - 1];
        //bring it to front
        transform.position = new Vector3(transform.position.x, transform.position.y, -6);

        StartCoroutine(Routines.DoLater(2, () =>
        {
            callback(result);
            Destroy(skillFx); 
        }));
    }

    public void ThrowAtTarget(Vector3 targetPosition, Action<int> callback)
    {
        // Mark as rolling the die
        isRolling = true;

        // Play sound
        OneShotAudio.Play("event:/sfx/die roll", transform);

        this.targetPosition = targetPosition;
        this.targetPosition.z = transform.position.z;
        this.callback = callback;
    }
}
