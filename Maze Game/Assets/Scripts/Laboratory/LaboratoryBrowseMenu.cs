using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class LaboratoryBrowseMenu : MonoBehaviour
{
    LaboratoryMenu laboratoryMenu;
    public LaboratoryEditorMenu laboratoryEditor;

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
                int x = i;

                Debug.Log(dir[x]);

                if (File.Exists(dir[x]))
                {
                    FileInfo info = new FileInfo(dir[x]);
                    if (info.Extension == ".soal")
                    {

                        GameObject ne = Instantiate(browseButtonPrefab, browseButtonContainer);
                        Button button = ne.GetComponent<Button>();

                        if (button)
                        {
                            button.onClick.AddListener(() => { OpenEditor(dir[x]); });
                        }
                    }
                }
            }

            noFilesText.SetActive(false);
        }
        else
        {
            noFilesText.SetActive(true);
        }
    }

    private void OpenEditor(string filename)
    {
        laboratoryMenu.OpenEditorWindow();
        laboratoryEditor.OpenMainGateKey(filename);
    }
}
