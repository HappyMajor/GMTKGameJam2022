using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonster
{
    public void ApplyDamage(float dmg);
    public void ApplyKnockback(Vector3 knockback);
}
