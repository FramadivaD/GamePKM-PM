using UnityEngine;
using System.Collections;

public class LaboratorySelectMenu : MonoBehaviour
{
    LaboratoryMenu laboratoryMenu;

    LaboratoryBrowseMenu browseMenu;
    LaboratoryEditorMenu editorMenu;

    public void Initialize(LaboratoryMenu laboratoryMenu)
    {
        this.laboratoryMenu = laboratoryMenu;

        browseMenu = laboratoryMenu.browseWindow;
        editorMenu = laboratoryMenu.editorWindow;
    }

    public void OpenCreateNewKey()
    {
        laboratoryMenu.OpenEditorWindow();
    }

    public void EditExistKey()
    {
        laboratoryMenu.OpenBrowseWindow();
        browseMenu.LoadDirectory();
    }
}
