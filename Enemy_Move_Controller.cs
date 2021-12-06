using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy_Move_Controller : MonoBehaviour
{
    public enum Move_State
    {
        None,
        Chasing,
        Wall_Checking
    }

    Move_State M_S;

    public GameObject Player;
    public Vector2 TopRight, bottomLeft;
    int XSize, YSize;
    dotted_Line Enemy_state;
    //public Collider2D[] test;
    //public Collider2D[] col;
    Rigidbody2D rigid2D;
    RaycastHit2D rayHit2D;
    int layermaskNum;

    float TTime = 0f;

    public bool Is_Wall_right = false;
    public bool Is_Wall_up = false;
    public bool Is_Wall_down = false;
    public bool Is_Wall_left = false;

    // Start is called before the first frame update
    void Start()
    {
        M_S = Move_State.None;
        Enemy_state = GetComponentInChildren<dotted_Line>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(M_S == Move_State.Chasing)
        {
            Debug.Log("플레이어 쫓고 있는 중");
        }

        if(M_S == Move_State.Wall_Checking)
        {
            Debug.Log("근처에 벽이 있음");
        }

        if (Enemy_state.E_S == dotted_Line.Enemy_State.Enemy_Player_Chasing)
        {
            Control_Wall_check_bool();

            if(Is_Wall_check())
            {
                //Debug.Log("true");
                Decide_Route();
                M_S = Move_State.Wall_Checking;
            }
            else
            {
                //Debug.Log("false");
                Chasing_player();
                M_S = Move_State.Chasing;
            }
            
        }

    }

    void Control_Wall_check_bool()
    {
        if (Is_Wall_left == true && Is_Wall_right == true && Is_Wall_up == true && Is_Wall_left == true)
        {
            Is_Wall_right = false; Is_Wall_up = false; Is_Wall_down = false; Is_Wall_left = false;
        }


        Wall_check_Right();


        Wall_check_Down();


        Wall_check_Up();


        Wall_check_Left();

    }

    Vector2 NowPos;

    bool Is_Wall_check()
    {
        
        if (Is_Wall_left == false && Is_Wall_right == false && Is_Wall_up == false && Is_Wall_down == false)
        {
            return false;
        }

        return true;

    }

    bool Is_Enemy_Moving()//여기에서 플레이어 멈추도록 만들어주는데 이걸 다르게 변경 시켜줘야 함
    {
        TTime += Time.deltaTime;

        if (TTime < 1f)
        {
            NowPos = transform.position;
        }

        if (TTime > 3)
        {
            if (Vector2.Distance(NowPos, transform.position) < 2)
            {
                //if (Is_Wall_right == false)
                //{
                //    //Wall_check_Right();
                //}
                //if (Is_Wall_down == false)
                //{
                //    //Wall_check_Down();
                //}
                //if (Is_Wall_up == false)
                //{
                //   // Wall_check_Up();
                //}
                //if (Is_Wall_left == false)
                //{
                //    //Wall_check_Left();
                //}
                return false;
                
            }
        }

        return true;
    }

    int Distance_Check(bool right, bool left, bool up, bool down)
    {
        int check = 1;
        float disright = Vector2.Distance(Player.transform.position, new Vector2(transform.position.x + 1, transform.position.y));//플레이어의 위치로 체크를 해야함
        float disleft = Vector2.Distance(Player.transform.position, new Vector2(transform.position.x - 1, transform.position.y));
        float disup = Vector2.Distance(Player.transform.position, new Vector2(transform.position.x, transform.position.y + 1));
        float disdown = Vector2.Distance(Player.transform.position, new Vector2(transform.position.x, transform.position.y - 1));
        float best_route = disright;
        if (right == true)
        {
            best_route = 1000f;
        }
        
        if (best_route > disleft && left == false)
        {
            best_route = disleft;
            check = 2;
        }

        if (best_route > disup && up == false)
        {
            best_route = disup;
            check = 3;
        }

        if (best_route > disdown && down == false)

        {
            best_route = disdown;
            check = 4;
        }


        return check;

    }

    void Decide_Route()
    {
        int number = Distance_Check(Is_Wall_right, Is_Wall_left, Is_Wall_up, Is_Wall_down);
        Debug.Log(number);
        switch (number)
        {
            case 1:
                {
                    rigid2D.velocity = Vector2.right * 2f;
                    //오른쪽 이동
                    break;
                }
            case 2:
                {
                    rigid2D.velocity = Vector2.left * 2f;
                    //왼쪽 이동
                    break;
                }
            case 3:
                {
                    rigid2D.velocity = Vector2.up * 2f;
                    //위쪽 이동
                    break;
                }

            case 4:
                {
                    rigid2D.velocity = Vector2.down * 2f;
                    //아래쪽 이동
                    break;
                }


        }


    }


    void Chasing_player()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, Time.deltaTime *2f);
    }

    void Wall_check_Right()//타일맵 콜라이더에 막혀서 이동하지 못하는 경우에 발동시켜줄 함수 
    {
        layermaskNum = 1 << LayerMask.NameToLayer("Wall");
        Debug.DrawRay(transform.position, Vector2.right * 0.6f, Color.blue);
        rayHit2D = Physics2D.Raycast(transform.position, Vector2.right * 0.6f, 1f, layermaskNum);

        if (rayHit2D.transform != null)
        {
            Is_Wall_right = true;
            // Debug.Log(rayHit2D.transform.position);
        }
        if (rayHit2D.transform == null)
        {
            Is_Wall_right = false;
            // Debug.Log(rayHit2D.transform.position);
        }

    }

    void Wall_check_Left()//타일맵 콜라이더에 막혀서 이동하지 못하는 경우에 발동시켜줄 함수 
    {
        layermaskNum = 1 << LayerMask.NameToLayer("Wall");
        Debug.DrawRay(transform.position, Vector2.left * 0.6f, Color.blue);
        rayHit2D = Physics2D.Raycast(transform.position, Vector2.left * 0.6f, 1f, layermaskNum);

        if (rayHit2D.transform != null)
        {
            Is_Wall_left = true;
            // Debug.Log(rayHit2D.transform.position);
        }
        if (rayHit2D.transform == null)
        {
            Is_Wall_left = false;
            // Debug.Log(rayHit2D.transform.position);
        }
    }

    void Wall_check_Down()//타일맵 콜라이더에 막혀서 이동하지 못하는 경우에 발동시켜줄 함수 
    {
        layermaskNum = 1 << LayerMask.NameToLayer("Wall");
        Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.blue);
        rayHit2D = Physics2D.Raycast(transform.position, Vector2.down * 0.6f, 1f, layermaskNum);

        if (rayHit2D.transform != null)
        {
            Is_Wall_down = true;
            // Debug.Log(rayHit2D.transform.position);
        } if (rayHit2D.transform == null)
        {
            Is_Wall_down = false;
            // Debug.Log(rayHit2D.transform.position);
        }

    }

    void Wall_check_Up()//타일맵 콜라이더에 막혀서 이동하지 못하는 경우에 발동시켜줄 함수 
    {
        layermaskNum = 1 << LayerMask.NameToLayer("Wall");
        Debug.DrawRay(transform.position, Vector2.up * 0.6f, Color.blue);
        rayHit2D = Physics2D.Raycast(transform.position, Vector2.up * 0.6f, 1f, layermaskNum);

        if (rayHit2D.transform != null)
        {
            Is_Wall_up = true;
            // Debug.Log(rayHit2D.transform.position);
        }if (rayHit2D.transform == null)
        {
            Is_Wall_up = false;
            // Debug.Log(rayHit2D.transform.position);
        }

    }

}
//void ray()
//{
//    test = Physics2D.OverlapCircleAll(transform.position, 100f);
//    //Debug.Log(Wall_check);

//    XSize = (int)(TopRight.x - bottomLeft.x);
//    YSize = (int)(TopRight.y - bottomLeft.y);

//    wall_Pos = new Wall_pos[XSize, YSize];

//    for(int i = 0; i<XSize; i++)
//    {
//        for(int j=0; j<YSize; j++)
//        {
//            foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
//            {
//                if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
//                {
//                    isWall = true;
//                    wall_Pos[i, j] = new Wall_pos((int)(i + bottomLeft.x), (int)(j + bottomLeft.y));

//                    //wall_Pos[i, j].Show_x_y();
//                }
//            }

//        }

//    }


//}