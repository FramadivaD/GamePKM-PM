using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator gateAnim;
    public bool open = false;
    
    void Start()
    {
        gateAnim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if(open){
            gateAnim.SetBool("isAnswer", true);
        }
    }
}
