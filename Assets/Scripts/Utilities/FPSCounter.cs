using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private Text text;
    private int framesUpdates=0;
    private float timer = 0;
    public float UPDATE_TIMER = 1;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        framesUpdates++;
        timer += Time.deltaTime;
        if(timer>=UPDATE_TIMER)
        {
            text.text = (framesUpdates / timer).ToString();
            timer = 0;
            framesUpdates = 0;
        }
    }
}
