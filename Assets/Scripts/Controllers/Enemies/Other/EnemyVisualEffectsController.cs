using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyVisualEffectsController : MonoBehaviour
{
    private enum visualEffects
    {
        Heal=0,
        Burn,
    }

    public void doHealEffect()
    {
        transform.GetChild((int)visualEffects.Heal).GetComponent<ParticleSystem>().Play();
    }
    public void doBurnEffect()
    {
        transform.GetChild((int)visualEffects.Burn).GetComponent<ParticleSystem>().Play();
    }

}
