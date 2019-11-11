using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    public float secoundToDie = 10;
    public IDestroyable target;

    void Start()
    {
        StartCoroutine(killTarget());
    }
    private IEnumerator killTarget()
    {
        yield return new WaitForSeconds(secoundToDie);
        target = GetComponent<IDestroyable>();
        if (target!=null)
        {
            target.kill();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
