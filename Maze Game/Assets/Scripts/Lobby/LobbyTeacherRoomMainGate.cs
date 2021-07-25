using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class LobbyTeacherRoomMainGate : MonoBehaviour
{
    [SerializeField] private GameObject browseMenu;

    [SerializeField] private Transform browseButtonContainer;
    [SerializeField] private GameObject browseButtonPrefab;

    [SerializeField] private GameObject noFilesText;

    [SerializeField] private Text mainGateKeyName;

    public MainGateKeyRaw CurrentMainGateKey { get; private set; } = null;

    public void OpenMainGateKeyBrowseMenu()
    {
        browseMenu.SetActive(true);

        LoadDirectory();
    }

    public void CloseMainGateKeyBrowseMenu()
    {
        browseMenu.SetActive(false);
    }

    private void LoadDirectory()
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

                        ne.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(info.Name);

                        if (button)
                        {
                            button.onClick.AddListener(() => { OpenMainGateKey(dir[x]); });
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

    private void OpenMainGateKey(string filename)
    {
        if (File.Exists(filename))
        {
            string fileBaseName = Path.GetFileNameWithoutExtension(new FileInfo(filename).Name);

            byte[] data = File.ReadAllBytes(filename);

            string content = System.Text.Encoding.ASCII.GetString(data);

            CurrentMainGateKey = JsonUtility.FromJson<MainGateKeyRaw>(content);

            mainGateKeyName.text = "Soal terpilih :\n" + fileBaseName;

            CloseMainGateKeyBrowseMenu();

            Debug.Log("Open Main Gate Key Success");
        }
        else
        {
            Debug.Log("File not exist");
        }
    }
}
