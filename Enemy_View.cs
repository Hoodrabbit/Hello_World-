using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_View : MonoBehaviour
{


    public dotted_Line Enemy_;
    LineRenderer Enemy_view_Line;
    float ray_distance = 5f;
    public LayerMask LM;

    // Start is called before the first frame update
    void Start()
    {
        Enemy_view_Line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemy_ray();
    }

    void Enemy_ray() // 적의 이동방향이 특정 상태일 때마다 광선을 쏘는 방향이 달라짐
    {
        if(Enemy_.E_S == dotted_Line.Enemy_State.Enemy_Noraml_Moving)
        {
            switch (Enemy_.E_M)
            {
                case dotted_Line.Enemy_MOVE.STOP:
                    {
                        break;
                    }
                case dotted_Line.Enemy_MOVE.Go_Right:
                    {
                        Debug.DrawRay(transform.position, Vector2.right * ray_distance);
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, ray_distance, LM);
                        Enemy_view_Line.SetPosition(0, transform.position);
                        Enemy_view_Line.SetPosition(1, new Vector2(transform.position.x + ray_distance, transform.position.y));
                        Hit_check(hit);
                        break;
                    }
                case dotted_Line.Enemy_MOVE.Go_Left:
                    {
                        Debug.DrawRay(transform.position, Vector2.left * ray_distance);
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, ray_distance, LM);
                        Enemy_view_Line.SetPosition(0, transform.position);
                        Enemy_view_Line.SetPosition(1, new Vector2(transform.position.x - ray_distance, transform.position.y));
                        Hit_check(hit);
                        break;
                    }
                case dotted_Line.Enemy_MOVE.Go_Down:
                    {
                        Debug.DrawRay(transform.position, Vector2.down * ray_distance);
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ray_distance, LM);
                        Enemy_view_Line.SetPosition(0, transform.position);
                        Enemy_view_Line.SetPosition(1, new Vector2(transform.position.x, transform.position.y - ray_distance));
                        Hit_check(hit);
                        break;
                    }
                case dotted_Line.Enemy_MOVE.Go_Up:
                    {
                        Debug.DrawRay(transform.position, Vector2.up * ray_distance);
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, ray_distance, LM);
                        Enemy_view_Line.SetPosition(0, transform.position);
                        Enemy_view_Line.SetPosition(1, new Vector2(transform.position.x, transform.position.y + ray_distance));
                        Hit_check(hit);
                        break;
                    }

                default:
                    break;
            }
        }
        
    }

    void Hit_check(RaycastHit2D hiT)
    {
        if(hiT.transform != null)
        {
           if(hiT.transform.tag == "Player")
            {
                Enemy_.Find_player = true;
            }
        }
    }



}
