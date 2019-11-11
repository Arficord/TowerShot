using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWallController : MonoBehaviour {
    public Image healthBarImg;

    public void Start()
    {
        GameData.onHpChange += redrawHealthbar;
    }

    public void takeDamage(EnemyController attaker){
        GameData.curHp -= attaker.currentStats.power;
        //redrawHealthbar();
        if(GameData.curHp<=0){
            //defeate();
        }
    }
    public void defeate(){
        try{
            Destroy(gameObject);
        }
        catch{
        }
    }

    public void redrawHealthbar(){
        healthBarImg.fillAmount=GameData.curHp/GameData.maxHp;
    }
}
