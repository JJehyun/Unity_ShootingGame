using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   //적 삭제시 SCore
    public int enemyScore;
    //적 이동 속도
    public float speed;
    //적 피통
    public int health;
    //이미지를 가져오기 위함
    public Sprite[] sprites;
    //이미지 변경을 위함
    SpriteRenderer spriteRenderer;
    //총알 변수를 담을 오브젝트를 생성
    public GameObject bulletObjA;
    public GameObject bulletObjB;   
    //총알발사 억제를 위한 변수2개maxShotDelay,curShotDelay
    public float maxShotDelay;// 0.1설정 -> 0.1초마다 총알 나감 , 0.2설정 -> 0.2초 마다 총알 나감
    public float curShotDelay;
    public string enemyName;
    public GameObject player;
    //아이템 드랍 변수
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    //데미지 관련 함수
    public void OnHit(int dmg){
        if(health <= 0)
            return;
        health -= dmg;
            //피격 당했을 때 이미지 변경 logic 1줄!
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite",0.1f);
            //피가 0 이하가 된다면 자기자신을 파괴한다.
            if(health <= 0){
                Player playerLogic = player.GetComponent<Player>();//component의 Player script를 가져옴
                playerLogic.score += enemyScore; 


                //랜덤 아이템 드랍 관련 로직
                int ran = Random.Range(0,10);
                if(ran < 3){// 30%
                    Debug.Log("아이템 없음");
                }
                else if(ran < 6){//Coin 드랍 30%
                    Instantiate(itemCoin,transform.position,itemCoin.transform.rotation);
                }
                else if(ran < 8){//Power 드랍 20%
                Instantiate(itemPower,transform.position,itemPower.transform.rotation);
                }
                else if(ran < 10){//Boom 드랍 20%
                Instantiate(itemBoom,transform.position,itemBoom.transform.rotation);
                }
                Destroy(gameObject);
            }
    }

    //피격 당한 후 원래이미지로 돌아오는 함수
    void ReturnSprite(){
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "BorderBullet")
            Destroy(gameObject);
        else if(other.gameObject.tag == "PlayerBullet"){
        //사용자가 만든 Bullet script 가져옴
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        //hit함수 호출
        OnHit(bullet.dmg);
        Destroy(other.gameObject);
        }
    }

    void Update()
    {   
        //총 쏘기 관련 logic
        Fire();
        //총알발사 느리게하는 함수
        Reload();
    }

    void Fire(){
        //장전이 되기전에는 발사체가 나가지 않는다.
        if(curShotDelay < maxShotDelay){
            return;
        }
        if(enemyName == "S"){
                GameObject Bullet = Instantiate(bulletObjA,transform.position,transform.rotation); //오브젝트 생성
                Rigidbody2D rigid = Bullet.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                //목표물 방향 = 목표물 위치 - 자신의 위치
                Vector3 dirVec = player.transform.position -  transform.position;
                rigid.AddForce(dirVec.normalized * 5,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
        }
        if(enemyName == "L"){
                GameObject BulletR = Instantiate(bulletObjA,transform.position + Vector3.right * 0.3f,transform.rotation);
                GameObject BulletL = Instantiate(bulletObjA,transform.position + Vector3.left * 0.3f,transform.rotation);
                Rigidbody2D rigidR = BulletR.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                Rigidbody2D rigidL = BulletL.GetComponent<Rigidbody2D>();//변수 초기화, 물리작용 추가
                Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
                Vector3 dirVecL = player.transform.position - (transform.position + Vector3.right * 0.3f);
                rigidR.AddForce(dirVecR.normalized * 5,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
                rigidL.AddForce(dirVecL.normalized * 5,ForceMode2D.Impulse);  // 오브젝트 위쪽으로 발사
        }
        curShotDelay = 0;
    }

    //총알발사를 지연시켜주는 함수
    void Reload(){
        curShotDelay += Time.deltaTime; //Time.deltaTime는 시간이 지날수록 점점 증가한다.
    }

}
