using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chase_img : MonoBehaviour
{
    public Image imagE;
    float alpha_value;
    

    // Start is called before the first frame update
    void Start()
    {
        imagE = GetComponent<Image>();
        alpha_value = imagE.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어가 추적하는 상태라면
        imagE.color = Color.Lerp(imagE.color, Color.clear, Time.deltaTime * 0.3f);
        

    }
}
