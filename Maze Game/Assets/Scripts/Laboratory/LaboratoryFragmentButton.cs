using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaboratoryFragmentButton : MonoBehaviour
{
    public MainGateKeyRaw mainKey;
    public MainGateFragmentRaw fragment;

    public Button deleteButton;
    public Button orderUpButton;
    public Button orderDownButton;

    public Button openFragmentButton;

    public void Initialize(MainGateKeyRaw mainKey, MainGateFragmentRaw fragment)
    {
        this.mainKey = mainKey;
        this.fragment = fragment;
    }

    public void DeleteFragment()
    {
        mainKey.RemoveFragment(fragment);
    }

    public void OrderUpFragment()
    {
        mainKey.OrderUpFragment(fragment);
    }

    public void OrderDownFragment()
    {
        mainKey.OrderDownFragment(fragment);
    }
}
