using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTimer : MonoBehaviour
{
    [HideInInspector]
    public Image timerFill;

    private float timerTime;
    private float currentTimerTime;

    private bool initDone = false;

    private void Update()
    {
        if (initDone)
        if (currentTimerTime > 0)
        {
            currentTimerTime -= Time.deltaTime;
            timerFill.fillAmount -= 1.0f / timerTime * Time.deltaTime;
        }
        else 
            Destroy(this.gameObject);
    }
    
    public void Init(float time)
    {
        timerTime = time;
        currentTimerTime = time;

        initDone = true;
    }
}
