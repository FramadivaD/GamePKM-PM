using UnityEngine;
using System.Collections.Generic;

public class MainGateKey
{
    public TeamType Team { get; private set; }
    public List<MainGateFragment> Fragments { get; private set; }

    public MainGateKey(TeamType teamType)
    {
        Fragments = new List<MainGateFragment>();
        Team = teamType;
    }

    public void AddFragment(MainGateFragment fragment)
    {
        Fragments.Add(fragment);
    }
}

public class MainGateFragment : IInventoryAble
{
    [SerializeField] private MainGateFragment mainKey;
    [SerializeField] private string key;
    [SerializeField] private string data;

    public MainGateKey MainKey { get; }
    public TeamType Team { get { return MainKey.Team; } }

    public string Key { get { return key; } }
    public string Data { get { return data; } }

    public MainGateFragment(MainGateKey mainKey, string key, string data)
    {
        MainKey = mainKey;
        this.key = key;
        this.data = data;
    }
}