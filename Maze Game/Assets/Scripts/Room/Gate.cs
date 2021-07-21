using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator gateAnim;
    public bool open = false;
    //float timer = 1.5f;
    
    void Start()
    {
        gateAnim = GetComponent<Animator>();
    }

    
    void Update()
    {
        if(open){
            gateAnim.SetBool("isAnswer", true);
        }
        /*if(gateAnim.GetBool("isAnswer") == true){
            if(timer <= 0){
                gateAnim.SetBool("isAnswer", false);
                timer = 1.5f;
            }
            else if(timer > 0){
                timer -= Time.deltaTime;
            }
        }*/
    }
}
