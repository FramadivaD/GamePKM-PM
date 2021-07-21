using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator gateAnim;
    private bool open = false;

    public TeamType teamType;
    
    void Start()
    {
        gateAnim = GetComponent<Animator>();
    }

    public void OpenGate()
    {
        open = true;
        gateAnim.SetBool("isAnswer", true);
    }
}
