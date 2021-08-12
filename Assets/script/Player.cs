using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   

    //player 이동 속도
    public float speed;
    //플레이어 목숨
    public int life;
    //플레이어 점수
    public int score;
    //총알발사 억제를 위한 변수2개maxShotDelay,curShotDelay
    public float maxShotDelay;// 0.1설정 -> 0.1초마다 총알 나감 , 0.2설정 -> 0.2초 마다 총알 나감
    public float curShotDelay;
    public int power;
    //사용자의 power의 최대값
    public int maxpower;
    //player 필살기
    public int boom;
    public int maxBoom;
    public bool isBoomTime;
    public GameObject boomEffect;
    //사물에 닿음 변수 속력을 0 만들어줌(움직이지 않게)
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;
    Animator anim;
    //총알 변수를 담을 오브젝트를 생성
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public Gamemanager manager; 
    public bool isHit; // 중복 피격을 방지하기 위한 변수 
    void Awake(){
        anim = GetComponent<Animator>();
    }


    void Update()
    {   //움직임 관련 logic
        Move();
        //총 쏘기 관련 logic
        Fire();
        Boom();
        //총알발사 느리게하는 함수
        Reload();
    }

    //움직임 관련 logic(Move)
    void Move(){
        //이동 logic 키보드 입력시 값이 float값으로 들어옴
        float h =Input.GetAxisRaw("Horizontal"); //GetAxisRaw값은 -1,0,1 3가지 값만 들어올 수 있다. (수평)
        float v =Input.GetAxisRaw("Vertical");   //수직

        if((isTouchRight && h == 1) || (isTouchLeft && h == -1))
        {h=0;}

        if((isTouchTop && v == 1) || (isTouchBottom && v == -1))
        {v=0;}
        //현재 위치값 받아옴
        Vector3 curPos = transform.position;
        //다음 위치(물리적인 이동이 아닌 transform이동은 Time.deltaTime 사용해야한다.)
        Vector3 nextPos = new Vector3(h,v,0)*speed*Time.deltaTime;

        transform.position = curPos + nextPos;
        //이동 관련logic 끝


        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")){
            anim.SetInteger("Input",(int)h);
        }
    }
    //움직임 관련 logic(Move) 끝

    void Fire(){
        //버튼을 안 눌렀을때 총알발사X
        if(!Input.GetButton("Fire1"))
            return;

        //장전이 되기전에는 발사체가 나가지 않는다.
        if(curShotDelay < maxShotDelay){
            return;
        }

        switch(power){
            case 1: //총알 한발
                //prefeb를 오브젝트로 생성하는 함수(Instantiate)
                //Instantiate(프리펩,생성 위치, 방향)
                GameObject Bullet = Instantiate(bulletObjA,transform.position,transform.rotation); //오브젝트 생성
                Rigidbody2D rigid = Bullet.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                rigid.AddForce(Vector2.up*15,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
            break;
            case 2: //총알 두발
                GameObject BulletR = Instantiate(bulletObjA,transform.position + Vector3.right * 0.1f,transform.rotation);
                GameObject BulletL = Instantiate(bulletObjA,transform.position + Vector3.left * 0.1f,transform.rotation);
                Rigidbody2D rigidR = BulletR.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                Rigidbody2D rigidL = BulletL.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                rigidR.AddForce(Vector2.up*15,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
                rigidL.AddForce(Vector2.up*15,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
            break;
            case 3:
                GameObject BulletRR = Instantiate(bulletObjA,transform.position + Vector3.right * 0.25f,transform.rotation);
                GameObject BulletLL = Instantiate(bulletObjA,transform.position + Vector3.left * 0.25f,transform.rotation);
                GameObject BulletCC = Instantiate(bulletObjB,transform.position,transform.rotation);
                Rigidbody2D rigidCC = BulletCC.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                Rigidbody2D rigidRR = BulletRR.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                Rigidbody2D rigidLL = BulletLL.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                rigidCC.AddForce(Vector2.up*15,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
                rigidRR.AddForce(Vector2.up*15,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
                rigidLL.AddForce(Vector2.up*15,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
            break;
        }
        curShotDelay = 0;
    }

    //총알발사를 지연시켜주는 함수
    void Reload(){
        curShotDelay += Time.deltaTime; //Time.deltaTime는 시간이 지날수록 점점 증가한다.
    }

    //경계에 충돌했을 때 속력을 0으로 변경
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Border"){
            switch (other.gameObject.name)
            {
                case "TopBorder":{isTouchTop = true;
                }
                break;

                case "BottomBoder":
                isTouchBottom = true;
                break;

                case "RightBorder":
                isTouchRight =true;
                break;

                case "LeftBorder":
                isTouchLeft = true;
                break;
            }
        } 
            //적에게 부딪혔을때 생명 -1
        else if(other.gameObject.tag == "Enemy" || other.gameObject.tag =="EnemyBullet"){
            if(isHit)  //맞았다면 아래함수는 실행하지 않는다. 중복피격을 방지하기 위한 로직
                return;

            isHit = true;
            life--;  //생명 줄음
            manager.UpdateLifeIcon(life);   //생명이미지 줄임 logic
            if(life == 0){
            manager.GameOver();             //생명다쓰면 게임 끝남
            }else{}
            manager.RespawnPlayer();        //player 리스폰
            gameObject.SetActive(false);    //player 피격시 잠깐 사라짐
        }

        //아이템과 충돌했을 때
        else if(other.gameObject.tag == "Item"){
            //충돌한 아이템의 script 컴포넌트를 가져온다.
            Item item = other.gameObject.GetComponent<Item>();

            //충돌한 오브젝트의 스크립트 컴포넌트가 coin, power, boom일때를 나눠서 switch문을 사용한다.
            switch(item.type){
                case "Coin":
                score += 1000;
                break;
                case "Power":
                //power가 이미 3단계 일때 800점수 증가 3단계아니라면 power 1증가
                if(power == maxpower){
                    score += 800;
                }else{power++;
                }
                break;
                case "Boom":
                //power가 이미 3단계 일때 800점수 증가 3단계아니라면 power 1증가
                if(boom == maxBoom){
                    score += 800;
                }else{
                    boom++;
                    manager.UpdateBoomIcon(boom);
                }
                break;
            }
            Destroy(other.gameObject);
        }
    }

    //필살기 효과끄기
    void OffBoomEffect(){
        boomEffect.SetActive(false);
        isBoomTime = false;
    }


    //경계에 충돌했을 때 속력을 0으로 변경
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Border"){
            switch (other.gameObject.name)
            {
                case "TopBorder":
                isTouchTop = false;
                break;

                case "BottomBoder":
                isTouchBottom = false;
                break;

                case "RightBorder":
                isTouchRight =false;
                break;

                case "LeftBorder":
                isTouchLeft = false;
                break;
            }
        } 
    }

    void Boom(){
        //버튼을 안 눌렀을때 총알발사X
        if(!Input.GetButton("Fire2"))
            return;
        
        //필살기 Boom 사용하고 있으면 아래 logic를 실행하지 않는다.
        if(isBoomTime){
            return;
        }

        if(boom == 0){
            return;
        }
        boom--;
        manager.UpdateBoomIcon(boom);
        isBoomTime = true;
        //effect 효과주기
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect",4f);
        //Remove Enemy 씬위에 있는 특정한 테그를 모두 배열에 할당함
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //Enemy테그를 가진 모든 적의 script를 가져온다.
        for(int index = 0; index < enemies.Length; index++){
            Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
            //모든 enemy script를 가져와서 사용자 함수onhit로 데미지를 준다.
            enemyLogic.OnHit(1000);
        }
        //적의 총알 전부 사라지는 logic
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        //Enemy테그를 가진 모든 적의 script를 가져온다.
        for(int index = 0; index < bullets.Length; index++){
            Destroy(bullets[index]);
        }
    }



}
