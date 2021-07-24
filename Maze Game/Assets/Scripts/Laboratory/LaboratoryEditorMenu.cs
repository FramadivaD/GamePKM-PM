using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaboratoryEditorMenu : MonoBehaviour
{
    LaboratoryMenu laboratoryMenu;

    public static WebCamTexture camTexture;

    private MainGateKeyRaw currentGateKey;
    private MainGateFragmentRaw currentGateFragment;

    [SerializeField] private GameObject fragmentButtonPrefab;
    [SerializeField] private Transform fragmentContainer;

    [SerializeField] private InputField mainGateKeyName;

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

    public void SaveMainGateKey()
    {
        Debug.Log("Save Main Gate Key");
    }

    public void AddFragment()
    {
        Debug.Log("Add Fragment");
    }
}
