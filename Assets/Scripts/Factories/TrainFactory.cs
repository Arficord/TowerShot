using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainFactory : MonoBehaviour
{
    public  float trainCooldown = 10;
    private float trainStopWatch = 0;

    public GameObject train;

    private void Update()
    {
        trainStopWatch += Time.deltaTime;
        if(trainStopWatch>=trainCooldown)
        {
            spawnTrain();
            trainStopWatch = 0;
        }
    }

    private void spawnTrain()
    {
        GameObject headTrain = Instantiate(train);
        headTrain.transform.SetParent(transform);
        headTrain.transform.localPosition = Vector3.zero;
    }
}
