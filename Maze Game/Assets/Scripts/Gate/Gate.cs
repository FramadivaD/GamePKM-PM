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

    [SerializeField] private MainGateKey mainKey;
    public MainGateKey MainKey { get { return mainKey; } private set { mainKey = value; } }

    public List<MainGateFragment> CollectedFragment { get; private set; } = new List<MainGateFragment>();

    void Start()
    {
        gateAnim = GetComponent<Animator>();
    }

    public void Initialize(TeamType teamType, MainGateKey mainKey)
    {
        this.teamType = teamType;
        this.MainKey = mainKey;
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
                        CollectedFragment.Add(inventory.RemoveItem(fragment) as MainGateFragment);

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
            if (CollectedFragment[i] != mainKey.Fragments[i])
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckGateIsReadyReordering()
    {
        return CollectedFragment.Count == mainKey.Fragments.Count;
    }

    public void SetCollectedFragment(List<MainGateFragment> orderedFragments)
    {
        CollectedFragment = new List<MainGateFragment>(orderedFragments);
    }

    public void OpenGate()
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
