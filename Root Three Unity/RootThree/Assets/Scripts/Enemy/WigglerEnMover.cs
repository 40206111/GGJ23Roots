using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WigglerEnMover : EnemyMover
{
    protected override Vector3 TargettingDirection()
    {
        Vector3 direct = base.TargettingDirection();
        return (Quaternion.Euler(0, 45 * ((Time.time % 5f) > 2.5f ? 1f : -1f), 0) * direct).normalized;
    }
}
