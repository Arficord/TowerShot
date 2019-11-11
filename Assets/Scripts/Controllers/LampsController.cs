using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampsController : MonoBehaviour {
    enum Side{
        right,
        left
    }
    private Light[,] lamps;
    private List<Light> leftLamps;

    private void Start()
    {
        Transform lightCase = transform.GetChild(0);
        lamps = new Light[2, lightCase.childCount];
        for (int iCase = 0; iCase < 2; iCase++) {
            for (int iLight = 0; iLight < lightCase.childCount; iLight++) {
                lamps[iCase, iLight] = lightCase.GetChild(iLight).GetChild(0).GetComponent<Light>();
            }
            lightCase = transform.GetChild(1);
        }
        
    }
    float time = 0;
    private void Update()
    {
        time += Time.deltaTime;
        if(time>=1){
            StartCoroutine(Flick((Side)Random.Range(0, 2), Random.Range(1, 6)));
            time = 0;
        }
    }
    private IEnumerator Flick(Side side, int index){
        for(int i = 0; i < 3; i++){
            lamps[(int)side, index].enabled = !lamps[(int)side, index].enabled;
            yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        }
        if(Random.Range(0,100)>85){
            lamps[(int)side, index].enabled = true;
        }
    }
    private void setAllLightStage(bool isEnabled){
        for(int i=0;i<2;i++){
            setSideLightStage((Side)i, isEnabled);
        }
    }
    private void setSideLightStage(Side side, bool isEnabled){
        for(int i = 0; i<lamps.Length/2;i++){
            setLightStage(side, i, isEnabled);
        }
    }
    private void setRowLightStage(int row, bool isEnabled){
        setLightStage(Side.right, row, isEnabled);
        setLightStage(Side.left, row, isEnabled);
    }
    private void setLightStage(Side side, int index, bool isEnabled){
        lamps[(int)side, index].enabled = isEnabled;
    }
}
