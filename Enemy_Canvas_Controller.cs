using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Canvas_Controller : MonoBehaviour
{
    Camera Maincam;
    Canvas MyCanvas;
    Text Timer_text;
    Enemy_Move_Controller Enemy_pos;
    //RectTransform Text_transform;

    // Start is called before the first frame update
    void Start()
    {
        MyCanvas = GetComponent<Canvas>();
        Maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Enemy_pos = GetComponentInParent<Enemy_Move_Controller>();
        MyCanvas.worldCamera = Maincam;
        Timer_text = GetComponentInChildren<Text>();
        //Text_transform = GetComponentInChildren<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer_text.transform.position = Maincam.WorldToScreenPoint(Enemy_pos.transform.position);
        //if(Input.GetKeyDown(KeyCode.Q))
        //Debug.Log(Maincam.WorldToScreenPoint(Enemy_pos.transform.position));
    }
}
