using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    Rigidbody2D rigid;

    void Awake()
    {   //컴포넌트 가져옴
        rigid =GetComponent<Rigidbody2D>();
        //컴포넌트를 가져온 후 아래 방향으로 3만큼의 속도를 준다. 중력의 영향을 없앴기 때문에 계속 3의 속도로 움직인다.
        rigid.velocity = Vector2.down * 3;
    }
}
