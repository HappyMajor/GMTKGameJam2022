using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathMagicAttack : MonsterBehaviourState
{
    private BringerOfDeath bringerOfDeath;
    private GameObject player;
    private Animator animator;
    public BringerOfDeathMagicAttack(BringerOfDeath bringerOfDeath)
    {
        this.bringerOfDeath = bringerOfDeath;
    }
    void MonsterBehaviourState.Start()
    {
        player = GameObject.Find("Player");
        animator = bringerOfDeath.gameObject.GetComponent<Animator>();

        bringerOfDeath.StartCoroutine(DoBehaviour());
    }

    IEnumerator DoBehaviour()
    {
        for(int i = 0; i < Random.Range(1,6); i++)
        {
            //Start casting animation
            animator.SetTrigger("Cast");

            // Play magic attack sound
            OneShotAudio.Play("event:/sfx/bringer of death/magic", bringerOfDeath.transform);

            //Wait for the animation to be in the middle
            yield return new WaitForSeconds(0.6f);

            //Now cast thunder in the middle of the animation

            SpawnThunderAt(GetRandomPositionCloseToPlayer());

            //wait again before switching state
            yield return new WaitForSeconds(1f);

        }
        //Switch to chase attack again
        bringerOfDeath.SetBehaviour(BringerOfDeath.Behaviour.TELEPORT_AWAY);
    }

    public void ApplyDamage(float dmg)
    {
        throw new System.NotImplementedException();
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        throw new System.NotImplementedException();
    }

    public void OnEnd()
    {

    }

    private void SpawnThunderAt(Vector3 position)
    {
        GameObject thunder = GameObject.Instantiate(bringerOfDeath.thunderPrefab, position + new Vector3(0,0,-0.2f), Quaternion.Euler(new Vector3(0,0,0)));
    }


    private Vector3 GetRandomPositionCloseToPlayer()
    {
        Vector3 randomPositionCloseToPlayer = Util.GetRandomPositionOfRectangle(player.transform.position + new Vector3(0,1,0), 2, 2);
        return randomPositionCloseToPlayer;
    }

    void MonsterBehaviourState.Update()
    {
    }
}
