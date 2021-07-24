using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

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

        mainGateKeyName.onEndEdit.AddListener(ChangeGateKeyName);
    }

    public void InitializeMainGateKey()
    {
        currentGateKey = new MainGateKeyRaw("");
    }

    public void OpenMainGateKey()
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
        if (currentGateKey != null)
        {
            currentGateKey.AddFragment(new MainGateFragmentRaw("1", "X"));
        }

        Debug.Log("Add Fragment");
    }

    public void BackToSelectMenu()
    {
        laboratoryMenu.OpenSelectWindow();
    }

    private void ChangeGateKeyName(string x)
    {
        if (currentGateKey != null)
        {
            currentGateKey.GateName = x;
        }
    }

    private void RefreshFragmentContainer()
    {
        foreach (Transform t in fragmentContainer)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < currentGateKey.Fragments.Count; i++)
        {
            int x = i;
            SpawnFragmentKey(currentGateKey.Fragments[x]);
        }
    }

    private void SpawnFragmentKey(MainGateFragmentRaw fragment)
    {
        GameObject ne = Instantiate(fragmentButtonPrefab, fragmentContainer);
        LaboratoryFragmentButton button = ne.GetComponent<LaboratoryFragmentButton>();

        if (button)
        {
            button.Initialize(currentGateKey, fragment);

            button.openFragmentButton.onClick.AddListener(() => { OpenFragmentKey(fragment); });

            button.deleteButton.onClick.AddListener(() => { RefreshFragmentContainer(); });
            button.orderUpButton.onClick.AddListener(() => { RefreshFragmentContainer(); });
            button.orderDownButton.onClick.AddListener(() => { RefreshFragmentContainer(); });
        }
    }

    private void OpenFragmentKey(MainGateFragmentRaw fragment)
    {
        Debug.Log("Open Fragment Key");
    }
}
