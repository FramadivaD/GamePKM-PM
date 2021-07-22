using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator gateAnim;

    [SerializeField] private bool isOpened = false;
    public bool IsOpened { get { return isOpened; } private set { isOpened = false; } }

    [SerializeField] private TeamType _teamType;
    public TeamType teamType { get { return _teamType; } private set { _teamType = value; } }
    
    void Start()
    {
        gateAnim = GetComponent<Animator>();
    }

    public void Initialize(TeamType teamType)
    {
        this.teamType = teamType;
        IsOpened = false;
    }

    public void OpenGate()
    {
        Debug.Log("Opened Gate");
        IsOpened = true;
        gateAnim.SetBool("isAnswer", true);
    }
}
