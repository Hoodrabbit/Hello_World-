using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Driection_check : MonoBehaviour
{
    public enum Player_State
    {
        Normal,
        Pick_UP_Box
    }

    public enum Player_Rotation_State
    {
        None,
        Up,
        Down,
        Right,
        Left
    }

    public Player_State P_S = Player_State.Normal;
    public Player_Rotation_State P_R_S = Player_Rotation_State.None;

    BoxCollider2D B2D;

    public Rigidbody2D Box_rigid;
    GameObject SomethingInGameObject;
   
    public bool SomethingIn = false;
    
    public Transform Parent_transform;

    // Start is called before the first frame update
    void Start()
    {
        B2D = GetComponent<BoxCollider2D>();
        //Parent_transform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SomethingIn == true && Input.GetKeyDown(KeyCode.Space))
        {
           // Debug.Log("왜 안됨");
            P_S = Player_State.Pick_UP_Box;
            //Debug.Log("왜 안됨_2");
        }
        else if(SomethingIn == false && Input.GetKeyDown(KeyCode.Space) && P_S == Player_State.Normal)
        {
            //텍스트에 플레이어 근처에 상호작용할 오브젝트? 가 존재하지 않습니다 라고 출력 시키기
        }
        if(Box_rigid != null)
        {
            switch (P_R_S)
            {
                case Player_Rotation_State.None:
                    {
                        //Box_rigid.constraints = RigidbodyConstraints2D.None;
                        //Box_rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        break;
                    }

                case Player_Rotation_State.Up:
                    {
                        Box_rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
                        Box_rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        break;
                    }
                case Player_Rotation_State.Down:
                    {
                        Box_rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
                        Box_rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        break;
                    }
                case Player_Rotation_State.Right:
                    {
                        Box_rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
                        Box_rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        break;
                    }
                case Player_Rotation_State.Left:
                    {
                        Box_rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
                        Box_rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        break;
                    }
            }
        }
        



        //뭔가 이상한데 
        if (Input.GetKey(KeyCode.A))
        {
            if(P_S == Player_State.Normal)
            P_R_S = Player_Rotation_State.Left; //박스를 집은 상태가 아닐때만 변경할 수 있도록 만들어주기 

            if (P_S == Player_State.Pick_UP_Box && P_R_S != Player_Rotation_State.Down && P_R_S != Player_Rotation_State.Up)
            {  
                Box_rigid.velocity = Vector2.left; // 이동할 때 콜라이더가 박스에 닿여있을 때 고정이 안되고 다른 방향으로 이동을 해버림
                //속력을 좀 더 빠르게 만들어 줘야 될 듯 함            
            }
            else
            {
                B2D.offset = new Vector2(-0.3f, 0.5f);
                B2D.size = new Vector2(0.2f, 0.3f);
            }
        }

        if (Input.GetKey(KeyCode.S)) // 여기 부분 이상함 수정해야됨
        {
            if (P_S == Player_State.Normal)
                P_R_S = Player_Rotation_State.Down;
            if (P_S == Player_State.Pick_UP_Box && P_R_S != Player_Rotation_State.Right && P_R_S != Player_Rotation_State.Left)
            {
                Box_rigid.velocity = Vector2.down;
            }
            else
            {
                B2D.offset = new Vector2(0f, 0f);
                B2D.size = new Vector2(0.4f, 0.55f);
            }
            
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (P_S == Player_State.Normal)
                P_R_S = Player_Rotation_State.Right;
            if (P_S == Player_State.Pick_UP_Box && P_R_S != Player_Rotation_State.Down && P_R_S != Player_Rotation_State.Up)
            {
                Box_rigid.velocity = Vector2.right;
            }
            else
            {
                B2D.offset = new Vector2(0.3f, 0.5f);
                B2D.size = new Vector2(0.2f, 0.3f);
            }   
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (P_S == Player_State.Normal)
                P_R_S = Player_Rotation_State.Up;
            if (P_S == Player_State.Pick_UP_Box && P_R_S != Player_Rotation_State.Right && P_R_S != Player_Rotation_State.Left)
            {
                Box_rigid.velocity = Vector2.up;
            }
            else
            {
                B2D.offset = new Vector2(0f, 0.4f);
                B2D.size = new Vector2(0.5f, 0.3f);
            }
                
        }
        
    }

    



    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Box")
        {
            SomethingIn = true;
            SomethingInGameObject = collision.gameObject;
            Box_rigid = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == SomethingInGameObject)
        {
            SomethingIn = false;
            P_S = Player_State.Normal;
        }
    }


    

}
