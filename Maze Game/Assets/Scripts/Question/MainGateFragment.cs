using UnityEngine;
using System.Collections.Generic;

#region Gameplay Data

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

    public Sprite DataImage { get { return null; } }

    public MainGateFragment(MainGateKey mainKey, string key, string data)
    {
        MainKey = mainKey;
        this.key = key;
        this.data = data;
    }
}

#endregion

#region Raw Laboratory Data

[System.Serializable]
public class MainGateKeyRaw
{
    public string GateName;
    public List<MainGateFragmentRaw> Fragments;

    public MainGateKeyRaw(string gateName)
    {
        GateName = gateName;
        Fragments = new List<MainGateFragmentRaw>();
    }

    public void AddFragment(MainGateFragmentRaw fragment)
    {
        Fragments.Add(fragment);
    }

    public void RemoveFragment(MainGateFragmentRaw fragment)
    {
        Fragments.Remove(fragment);
    }

    public void OrderUpFragment(MainGateFragmentRaw fragment)
    {
        for (int i = 0;i < Fragments.Count; i++)
        {
            if (i > 0)
            {
                if (Fragments[i] == fragment)
                {
                    MainGateFragmentRaw temp = Fragments[i];
                    Fragments[i] = Fragments[i - 1];
                    Fragments[i - 1] = temp;

                    return;
                }
            }
        }
    }

    public void OrderDownFragment(MainGateFragmentRaw fragment)
    {
        for (int i = 0; i < Fragments.Count; i++)
        {
            if (i < Fragments.Count - 1)
            {
                if (Fragments[i] == fragment)
                {
                    MainGateFragmentRaw temp = Fragments[i];
                    Fragments[i] = Fragments[i + 1];
                    Fragments[i + 1] = temp;

                    return;
                }
            }
        }
    }
}

[System.Serializable]
public class MainGateFragmentRaw
{
    public string Key;
    public string Data; //filename

    public Sprite DataImage { get { return null; } }

    public MainGateFragmentRaw(string key, string data)
    {
        Key = key;
        Data = data;
    }
}

#endregion