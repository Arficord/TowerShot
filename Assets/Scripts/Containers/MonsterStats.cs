using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MonsterStats
{
    public float health;
    public float power;
    public float speed;

    public MonsterStats(MonsterStats parent) :this(parent.health, parent.power,  parent.speed){
    }
    public MonsterStats(float health, float power, float speed){
        this.health = health;
        this.power = power;
        this.speed = speed;
    }
}
