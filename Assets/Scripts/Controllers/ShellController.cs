using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour {
    public GameObject hitParticle;
    [HideInInspector]
    public ShellStats stats;
    private Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        move();
    }

    public void move(){
        rigidbody.velocity = transform.forward*stats.speed;
    }
    public void kill(){
        Destroy(gameObject);
    }
 
    private void OnCollisionEnter(Collision collision)
    {
        spawnCollisionEffect(collision.transform);

        if (stats.bounces>0){
            Debug.Log("Bounced " + stats.bounces);
            stats.bounces--;
            Vector3 reflectBeam = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            transform.localRotation = Quaternion.FromToRotation(Vector3.forward, reflectBeam);
            rigidbody.velocity = reflectBeam * stats.speed;
        }
        else{
            Debug.Log("Dont Bounced " + stats.bounces);
            kill();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.transform.GetComponent<EnemyController>();
        if (enemy!=null){


            spawnCollisionEffect(other.transform);

            enemy.shellHit(this);
            if(stats.penetrations>0){
                Debug.Log("Penetrated " + stats.penetrations);
                stats.penetrations--;
                stats.damage *= (float)Math.Round( Mathf.Pow(Upgrades.penetration_damageToLast, 1.0f / GameData.upgrades.penetration_lvl), 1);
            }
            else{
                Debug.Log("Dont Penetrated " + stats.penetrations);
                kill();
            }
        }
    }

    private void spawnCollisionEffect(Transform parent){
        Transform hitEffect = Instantiate(hitParticle, transform.localPosition, transform.rotation).transform;
        hitEffect.SetParent(parent);
    }
}
