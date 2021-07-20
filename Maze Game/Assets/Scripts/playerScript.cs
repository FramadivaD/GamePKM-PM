using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerScript : MonoBehaviour
{
    public GameObject mainCircle, outCircle;
    public bool isJoystick = true;
    Vector2 circleDir;
    Rigidbody2D rb;
    Animator anim;
    gameManage GM;

    public float ms;
    float mss;
    void Start()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        GM = GameObject.Find("Game Manager").GetComponent<gameManage>();
        anim = GetComponent<Animator>();
        Invoke("triggerfalse", 0.5f);
        rb = GetComponent<Rigidbody2D>();
        mss = ms + 5;
    }

    void triggerfalse(){
        gameObject.GetComponent<Collider2D>().isTrigger = false;
    }

    void Update()
    {
        if(GM.Allowed == true){
            //Joystick Control
            if(isJoystick){
                if(Input.GetMouseButtonDown(0)){
                    Vector3 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 mouseDis = mouseDir - GameObject.Find("Main Camera").transform.position;
                    if(mouseDis.x <= -2f && mouseDis.y <= 0){
                        outCircle.SetActive(true);
                        outCircle.transform.position = new Vector3(mouseDir.x, mouseDir.y, outCircle.transform.position.z);
                    }
                }if(Input.GetMouseButtonUp(0)){
                    outCircle.SetActive(false);
                    circleDir = Vector2.zero;
                }
                if(outCircle.activeSelf){
                    Vector2 circleDis = Camera.main.ScreenToWorldPoint(Input.mousePosition) - outCircle.transform.position;
                    circleDir = Vector2.ClampMagnitude(circleDis, 1f);
                    mainCircle.transform.position = new Vector2(outCircle.transform.position.x + circleDir.x, outCircle.transform.position.y + circleDir.y);
                    
                }
            }else if(!isJoystick){
                outCircle.SetActive(false);
            }
            
            //Player Move
            float moveX = 0; 
            float moveY = 0;
            if(circleDir.x >= 0.2f || circleDir.x <= -0.2f){
                moveX = circleDir.x;
            }if(circleDir.y >= 0.2f || circleDir.y <= -0.2f){
                moveY = circleDir.y;
            }
            rb.velocity = new Vector2(moveX * ms, moveY * ms);

            if(rb.velocity.x > 0){
                anim.SetBool("isWalk", false);
                anim.SetBool("isWalkL", false);
                anim.SetBool("isWalkR", true);
            }else if(rb.velocity.x < 0){
                anim.SetBool("isWalk", false);
                anim.SetBool("isWalkL", true);
                anim.SetBool("isWalkR", false);
            }else if(rb.velocity.x == 0 && rb.velocity.y == 0){
                anim.SetBool("isWalk", false);
                anim.SetBool("isWalkL", false);
                anim.SetBool("isWalkR", false);
            }else if(rb.velocity.y != 0 && rb.velocity.x == 0){
                anim.SetBool("isWalk", true);
            }
            //Speed Up
            if(Input.GetKeyDown(KeyCode.LeftShift)){
                ms = mss;
            }if(Input.GetKeyUp(KeyCode.LeftShift)){
                ms = mss - 5;
            }
            
            
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Treasure")){
            GM.available = true;
            other.gameObject.GetComponent<SpriteRenderer>().sprite = GM.treasureSprite;
            GameObject.Find("InteractButton").GetComponent<Image>().color = new Color(1f,1f,1f,1f);
        }if(other.CompareTag("GateOpen")){
            GM.gateCheck = true;
            other.gameObject.GetComponent<SpriteRenderer>().sprite = GM.treasureSprite;
            GameObject.Find("InteractButton").GetComponent<Image>().color = new Color(1f,1f,1f,1f);
        }
        if(other.CompareTag("damage")){
            if(other.gameObject.transform.position.x > transform.position.x){
                rb.AddForce(Vector2.right * 3f);
                //rb.velocity = new Vector2(3f, rb.velocity.y);
                Debug.Log("damage from right");
            }else if(other.gameObject.transform.position.x <= transform.position.x){
                rb.AddForce(Vector2.right * -3f);
                //rb.velocity = new Vector2(-3f, rb.velocity.y);
                Debug.Log("damage from left");
            }
            gameObject.GetComponent<healthScript>().health--;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Treasure")){
            
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Treasure")){
            GM.available = false;
            GameObject.Find("InteractButton").GetComponent<Image>().color = new Color(1f,1f,1f,0.184f);
        }
        if(other.CompareTag("GateOpen")){
            GM.gateCheck = false;
            other.gameObject.GetComponent<SpriteRenderer>().sprite = GM.treasureSprite;
            GameObject.Find("InteractButton").GetComponent<Image>().color = new Color(1f,1f,1f,1f);
        }
    }
}
