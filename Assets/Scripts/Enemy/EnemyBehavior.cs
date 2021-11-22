using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyBehavior
{
    public void TakeDamage(float totalDamage, PlayerBehavior playerBehavior);

    public bool IsAlive();
}