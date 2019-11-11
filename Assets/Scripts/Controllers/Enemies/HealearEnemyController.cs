using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealearEnemyController : EnemyController
{
    private EnemyController abilityTarget;
    public GameObject beamPrefab;
    //public GameObject healingEffect;
    private BeamController beam;

    protected override void specificInitialize()
    {
        onEnemyDeath += killBeam;
    }

    protected override void actAI(){
        if (transform.position.z < 10)
        {
            if (action == ActionState.WALK)
            {
                Debug.Log("STOP");
                stopMoving();
            }
            if (action != ActionState.USING_ABILITY && EnemyController.liveEnemies.Count > 1)
            {
                Debug.Log("ABILITY");
                startUseAbility();
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

    public void startUseAbility(){
        action = ActionState.USING_ABILITY;
        animator.SetInteger("CurrentAction", (int)ActionState.USING_ABILITY);
    }
    public void ablilityAnimationEvent(){
        if (abilityTarget == null)
        {
            abilityTarget = EnemyController.findClosestEnemy(this); 
            if(abilityTarget==null)
            {
                return;
            }
            abilityTarget.onEnemyDeath += onTargetDead;
        }
        Debug.Log(abilityTarget.name+" "+ name);
        beam = Instantiate(beamPrefab).GetComponent<BeamController>();
        beam.beamToTarget(abilityTarget);
        beam.transform.SetParent(transform);
        beam.transform.localPosition = new Vector3(0, 10.3f, -1.9f);

        StartCoroutine(proceedAbility());
    }
    private IEnumerator proceedAbility()
    {
        while(abilityTarget!=null && action!=ActionState.DEATH)
        {
            abilityTarget.addHealth(currentStats.power);
            if(abilityTarget.currentStats.speed < abilityTarget.baseStats.speed)
            {
                abilityTarget.setSpeed(abilityTarget.baseStats.speed);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void killBeam()
    {
        if (beam!=null)
        {
            beam.kill();
        }
    }
    public void onTargetDead(){
        if(this==null)
        {
            return;
        }
        Debug.Log("Target is DEAD!");
        abilityTarget = null;
        action = ActionState.DEFAULT;
        animator.SetInteger("CurrentAction", (int)ActionState.DEFAULT);
    }
    public override Vector3 getCenterPoint()
    {
        if (GetComponent<CapsuleCollider>() == null)
        {
            return Vector3.zero;
        }
        return transform.position + GetComponent<CapsuleCollider>().center*0.6f;
    }
    public override Vector3 getHeadPoint()
    {
        return getCenterPoint();
    }
}
