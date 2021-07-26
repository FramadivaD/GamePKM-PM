using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GateFragmentOrder
{
    public List<int> order;

    public GateFragmentOrder(List<int> order)
    {
        this.order = new List<int>(order);
    }
}

public class Gate : MonoBehaviour
{
    [SerializeField] private PhotonView pv;

    Animator gateAnim;

    [SerializeField] private bool isOpened = false;
    public bool IsOpened { get { return isOpened; } private set { isOpened = value; } }

    [SerializeField] private TeamType _teamType;
    public TeamType teamType { get { return _teamType; } private set { _teamType = value; } }

    [SerializeField] private MainGateKey mainKey;
    public MainGateKey MainKey { get { return mainKey; } private set { mainKey = value; } }

    public List<int> CollectedFragmentIndex { get; private set; } = new List<int>();

    void Start()
    {
        gateAnim = GetComponent<Animator>();
    }

    public void Initialize(TeamType teamType)
    {
        if (PhotonNetwork.connected) {
            pv.RPC("InitializeRPC", PhotonTargets.AllBuffered, (int)teamType);
        } else
        {
            InitializeRPC((int)teamType);
        }
    }

    [PunRPC]
    private void InitializeRPC(int teamType)
    {
        this.teamType = (TeamType) teamType;
        this.MainKey = GameManager.PlayersTeam[this.teamType].FragmentsKey;
        IsOpened = false;
    }

    public void TryOpenGate(InventoryManager inventory)
    {
        Debug.Log("Try Store Fragment to Collected Gate");

        // Put Key Into CollectedFragment
        for (int i = 0;i < inventory.inventory.Length; i++)
        {
            if (inventory.inventory[i] != null)
            {
                if (inventory.inventory[i] is MainGateFragment)
                {
                    MainGateFragment fragment = inventory.inventory[i] as MainGateFragment;

                    Debug.Log("Check kalau kode key main sama.");

                    if (fragment.MainKey == mainKey)
                    {
                        CollectedFragmentIndex.Add((inventory.RemoveItem(fragment) as MainGateFragment).FragmentIndex);

                        Debug.Log("Fragment Stored : " + fragment.Key);
                        PlayCollectAnimation();
                    }
                }
            }
        }
    }

    public bool CheckGateShouldBeOpen()
    {
        if (!CheckGateIsReadyReordering())
        {
            return false;
        }

        for (int i = 0; i < mainKey.Fragments.Count; i++)
        {
            if (CollectedFragmentIndex[i] != mainKey.Fragments[i].FragmentIndex)
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckGateIsReadyReordering()
    {
        return CollectedFragmentIndex.Count == mainKey.Fragments.Count;
    }

    public void SetCollectedFragment(List<int> orderedFragments)
    {
        GateFragmentOrder newOrder = new GateFragmentOrder(orderedFragments);

        string orderJson = JsonUtility.ToJson(newOrder);
        if (PhotonNetwork.connected)
        {
            pv.RPC("SetCollectedFragmentRPC", PhotonTargets.AllBuffered, orderJson);
        } else
        {
            SetCollectedFragmentRPC(orderJson);
        }
    }

    [PunRPC]
    private void SetCollectedFragmentRPC(string gateFragmentOrderJson)
    {
        GateFragmentOrder order = JsonUtility.FromJson<GateFragmentOrder>(gateFragmentOrderJson);

        CollectedFragmentIndex = order.order;
    }

    public void OpenGate()
    {
        if (PhotonNetwork.connected) {
            pv.RPC("OpenGateRPC", PhotonTargets.AllBuffered);
        } else
        {
            OpenGateRPC();
        }
    }

    [PunRPC]
    private void OpenGateRPC()
    {
        Debug.Log("Opened Gate");
        IsOpened = true;
        gateAnim.SetBool("isAnswer", true);
    }

    private void PlayCollectAnimation()
    {
        
    }

    public bool CheckIsOpened()
    {
        return isOpened;
    }
}
