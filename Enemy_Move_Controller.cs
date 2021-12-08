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
    public GameObject Enemy_Prefab;
    public GameObject Enemy;
    public Vector2 TopRight, bottomLeft;
    int XSize, YSize;
    dotted_Line Enemy_state;
    //public Collider2D[] test;
    //public Collider2D[] col;
    Rigidbody2D rigid2D;
    RaycastHit2D rayHit2D;
    int layermaskNum;

    float TTime = 0f;

    LineRenderer LR_Player_check;

    public bool Is_Wall_right = false;
    public bool Is_Wall_up = false;
    public bool Is_Wall_down = false;
    public bool Is_Wall_left = false;

    bool Time_over = false;
    bool Start_Timer = true;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        M_S = Move_State.None;
        Enemy_state = GetComponentInChildren<dotted_Line>();
        rigid2D = GetComponent<Rigidbody2D>();
        LR_Player_check = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(6, 7);
        Physics2D.IgnoreLayerCollision(7, 7);
    }

    // Update is called once per frame
    void Update()
    {
        

        if(M_S == Move_State.Chasing)
        {
            //Debug.Log("플레이어 쫓고 있는 중");
        }

        if(M_S == Move_State.Wall_Checking)
        {
            //Debug.Log("근처에 벽이 있음");
        }

        if (Enemy_state.E_S == dotted_Line.Enemy_State.Enemy_Player_Chasing)
        {
            TTime += Time.deltaTime;

            if(TTime<10)
            {

                //여기에 자신의 하위 오브젝트에 canvas만들고 텍스트도 복제하는 스크립트 작성하기
                if(Start_Timer == true)
                {
                    Enemy = Instantiate(Enemy_Prefab);
                    Enemy.transform.SetParent(transform);
                    Start_Timer = false;
                }
                

                Control_Wall_check_bool();
                LR_Player_check.enabled = false;
                if (Is_Wall_check())
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

            if(TTime >=10)
            {
                
                //Debug.Log("왜 안될까");
                Time_over = true;
            }

            if(Time_over == true)
            {
                rigid2D.velocity = Vector2.zero;
                Enemy_state.E_S = dotted_Line.Enemy_State.Enemy_Return_Start_Pos;
                Enemy_state.Find_player = false;
                Enemy_state.Go_Back = false;
                Time_over = false;
                TTime = 0;
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
        //Debug.Log(number);
        switch (number)
        {
            case 1:
                {
                    rigid2D.velocity = Vector2.right * 2f;
                    animator.SetInteger("Direction", 2);
                    //오른쪽 이동
                    break;
                }
            case 2:
                {
                    rigid2D.velocity = Vector2.left * 2f;
                    animator.SetInteger("Direction", 3);
                    //왼쪽 이동
                    break;
                }
            case 3:
                {
                    rigid2D.velocity = Vector2.up * 2f;
                    animator.SetInteger("Direction", 0);
                    //위쪽 이동
                    break;
                }

            case 4:
                {
                    rigid2D.velocity = Vector2.down * 2f;
                    animator.SetInteger("Direction", 1);
                    //아래쪽 이동
                    break;
                }


        }


    }


    void Chasing_player()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, Time.deltaTime *2f);

        if(Player.transform.position.x > transform.position.x)
        {
            animator.SetInteger("Direction", 2);
        }
        else
        {
            animator.SetInteger("Direction", 3);
        }

        if(Player.transform.position.y > transform.position.y)
        {
            animator.SetInteger("Direction", 0);
        }
        else
        {
            animator.SetInteger("Direction", 1);
        }

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

    public float TTime_Value()
    {
        return TTime;
    }


}
