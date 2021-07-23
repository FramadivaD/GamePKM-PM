using UnityEngine;
using System.Collections;

public class LaboratoryMenu : MonoBehaviour
{
    public LaboratorySelectMenu selectWindow;
    public LaboratoryBrowseMenu browseWindow;
    public LaboratoryEditorMenu editorWindow;

    private void Start()
    {
        selectWindow.Initialize(this);
        browseWindow.Initialize(this);
        editorWindow.Initialize(this);

        OpenSelectWindow();
    }

    #region All About Navigation

    public void OpenSelectWindow()
    {
        selectWindow.gameObject.SetActive(true);
        browseWindow.gameObject.SetActive(false);
        editorWindow.gameObject.SetActive(false);
    }

    public void OpenBrowseWindow()
    {
        selectWindow.gameObject.SetActive(false);
        browseWindow.gameObject.SetActive(true);
        editorWindow.gameObject.SetActive(false);
    }

    public void OpenEditorWindow()
    {
        selectWindow.gameObject.SetActive(false);
        browseWindow.gameObject.SetActive(false);
        editorWindow.gameObject.SetActive(true);
    }

    #endregion
}
