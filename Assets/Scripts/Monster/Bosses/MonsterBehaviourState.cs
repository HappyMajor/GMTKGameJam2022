using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MonsterBehaviourState
{
    public void Start();
    public void Update();

    public void OnEnd();

    public void ApplyDamage(float dmg);

    public void ApplyKnockback(Vector3 knockback);
}
