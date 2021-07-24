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

    public void OpenMainGateKey(string filename)
    {
        if (File.Exists(filename))
        {
            string fileBaseName = new FileInfo(filename).Name;

            byte[] data = File.ReadAllBytes(filename);

            string content = System.Text.Encoding.ASCII.GetString(data);

            currentGateKey = JsonUtility.FromJson<MainGateKeyRaw>(content);

            mainGateKeyName.text = fileBaseName;

            RefreshFragmentContainer();

            Debug.Log("Open Main Gate Key Success");
        } else
        {
            Debug.Log("File not exist");
        }
    }

    public void SaveMainGateKey()
    {
        if (mainGateKeyName.text.Length > 0) {
            string content = JsonUtility.ToJson(currentGateKey);

            // Create directory
            AndroidHelper.CheckAndCreateDirectory(AndroidHelper.MainGateSavePath);
            string basePath = AndroidHelper.MainGateSavePath + "/Data";
            AndroidHelper.CheckAndCreateDirectory(basePath);

            string filename = basePath + "/" + mainGateKeyName.text + ".soal";

            byte[] data = System.Text.Encoding.ASCII.GetBytes(content);

            File.WriteAllBytes(filename, data);

            Debug.Log("Save Main Gate Key");
        } else
        {
            Debug.Log("Please insert file name");
        }
    }

    public void AddFragment()
    {
        if (currentGateKey != null)
        {
            currentGateKey.AddFragment(new MainGateFragmentRaw("Key " + currentGateKey.Fragments.Count, "Answer"));
        }

        RefreshFragmentContainer();

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
