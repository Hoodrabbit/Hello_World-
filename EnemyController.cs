using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D Enemy_Rigidbody;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Enemy_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //���� ������ ���� ���� �������� ��
        timer += Time.deltaTime;
        if(timer >= 5 && timer <10)
        {
            Enemy_Rigidbody.velocity = Vector2.zero;
            //Debug.Log("�۵���");
            //Enemy_Rigidbody.velocity = Vector2.down;

        }
        if(timer >= 10)
        {
            Enemy_destination(Enemy_Rigidbody);
            
            timer = 0f;
        }
        
        


    }

    void Enemy_destination(Rigidbody2D Enemy)
    {
        Enemy.velocity = new Vector2(0, Random.Range(-1, 2));
    }


}
