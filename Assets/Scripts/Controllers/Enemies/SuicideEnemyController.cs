using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemyController : EnemyController
{
    public override Vector3 getCenterPoint()
    {
        if (GetComponent<CapsuleCollider>() == null)
        {
            return Vector3.zero;
        }
        return transform.position + GetComponent<CapsuleCollider>().center * 0.6f;
    }

    public override Vector3 getHeadPoint()
    {
        return getCenterPoint();
    }
    public void attackAnimationEvent()
    {
        attack();
    }
    protected void attack()
    {
        mainWall.takeDamage(this);
        downAnimationEvent();
    }
}
