﻿using UnityEngine;
using System.Collections;

public class LaboratoryEditorMenu : MonoBehaviour
{
    LaboratoryMenu laboratoryMenu;

    public static WebCamTexture camTexture;

    private MainGateKeyRaw currentGateKey;
    private MainGateFragmentRaw currentGateFragment;

    public void Initialize(LaboratoryMenu laboratoryMenu)
    {
        this.laboratoryMenu = laboratoryMenu;
    }

    public void InitializeMainGateKey()
    {
        currentGateKey = new MainGateKeyRaw("");
    }

    public void CreateFragmentData()
    {
        if (currentGateKey != null)
        {
            MainGateFragmentRaw fragment = new MainGateFragmentRaw("", "");
        }
    }

    public void AddFragmentData(MainGateFragmentRaw fragment)
    {
        if (currentGateKey != null)
        {
            currentGateKey.AddFragment(fragment);
        }
    }

    public void RemoveFragmentData(MainGateFragmentRaw fragment)
    {
        if (currentGateKey != null)
        {
            currentGateKey.RemoveFragment(fragment);
        }
    }
}
