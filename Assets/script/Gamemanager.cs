using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{   //적 프리펩 배열 생성
    public GameObject[] enemyObj;
    //프리펩 생성위치 배열 생성
    public Transform[] spawnPoints;
    //적생성 딜레이 생성
    public float maxSpawnDelay;
    public float curSpawnDelay;
    public GameObject player;
    //UI변경
    public Text scoreText;//점수 
    public Image[] lifeImg;// 생명 이미지
    public Image[] boomImg;//필살기 이미지
    public GameObject gameOverSet;

    void Update(){
        curSpawnDelay += Time.deltaTime; //계속 증가하는 값

        //시간이 지나면 적 소환
        if(curSpawnDelay > maxSpawnDelay){
            maxSpawnDelay = Random.Range(0.5f,3f); //0.5초부터 3초 까지 랜덤하게 숫자가 바뀐다. 딜레이 랜덤하게 바꿈
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        //ui 점수판 변경 로직
        Player playerLogic = player.GetComponent<Player>(); //component 스크립트 파일 가져옴
        scoreText.text = string.Format("{0:n0}",playerLogic.score);
    }

    void SpawnEnemy(){
        int ranEnemy = Random.Range(0,3);//0,1,2 셋중하나 적 랜덤 소환
        int ranPoint = Random.Range(0,9);//소환 포인트 0,1,2,3,4 중 하나
        GameObject enemy = Instantiate(enemyObj[ranEnemy],spawnPoints[ranPoint].position,spawnPoints[ranPoint].rotation);
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();

        //적이 생성된 후 player오브젝트를 넘겨줌
        enemyLogic.player = player;

        if(ranPoint == 5 || ranPoint ==6){
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed*(-1),-1);
        }else if(ranPoint == 7 || ranPoint == 8){
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed,-1);
        }else{
            rigid.velocity = new Vector2(0,enemyLogic.speed * (-1));
        }
    }
        //플레이어 죽었을 때 다시 태어나는 로직
    public void RespawnPlayer(){
        Invoke("RespawnPlayerExe",2f);
    }
        //플레이어 죽었을 때2초뒤에 다시 리스폰하는 logic
    void RespawnPlayerExe(){
        // (0,3.5,0)자리에 player 리스폰한다.
        player.transform.position = Vector3.down * 3.5f;
        //player를 활성화 한다.
        player.SetActive(true);

        //아래 두줄 중복 피격을 방지하기 위한 로직
        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }


        //생명아이콘 사라지게 하는 로직
    public void UpdateLifeIcon(int life){
        for (int index=0; index<3; index++){
            lifeImg[index].color = new Color (1,1,1,0);  }  //색상을 모두 지움

        for (int index=0; index<life; index++){
            lifeImg[index].color = new Color (1,1,1,1);    //색상을 모두 모두 킴
                }
    }
    //생명다써서 게임 오버 로직
    public void GameOver(){
        gameOverSet.SetActive(true);
    }

    //player 사망 후 retry 버튼을 눌렀을 때 다시 시작 logic
    public void GameRetry(){
        SceneManager.LoadScene(0);
    }


        //필살기 ui 컨트롤
    public void UpdateBoomIcon(int boom){
        for (int index=0; index<3; index++){
            boomImg[index].color = new Color (1,1,1,0);  }  //색상을 모두 지움

        for (int index=0; index<boom; index++){
            boomImg[index].color = new Color (1,1,1,1);    //색상을 모두 모두 킴
                }
    }
}
