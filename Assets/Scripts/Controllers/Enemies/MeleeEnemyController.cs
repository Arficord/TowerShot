using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    public float attackRange;
    protected override void actAI()
    {
        if (transform.position.z < attackRange && mainWall != null)
        {
            if (action == ActionState.WALK)
            {
                stopMoving();
            }
            if (action != ActionState.ATTACK)
            {
                startAttacking();
            }
        }
        else
        {
            if (action != ActionState.WALK)
            {
                startMoving();
            }
        }
    }
    protected void startAttacking()
    {
        action = ActionState.ATTACK;
        animator.SetInteger("CurrentAction", (int)ActionState.ATTACK);
    }
    protected void attack()
    {
        mainWall.takeDamage(this);
    }
    public void attackAnimationEvent()
    {
        attack();
    }

    public override Vector3 getCenterPoint()
    {
        if (GetComponent<BoxCollider>() == null)
        {
            return Vector3.zero;
        }
        return new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider>().size.y / 2, transform.position.z);
    }
    public override Vector3 getHeadPoint()
    {
        if (GetComponent<BoxCollider>() == null)
        {
            return Vector3.zero;
        }
        return new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider>().size.y * 0.85f, transform.position.z-0.1f);
    }
}
