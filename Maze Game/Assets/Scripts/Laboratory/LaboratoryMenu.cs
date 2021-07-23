using UnityEngine;
using System.Collections;

public class LaboratoryMenu : MonoBehaviour
{
    public static WebCamTexture camTexture;

    private MainGateKeyRaw mainGateRawData;

    public void InitializeMainGateKey()
    {
        mainGateRawData = new MainGateKeyRaw();
    }

    public void CreateFragmentData()
    {
        if (mainGateRawData != null)
        {
            MainGateFragmentRaw fragment = new MainGateFragmentRaw("", "");
        }
    }

    public void AddFragmentData(MainGateFragmentRaw fragment)
    {
        if (mainGateRawData != null)
        {
            mainGateRawData.AddFragment(fragment);
        }
    }

    public void RemoveFragmentData(MainGateFragmentRaw fragment)
    {
        if (mainGateRawData != null)
        {
            mainGateRawData.RemoveFragment(fragment);
        }
    }
}
