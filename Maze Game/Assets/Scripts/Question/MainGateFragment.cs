using UnityEngine;
using System.Collections.Generic;

#region Gameplay Data

[System.Serializable]
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

    public static MainGateKey ConvertFromRawData(TeamType team, MainGateKeyRaw mainGateRaw)
    {
        MainGateKey mainGate = new MainGateKey(team);
        int fragmentIndex = 0;
        foreach (MainGateFragmentRaw fragmentRaw in mainGateRaw.Fragments)
        {
            MainGateFragment fragment = new MainGateFragment(mainGate, fragmentIndex, fragmentRaw.Key, fragmentRaw.Data);

            // Read image data by convert from bytes to Sprite
            if (fragmentRaw.DataImage != null && fragmentRaw.DataImage.Length > 0)
            {
                Texture2D texture = new Texture2D(fragmentRaw.DataImageWidth, fragmentRaw.DataImageHeight);
                texture.LoadRawTextureData(fragmentRaw.DataImage);
                texture.Apply();

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                fragment.SetDataImage(sprite);
            } else
            {
                fragment.SetDataImage(null);
            }

            mainGate.AddFragment(fragment);
            fragmentIndex++;
        }

        return mainGate;
    }
}

[System.Serializable]
public class MainGateFragment : IInventoryAble
{
    [SerializeField] private MainGateFragment mainKey { get; set; }
    [SerializeField] private string key;
    [SerializeField] private string data;
    [SerializeField] private Sprite dataImage;

    public MainGateKey MainKey { get; }
    public TeamType Team { get { return MainKey.Team; } }

    public string Key { get { return key; } }
    public string Data { get { return data; } }

    public Sprite DataImage { get { return dataImage; } }

    public int FragmentIndex { get; }

    public MainGateFragment(MainGateKey mainKey, int fragmentIndex, string key, string data)
    {
        MainKey = mainKey;
        FragmentIndex = fragmentIndex;

        this.key = key;
        this.data = data;
    }

    public void SetDataImage(Sprite sprite)
    {
        dataImage = sprite;
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

    public byte[] DataImage;
    public int DataImageWidth;
    public int DataImageHeight;

    public MainGateFragmentRaw(string key, string data)
    {
        Key = key;
        Data = data;
    }
}

#endregion