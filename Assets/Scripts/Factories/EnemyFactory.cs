using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyFactory : MonoBehaviour {
    public enum EnemyType{
        sample,
        slimeHands,
        littleSlimeHands,
    }
    public  MainWallController mainWall;
    public GameObject[] enemies;
    public GameObject[] spawnDots;

    public int lastWave = 5;
    public int curWave = 0;

    private void Start()
    {
        EnemyController.onEnemiesEnded += onEnemiesEnded;
        EnemyController.mainWall = mainWall;
    }

    private void onEnemiesEnded(){
        if(curWave<lastWave)
        {
            startWave();
        }
        else
        {
            curWave = 0;
            EventController.eventController.onEndedStage();
        }
    }

    public void startSpawnWaves()
    {
        startWave();
    }
    private void startWave(){
        curWave++;
        EventController.eventController.updateWaveText(curWave, lastWave);
        spawnEnemies(getEnemySheat());
    }
    private GameObject[] getEnemySheat(){
        GameObject[] spawnSheet = new GameObject[10];
        for(int i=0;i<spawnSheet.Length/2;i++){
            if(i==0||i==4)
            {
                //spawnSheet[i] = enemies[2];
                continue;
            }
            spawnSheet[i] = enemies[0];
        }
        spawnSheet[7] = enemies[1];
        return spawnSheet;
    }
    private void spawnEnemies(GameObject[] enemySpawnSheet) {
        for (int i = 0; i < enemySpawnSheet.Length; i++){
            if(enemySpawnSheet[i]!=null)
            {
                GameObject temp = Instantiate(enemySpawnSheet[i]);
                temp.transform.SetParent(spawnDots[i].transform);
                temp.transform.localPosition = Vector3.zero;
            }
        }
    }
}