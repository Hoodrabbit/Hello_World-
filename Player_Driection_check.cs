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
           // Debug.Log("�� �ȵ�");
            P_S = Player_State.Pick_UP_Box;
            //Debug.Log("�� �ȵ�_2");
        }
        else if(SomethingIn == false && Input.GetKeyDown(KeyCode.Space) && P_S == Player_State.Normal)
        {
            //�ؽ�Ʈ�� �÷��̾� ��ó�� ��ȣ�ۿ��� ������Ʈ? �� �������� �ʽ��ϴ� ��� ��� ��Ű��
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
        



        //���� �̻��ѵ� 
        if (Input.GetKey(KeyCode.A))
        {
            if(P_S == Player_State.Normal)
            P_R_S = Player_Rotation_State.Left; //�ڽ��� ���� ���°� �ƴҶ��� ������ �� �ֵ��� ������ֱ� 

            if (P_S == Player_State.Pick_UP_Box && P_R_S != Player_Rotation_State.Down && P_R_S != Player_Rotation_State.Up)
            {  
                Box_rigid.velocity = Vector2.left; // �̵��� �� �ݶ��̴��� �ڽ��� �꿩���� �� ������ �ȵǰ� �ٸ� �������� �̵��� �ع���
                //�ӷ��� �� �� ������ ����� ��� �� �� ��            
            }
            else
            {
                B2D.offset = new Vector2(-0.3f, 0.5f);
                B2D.size = new Vector2(0.2f, 0.3f);
            }
        }

        if (Input.GetKey(KeyCode.S)) // ���� �κ� �̻��� �����ؾߵ�
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
