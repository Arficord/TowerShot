using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectController : MonoBehaviour
{
    public enum Effect
    {
        EXPLOSIVE
    }

    public Effect effect;
    public GameObject explosiveEffect;

    private void setRadius(float r)
    {
        transform.localScale = new Vector3(r*2, r*2, r*2);
    }
    private void setRadius()
    {
        switch(effect)
        {
            case Effect.EXPLOSIVE:
                setRadius(Upgrades.deathExplosive_baseRadius + Upgrades.deathExplosive__radiusPerLvl * GameData.upgrades.deathExplosive_lvl);
                break;
        }
    }
    private void play()
    {
        switch(effect)
        {
            case Effect.EXPLOSIVE:
                Instantiate(explosiveEffect, transform);
                break;
        }
    }
    public void play(Effect eff)
    {
        effect = eff;
        setRadius();
        play();
        StartCoroutine(offColider());
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            switch (effect)
            {
                case Effect.EXPLOSIVE:
                    enemy.takeDamage(GameData.shellBaseStats.damage * (Upgrades.deathExplosive_baseDamage + Upgrades.deathExplosive__damagePerLvl * GameData.upgrades.deathExplosive_lvl));
                    break;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

    }

    private IEnumerator offColider()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GetComponent<CapsuleCollider>().enabled = false;
    }
}
