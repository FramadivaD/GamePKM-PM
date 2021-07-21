using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float timeAttack;
    public float timeIdle;
    public float enemySpeed;
    bool isAttack = false;
    float startTimeAttack, startTimeIdle;
    Animator enemyAnim;
    public GameObject playerTarget;

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        startTimeIdle = timeIdle;
        startTimeAttack = timeAttack;
        playerTarget = GameObject.Find("JamurMerah");
    }

    
    void Update()
    {
        if(GameObject.Find("Game Manager").GetComponent<GameManager>().Allowed){
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, Time.deltaTime * enemySpeed);
            if(transform.position.x < playerTarget.transform.position.x && transform.rotation.y == 0){
                transform.eulerAngles = Vector3.down * 180f;
            }
            else if(transform.position.x > playerTarget.transform.position.x && transform.rotation.y != 0){
                transform.eulerAngles = Vector3.zero;
            }
            if(startTimeAttack <= 0 && !isAttack){
                enemyAnim.SetTrigger("StartAttack");
                isAttack = true;
                startTimeAttack = timeAttack;
            }else if(startTimeAttack > 0 && !isAttack){
                startTimeAttack -= Time.deltaTime;
            }
            if(startTimeIdle <= 0 && isAttack){
                enemyAnim.SetTrigger("StopAttack");
                isAttack = false;
                startTimeIdle = timeIdle;
            }else if(startTimeIdle > 0 && isAttack){
                startTimeIdle -= Time.deltaTime;
        }
        }
    }
}
