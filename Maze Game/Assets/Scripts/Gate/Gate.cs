using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator gateAnim;
    private bool open = false;

    [SerializeField] private TeamType _teamType;
    public TeamType teamType { get { return _teamType; } private set { _teamType = value; } }
    
    void Start()
    {
        gateAnim = GetComponent<Animator>();
    }

    public void Initialize(TeamType teamType)
    {
        this.teamType = teamType;
    }

    public void OpenGate()
    {
        open = true;
        gateAnim.SetBool("isAnswer", true);
    }
}
