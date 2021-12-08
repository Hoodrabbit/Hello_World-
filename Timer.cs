using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text timetime;
    string Timetext;
    public Enemy_Move_Controller TTime_;
    int TTime_int= 30;
    float TTime_2;

    // Start is called before the first frame update
    void Start()
    {
        timetime = GetComponent<Text>();
        TTime_ = GetComponentInParent<Enemy_Move_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
       
        TTime_2 += Time.deltaTime;

        if(TTime_2 >= 1)
        {
            TTime_int = 30 - (int)TTime_.TTime_Value();
            TTime_2 = 0;
        }
        Timetext = string.Format("00 : {0:D2}", TTime_int);

        timetime.text = Timetext;
    }
}
