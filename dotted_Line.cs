using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotted_Line : MonoBehaviour
{
    public enum Enemy_MOVE
    {
        STOP,
        Go_Right,
        Go_Left,
        Go_Down,
        Go_Up

    }

    public enum Enemy_pos
    {
        Right_Up,
        Right_Down,
        Left_Up,
        Left_Down
    }



    public Enemy_MOVE E_M;
    public Enemy_pos E_p;

    public Vector2 Destination;

    Animator animator;

    public bool Move_Check_bool = true;
    public bool Go_Back = false;

    Vector2 Move_check;


    Vector2 Enemy_Start_Pos;
    Vector2 Enemy_X;
    Vector2 Enemy_Y;

    Vector2 LR_X;
    Vector2 LR_Y;

    int rand_num;

    LineRenderer LR;
    public GameObject Enemy_gameobject;
    float TTime = 0f;

    //float distance_square = 10f;

    public void Start()
    { 
        LR = GetComponent<LineRenderer>();
        animator = Enemy_gameobject.GetComponent<Animator>();
        Selecting_Enemy_pos();
    }

    private void Update()
    {
        TTime += Time.deltaTime;
        if (TTime >= 2)
        {
            //Debug.Log(Move_check.x == Square.transform.position.x);
            if (Move_check.x == Enemy_gameobject.transform.position.x && Move_check.y == Enemy_gameobject.transform.position.y)
            {
                if (Vector2.Distance(Move_check, Destination) <= 0.15f)
                {
                    Debug.Log(Move_check + " , " + Destination);
                    Go_Back = true;
                }
                if (Vector2.Distance(Move_check, Enemy_Start_Pos) <= 0.15f)
                {
                    Debug.Log(Move_check + " , " + Destination);
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
                    //¸ØÃçÀÖ´Â Áß 
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
        LR.SetPosition(0, Enemy_Start_Pos);
        if (rand_num == 1)
        {
            LR.SetPosition(1, new Vector2(Destination.x, Enemy_Start_Pos.y));
            LR.SetPosition(3, new Vector2(Enemy_Start_Pos.x, Destination.y));

        }
        else
        {
            LR.SetPosition(1, new Vector2(Enemy_Start_Pos.x, Destination.y));
            LR.SetPosition(3, new Vector2(Destination.x, Enemy_Start_Pos.y));

        }


        LR.SetPosition(2, Destination);
        LR.SetPosition(4, Enemy_Start_Pos);
        Enemy_X = new Vector2(Enemy_gameobject.transform.position.x, 0);
        Enemy_Y = new Vector2(0, Enemy_gameobject.transform.position.y);

        if (rand_num == 1)
        {
            LR_X = new Vector2(LR.GetPosition(1).x, 0);
            LR_Y = new Vector2(0, LR.GetPosition(2).y);
        }
        else
        {
            LR_X = new Vector2(LR.GetPosition(2).x, 0);
            LR_Y = new Vector2(0, LR.GetPosition(1).y);
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
        if(rand_num == 1)
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
