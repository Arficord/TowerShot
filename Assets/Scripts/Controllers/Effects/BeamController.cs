using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{

    public void beamToTarget(Vector3 point)
    {
        StartCoroutine(followTarget(point));
    }
    public void beamToTarget(Transform target){
        StartCoroutine(followTarget(target));
    }
    public void beamToTarget(EnemyController target)
    {
        StartCoroutine(followTarget(target));
    }

    public void beamTo(Vector3 point)
    {
        transform.LookAt(point);
        float distance = Vector3.Distance(transform.position, point);
        Transform laser = transform.GetChild(0);
        laser.localScale = new Vector3(1, 1, distance);
    }
    public void beamTo(Transform target)
    {
        beamTo(target.position);
    }

    private IEnumerator followTarget(Vector3 point){
        while (true)
        { 
            beamTo(point);
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator followTarget(Transform target)
    {
        while (target!=null)
        {
            beamTo(target);
            yield return new WaitForEndOfFrame();
        }
        kill();
    }
    private IEnumerator followTarget(EnemyController target)
    {
        while (target!=null && target.action!=ActionState.DEATH)
        {
            beamTo(target.getHeadPoint());
            yield return new WaitForEndOfFrame();
        }
        kill();
    }

    public void kill()
    {
        Destroy(gameObject);
    }
}
