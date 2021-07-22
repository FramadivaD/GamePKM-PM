using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MainGateKey
{
    public TeamType team;
    public List<MainGateFragment> fragments;
}

[System.Serializable]
public class MainGateFragment
{
    [SerializeField] private string key;
    [SerializeField] private string data;

    public MainGateKey MainKey { get; }
    public TeamType Team { get { return MainKey.team; } }

    public string Key { get { return key; } }
    public string Data { get { return data; } }

    public MainGateFragment(MainGateKey mainKey, string key, string data)
    {
        MainKey = mainKey;
        this.key = key;
        this.data = data;
    }
}