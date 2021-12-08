using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotted_Line : MonoBehaviour
{

    public enum Enemy_State
    {
        Enemy_Noraml_Moving,
        Enemy_Player_Chasing,
        Enemy_Return_Start_Pos
    }

    public enum Enemy_MOVE
    {
        STOP,
        Go_Right,
        Go_Left,
        Go_Down,
        Go_Up,
    }

    public enum Enemy_pos
    {
        Right_Up,
        Right_Down,
        Left_Up,
        Left_Down
    }

    enum Enemy_Color
    {
        visible,
        invisble
    }
    Enemy_Color E_C;


    public Enemy_MOVE E_M;
    public Enemy_pos E_p;
    public Enemy_State E_S;

    public Vector2 Destination;

    Animator animator;

    public bool Move_Check_bool = true; //적이 정해진 길을 따라 움직이는 상태에서 움직이는 지 체크를 하여 안 움직이면 다른 방향으로 전환을 시킴
    public bool Go_Back = false;

    public bool Find_player = false;
    int check_enum;
    Vector2 Enemy_Before_Pos;

    Vector2 Move_check;

    public Vector2 Enemy_Start_Pos;
    Vector2 Enemy_X;
    Vector2 Enemy_Y;

    Vector2 LR_X;
    Vector2 LR_Y;

    int rand_num;

    LineRenderer Enemy_Moving_Route;
    public LineRenderer Enemy_Ray_check_visible;

    public GameObject Enemy_gameobject;
    float TTime = 0f;

    //float distance_square = 10f;

    public void Start()
    {
        Enemy_Moving_Route = GetComponent<LineRenderer>();
        animator = Enemy_gameobject.GetComponent<Animator>();

        E_S = Enemy_State.Enemy_Noraml_Moving;
        E_C = Enemy_Color.visible;
        Selecting_Enemy_pos();
    }

    private void Update()
    {

        if (Find_player == true)
        {
            check_enum = (int)E_M;
            Enemy_Before_Pos = Enemy_gameobject.transform.position;

            E_S = Enemy_State.Enemy_Player_Chasing;
        }

        if (Find_player == false && E_S == Enemy_State.Enemy_Player_Chasing)
        {

            Enemy_gameobject.transform.position = Enemy_Before_Pos; // 일단 임시로 이렇게 해놓음
            E_M = (Enemy_MOVE)check_enum;//이렇게 하면 되네 굿
        }

        Control_Enemy_State(E_S);



    }

    void Control_Enemy_State(Enemy_State enemy_State)
    {
        switch (enemy_State)
        {
            case Enemy_State.Enemy_Noraml_Moving:
                {
                    Enemy_Ray_check_visible = Enemy_gameobject.GetComponent<LineRenderer>();
                    if(Enemy_Ray_check_visible.enabled == false)
                    {
                        Enemy_Ray_check_visible.enabled = true;
                    }

                    TTime += Time.deltaTime;
                    if (TTime >= 2)
                    {
                        //Debug.Log(Move_check.x == Square.transform.position.x);
                        if (Move_check.x == Enemy_gameobject.transform.position.x && Move_check.y == Enemy_gameobject.transform.position.y)
                        {
                            if (Vector2.Distance(Move_check, Destination) <= 0.15f)
                            {
                                //Debug.Log(Move_check + " , " + Destination); 
                                Go_Back = true;
                            }
                            if (Vector2.Distance(Move_check, Enemy_Start_Pos) <= 0.15f)
                            {
                                //Debug.Log(Move_check + " , " + Destination);
                                Go_Back = false;
                            }
                            Move_Check_bool = false;

                        }

                        Move_check = new Vector2(Enemy_gameobject.transform.position.x, Enemy_gameobject.transform.position.y);
                        TTime = 0;
                    }

                    Set_Enemy_moving_range();

                    switch (E_M)
                    {
                        case Enemy_MOVE.STOP:
                            {
                                //멈춰있는 중 
                                break;
                            }

                        case Enemy_MOVE.Go_Right:
                            {
                                Move_Right();
                                animator.SetInteger("Direction", 2);

                                break;
                            }
                        case Enemy_MOVE.Go_Left:
                            {
                                Move_Left();
                                animator.SetInteger("Direction", 3);

                                break;
                            }
                        case Enemy_MOVE.Go_Down:
                            {
                                Move_Down();
                                animator.SetInteger("Direction", 0);
                                break;
                            }
                        case Enemy_MOVE.Go_Up:
                            {
                                Move_Up();
                                animator.SetInteger("Direction", 1);
                                break;
                            }
                    }
                    break;
                }
            case Enemy_State.Enemy_Player_Chasing:
                {
                    //Enemy_Move_Controller.cs
                    break;
                }
            case Enemy_State.Enemy_Return_Start_Pos:
                {
                    //Debug.Log("왜 안됨222");
                    SpriteRenderer Enemy_SR = Enemy_gameobject.GetComponent<SpriteRenderer>();
                    switch (E_C)
                    {
                        case Enemy_Color.visible:
                            {
                                //Debug.Log("왜 안됨111");
                                Enemy_SR.color = Color.Lerp(Enemy_SR.color, Color.clear, Time.deltaTime);
                                //Debug.Log(Enemy_SR.color.a);
                                if (Enemy_SR.color.a < 0.01f)
                                {
                                    //Debug.Log("왜 안됨");
                                    E_C = Enemy_Color.invisble;
                                }
                                break; 
                            }
                        case Enemy_Color.invisble:
                            {
                                Enemy_gameobject.transform.position = Vector2.MoveTowards(Enemy_gameobject.transform.position, Enemy_Start_Pos, 1);
                                Enemy_SR.color = Color.Lerp(Enemy_SR.color, Color.black, Time.deltaTime);
                                if(Enemy_SR.color.a > 0.99f)
                                {
                                    Selecting_Enemy_pos();
                                    E_S = Enemy_State.Enemy_Noraml_Moving;
                                    E_C = Enemy_Color.visible;
                                    

                                }
                                break;
                            }
                    }
                   
                    
                    //Enemy_gameobject.transform.position = Vector2.MoveTowards(Enemy_gameobject.transform.position, Enemy_Start_Pos, 1);
                    break;
                }
        }
    }



    void Selecting_Enemy_pos()
    {
        rand_num = Random.Range(0, 2);
        Move_check = new Vector2(Enemy_gameobject.transform.position.x, Enemy_gameobject.transform.position.y);
        Enemy_Start_Pos = Enemy_gameobject.transform.position;
        if (rand_num == 1)
        {
            if (Enemy_Start_Pos.x < Destination.x && Enemy_Start_Pos.y < Destination.y)
            {
                E_M = Enemy_MOVE.Go_Right;
                E_p = Enemy_pos.Left_Down;
            }
            if (Enemy_Start_Pos.x < Destination.x && Enemy_Start_Pos.y > Destination.y)
            {
                E_M = Enemy_MOVE.Go_Right;
                E_p = Enemy_pos.Left_Up;

            }
            if (Enemy_Start_Pos.x > Destination.x && Enemy_Start_Pos.y < Destination.y)
            {
                E_M = Enemy_MOVE.Go_Left;
                E_p = Enemy_pos.Right_Down;
            }
            if (Enemy_Start_Pos.x > Destination.x && Enemy_Start_Pos.y > Destination.y)
            {
                E_M = Enemy_MOVE.Go_Left;
                E_p = Enemy_pos.Right_Up;
            }
        }
        else
        {
            if (Enemy_Start_Pos.x < Destination.x && Enemy_Start_Pos.y < Destination.y)
            {
                E_M = Enemy_MOVE.Go_Up;
                E_p = Enemy_pos.Left_Down;
            }
            if (Enemy_Start_Pos.x < Destination.x && Enemy_Start_Pos.y > Destination.y)
            {
                E_M = Enemy_MOVE.Go_Down;
                E_p = Enemy_pos.Left_Up;
            }
            if (Enemy_Start_Pos.x > Destination.x && Enemy_Start_Pos.y < Destination.y)
            {
                E_M = Enemy_MOVE.Go_Up;
                E_p = Enemy_pos.Right_Down;
            }
            if (Enemy_Start_Pos.x > Destination.x && Enemy_Start_Pos.y > Destination.y)
            {
                E_M = Enemy_MOVE.Go_Down;
                E_p = Enemy_pos.Right_Up;
            }
        }
    }

    void Set_Enemy_moving_range()
    {
        Enemy_Moving_Route.SetPosition(0, Enemy_Start_Pos);
        if (rand_num == 1)
        {
            Enemy_Moving_Route.SetPosition(1, new Vector2(Destination.x, Enemy_Start_Pos.y));
            Enemy_Moving_Route.SetPosition(3, new Vector2(Enemy_Start_Pos.x, Destination.y));

        }
        else
        {
            Enemy_Moving_Route.SetPosition(1, new Vector2(Enemy_Start_Pos.x, Destination.y));
            Enemy_Moving_Route.SetPosition(3, new Vector2(Destination.x, Enemy_Start_Pos.y));

        }


        Enemy_Moving_Route.SetPosition(2, Destination);
        Enemy_Moving_Route.SetPosition(4, Enemy_Start_Pos);
        Enemy_X = new Vector2(Enemy_gameobject.transform.position.x, 0);
        Enemy_Y = new Vector2(0, Enemy_gameobject.transform.position.y);

        if (rand_num == 1)
        {
            LR_X = new Vector2(Enemy_Moving_Route.GetPosition(1).x, 0);
            LR_Y = new Vector2(0, Enemy_Moving_Route.GetPosition(2).y);
        }
        else
        {
            LR_X = new Vector2(Enemy_Moving_Route.GetPosition(2).x, 0);
            LR_Y = new Vector2(0, Enemy_Moving_Route.GetPosition(1).y);
        }
    }


    void Move_Right()
    {
        if (rand_num == 1)
        {
            switch (E_p)
            {
                case Enemy_pos.Right_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Up;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Right_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Down;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Down;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Up;
                            Move_Check_bool = true;
                        }
                        break;
                    }
            }
        }

        else
        {
            switch (E_p)
            {
                case Enemy_pos.Right_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Down;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Right_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Up;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Up;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Down;
                            Move_Check_bool = true;
                        }
                        break;
                    }

            }
        }
        if (Vector2.Distance(Enemy_X, LR_X) >= 0.1f && Go_Back == false)
        {
            Enemy_gameobject.transform.position = new Vector2(Enemy_gameobject.transform.position.x + 1 * Time.deltaTime, Enemy_gameobject.transform.position.y);
        }

        if (Vector2.Distance(Enemy_X, new Vector2(Enemy_Start_Pos.x, 0)) >= 0.1f && Go_Back == true)
        {
            Enemy_gameobject.transform.position = new Vector2(Enemy_gameobject.transform.position.x + 1 * Time.deltaTime, Enemy_gameobject.transform.position.y);
        }


    }

    void Move_Left()
    {
        if (rand_num == 1)
        {
            switch (E_p)
            {
                case Enemy_pos.Right_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Down;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Right_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Up;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Up;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Down;
                            Move_Check_bool = true;
                        }
                        break;
                    }
            }
        }

        else
        {
            switch (E_p)
            {
                case Enemy_pos.Right_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Up;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Right_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Down;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Down;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Up;
                            Move_Check_bool = true;
                        }
                        break;
                    }

            }
        }

        if (Vector2.Distance(Enemy_X, LR_X) >= 0.1f && Go_Back == false)
        {
            Enemy_gameobject.transform.position = new Vector2(Enemy_gameobject.transform.position.x - 1 * Time.deltaTime, Enemy_gameobject.transform.position.y);
        }

        if (Vector2.Distance(Enemy_X, new Vector2(Enemy_Start_Pos.x, 0)) >= 0.1f && Go_Back == true)
        {
            Enemy_gameobject.transform.position = new Vector2(Enemy_gameobject.transform.position.x - 1 * Time.deltaTime, Enemy_gameobject.transform.position.y);
        }
    }

    void Move_Up()
    {
        if (rand_num == 1)
        {
            switch (E_p)
            {
                case Enemy_pos.Right_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Left;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Right_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Right;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Right;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Left;
                            Move_Check_bool = true;
                        }
                        break;
                    }
            }
        }

        else
        {
            switch (E_p)
            {
                case Enemy_pos.Right_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Right;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Right_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Left;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Left;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Right;
                            Move_Check_bool = true;
                        }
                        break;
                    }

            }
        }

        if (Vector2.Distance(Enemy_Y, LR_Y) >= 0.1f && Go_Back == false)
        {
            Enemy_gameobject.transform.position = new Vector2(Enemy_gameobject.transform.position.x, Enemy_gameobject.transform.position.y + 1 * Time.deltaTime);
        }

        if (Vector2.Distance(Enemy_Y, new Vector2(0, Enemy_Start_Pos.y)) >= 0.1f && Go_Back == true)
        {
            Enemy_gameobject.transform.position = new Vector2(Enemy_gameobject.transform.position.x, Enemy_gameobject.transform.position.y + 1 * Time.deltaTime);
        }

    }

    void Move_Down()
    {
        if (rand_num == 1)
        {
            switch (E_p)
            {
                case Enemy_pos.Right_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Right;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Right_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Left;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Left;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Right;
                            Move_Check_bool = true;
                        }
                        break;
                    }
            }
        }

        else
        {
            switch (E_p)
            {
                case Enemy_pos.Right_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Left;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Right_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Right;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Up:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Right;
                            Move_Check_bool = true;
                        }
                        break;
                    }
                case Enemy_pos.Left_Down:
                    {
                        if (Move_Check_bool == false)
                        {
                            E_M = Enemy_MOVE.Go_Left;
                            Move_Check_bool = true;
                        }
                        break;
                    }

            }
        }

        if (Vector2.Distance(Enemy_Y, LR_Y) >= 0.1f && Go_Back == false)
        {
            Enemy_gameobject.transform.position = new Vector2(Enemy_gameobject.transform.position.x, Enemy_gameobject.transform.position.y - 1 * Time.deltaTime);
        }

        if (Vector2.Distance(Enemy_Y, new Vector2(0, Enemy_Start_Pos.y)) >= 0.1f && Go_Back == true)
        {
            Enemy_gameobject.transform.position = new Vector2(Enemy_gameobject.transform.position.x, Enemy_gameobject.transform.position.y - 1 * Time.deltaTime);
        }

    }

   




}
