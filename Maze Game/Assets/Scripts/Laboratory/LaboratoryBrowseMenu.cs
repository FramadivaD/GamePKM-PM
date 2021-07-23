using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class LaboratoryBrowseMenu : MonoBehaviour
{
    LaboratoryMenu laboratoryMenu;

    public GameObject browseButtonPrefab;
    public Transform browseButtonContainer;

    public GameObject noFilesText;

    public void Initialize(LaboratoryMenu laboratoryMenu)
    {
        this.laboratoryMenu = laboratoryMenu;
    }

    public void LoadDirectory()
    {
        foreach (Transform t in browseButtonContainer)
        {
            Destroy(t.gameObject);
        }

        AndroidHelper.CheckAndCreateDirectory(AndroidHelper.MainGateSavePath);

        string basePath = AndroidHelper.MainGateSavePath + "/Data";

        AndroidHelper.CheckAndCreateDirectory(basePath);

        string[] dir = Directory.GetFiles(basePath);

        if (dir.Length > 0)
        {
            for (int i = 0; i < dir.Length; i++)
            {
                GameObject ne = Instantiate(browseButtonPrefab, browseButtonContainer);
                Button button = ne.GetComponent<Button>();

                Debug.Log(dir[i]);

                if (button)
                {
                    //button.
                }
            }

            noFilesText.SetActive(true);
        }
        else
        {
            noFilesText.SetActive(true);
        }
    }

    private void OpenEditor()
    {
        laboratoryMenu.OpenEditorWindow();
    }
}
