using UnityEngine;
using System.Collections;

public class LaboratoryMenu : MonoBehaviour
{
    public static WebCamTexture camTexture;

    private MainGateKeyRaw currentGateKey;
    private MainGateFragmentRaw currentGateFragment;

    public GameObject selectWindow;
    public GameObject browseWindow;
    public GameObject editorWindow;

    private void Start()
    {
        OpenSelectWindow();
    }

    public void InitializeMainGateKey()
    {
        currentGateKey = new MainGateKeyRaw();
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

    #region All About Navigation

    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
        browseWindow.SetActive(false);
        editorWindow.SetActive(false);
    }

    public void OpenBrowseWindow()
    {
        selectWindow.SetActive(false);
        browseWindow.SetActive(true);
        editorWindow.SetActive(false);
    }

    public void OpenEditorWindow()
    {
        selectWindow.SetActive(false);
        browseWindow.SetActive(false);
        editorWindow.SetActive(true);
    }

    #endregion
}
