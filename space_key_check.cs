using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class space_key_check : MonoBehaviour
{
    Text space_check;
    public Player_Driection_check P_D_C;

    // Start is called before the first frame update
    void Start()
    {
        space_check = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && P_D_C.SomethingIn == true)
        {
            if(space_check.enabled == false)
            {
                space_check.enabled = true;
            }
            space_check.text = "Grab_Box";
        }

        if(P_D_C.SomethingIn == false)
        {
            space_check.enabled = false;
        }
        

    }
}
