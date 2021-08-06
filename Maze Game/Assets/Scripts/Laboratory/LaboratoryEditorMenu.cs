using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

using SimpleFileBrowser;
using Extensione.Window;

public class LaboratoryEditorMenu : MonoBehaviour
{
    LaboratoryMenu laboratoryMenu;

    public GameObject mainEditor;
    public GameObject fragmentEditor;

    public static WebCamTexture camTexture;

    private MainGateKeyRaw currentGateKey;
    private MainGateFragmentRaw currentGateFragment;

    [Header("Main Editor")]
    [SerializeField] private GameObject fragmentButtonPrefab;
    [SerializeField] private Transform fragmentContainer;

    [SerializeField] private InputField mainGateKeyName;

    [Header("Fragment Editor")]
    [SerializeField] private InputField fragmentKeyName;
    [SerializeField] private InputField fragmentKeyData;
    [SerializeField] private Image fragmentImagePreview;
    [SerializeField] private RawImage fragmentImageCamera;

    [SerializeField] private Button cameraUpdateButton;
    [SerializeField] private Button cameraSaveButton;

    [SerializeField] private Button openGalleryButton;

    [SerializeField] private Button cameraSnapButton;
    [SerializeField] private Button cameraCancelButton;

    public void Initialize(LaboratoryMenu laboratoryMenu)
    {
        this.laboratoryMenu = laboratoryMenu;

        mainGateKeyName.onEndEdit.AddListener(ChangeGateKeyName);
    }

    public void InitializeMainGateKey()
    {
        currentGateKey = new MainGateKeyRaw("");
        currentGateFragment = null;

        mainGateKeyName.text = "";
        fragmentKeyName.text = "";
        fragmentKeyData.text = "";

        RefreshFragmentContainer();

        OpenMainEditor();
    }
    
    #region All About fragment data manipulation

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

    #endregion

    #region All about main gate data save and load

    public void OpenMainGateKey(string filename)
    {
        if (File.Exists(filename))
        {
            string fileBaseName = Path.GetFileNameWithoutExtension(new FileInfo(filename).Name);

            byte[] data = File.ReadAllBytes(filename);

            string content = System.Text.Encoding.ASCII.GetString(data);

            currentGateKey = JsonUtility.FromJson<MainGateKeyRaw>(content);

            mainGateKeyName.text = fileBaseName;

            RefreshFragmentContainer();

            OpenMainEditor();

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

            OpenMainEditor();

            Debug.Log("Save Main Gate Key");

            WindowMaster.Instance.Show("Gate Key Berhasil disimpan!");
        } else
        {
            Debug.Log("Please insert file name");

            WindowMaster.Instance.Show("Mohon masukkan nama file!");
        }
    }

    public void AddFragment()
    {
        if (currentGateKey != null)
        {
            MainGateFragmentRaw raw = new MainGateFragmentRaw("Key " + currentGateKey.Fragments.Count, "Answer");

            Texture2D ex = new Texture2D(1, 1);
            ex.SetPixels(new Color[] {new Color(255, 255, 255) });
            ex.Apply();

            raw.DataImage = ex.EncodeToJPG();
            raw.DataImageWidth = ex.width;
            raw.DataImageHeight = ex.height;

            currentGateKey.AddFragment(raw);
        }

        RefreshFragmentContainer();

        Debug.Log("Add Fragment");
    }

    #endregion

    public void BackToSelectMenu()
    {
        laboratoryMenu.OpenSelectWindow();

        OpenMainEditor();
    }

    public void SaveMainGateKeyAndBackToSelectMenu()
    {
        if (mainGateKeyName.text.Length > 0)
        {
            SaveMainGateKey();
            BackToSelectMenu();

            WindowMaster.Instance.Show("Gate Key Berhasil disimpan!");
        }
        else
        {
            Debug.Log("Please insert file name");

            WindowMaster.Instance.Show("Mohon masukkan nama file!");
        }
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
        OpenFragmentEditor();

        currentGateFragment = fragment;
        fragmentKeyName.text = fragment.Key;
        fragmentKeyData.text = fragment.Data;

        if (fragment.DataImage != null && fragment.DataImage.Length > 0)
        {
            Texture2D texture = new Texture2D(fragment.DataImageWidth, fragment.DataImageHeight);
            //texture.LoadRawTextureData(fragment.DataImage);
            texture.LoadImage(fragment.DataImage);
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            fragmentImagePreview.sprite = sprite;

            fragmentImageCamera.rectTransform.sizeDelta = new Vector2(500, texture.height * 500 / texture.width);
            fragmentImagePreview.rectTransform.sizeDelta = new Vector2(500, texture.height * 500 / texture.width);
        } else
        {
            fragmentImagePreview.sprite = null;
        }

        // Sprite imageData

        HideFragmentButtonCamera();

        Debug.Log("Open Fragment Key");
    }

    #region All About Fragment Editor

    // Take image file from gallery
    public void OpenGallery()
    {
        Debug.Log("Open gallery..");

        AndroidHelper.CheckAndCreateDirectory(AndroidHelper.GetAndroidExternalStoragePath());
        AndroidHelper.CheckAndCreateDirectory(AndroidHelper.MainGateSavePath);
        AndroidHelper.CheckAndCreateDirectory(AndroidHelper.MainGateSavePath + "/Data");

        string basepath = AndroidHelper.MainGateSavePath + "/Data";

        FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.SetFilters(false, ".jpg", ".jpeg", ".png");
        FileBrowser.ShowLoadDialog(
            (paths) => { OpenGalleryImage(paths[0]); }, 
            ()=> { }, 
            FileBrowser.PickMode.Files, 
            false, 
            basepath
        );
    }

    private void OpenGalleryImage(string filepath)
    {
        if (File.Exists(filepath))
        {
            byte[] imageData = File.ReadAllBytes(filepath);

            Texture2D source = new Texture2D(0, 0);
            source.LoadImage(imageData);
            source.Apply();

            // Resize
            int alteredX = source.width;
            int alteredY = source.height;

            source.filterMode = FilterMode.Point;

            RenderTexture rt = RenderTexture.GetTemporary(alteredX, alteredY);
            rt.filterMode = FilterMode.Point;

            RenderTexture.active = rt;
            Graphics.Blit(source, rt);

            Texture2D ntex = new Texture2D(alteredX, alteredY);
            ntex.ReadPixels(new Rect(0, 0, alteredX, alteredY), 0, 0);
            ntex.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            Sprite sprite = Sprite.Create(ntex, new Rect(0, 0, alteredX, alteredY), new Vector2(0.5f, 0.5f));

            fragmentImagePreview.sprite = sprite;

            // currentGateFragment.DataImage = ntex.GetRawTextureData();
            currentGateFragment.DataImage = ntex.EncodeToJPG();
            currentGateFragment.DataImageWidth = ntex.width;
            currentGateFragment.DataImageHeight = ntex.height;

            fragmentImageCamera.rectTransform.sizeDelta = new Vector2(500, alteredY * 500 / alteredX);
            fragmentImagePreview.rectTransform.sizeDelta = new Vector2(500, alteredY * 500 / alteredX);
        }

        HideFragmentButtonCamera();
    }

    // Update Button
    public void OpenCamera()
    {
        if (!camTexture)
        {
            camTexture = new WebCamTexture(256, 256);
        }

        fragmentImageCamera.texture = camTexture;
        fragmentImageCamera.material.mainTexture = camTexture;

        fragmentImageCamera.rectTransform.sizeDelta = new Vector2(500, camTexture.height * 500 / camTexture.width);
        fragmentImagePreview.rectTransform.sizeDelta = new Vector2(500, camTexture.height * 500 / camTexture.width);

        camTexture.Play();

        ShowFragmentButtonCamera();
    }

    // Snap Button
    public void SnapCamera()
    {
        if (camTexture)
        {
            Color[] colors = camTexture.GetPixels();
            Texture2D source = new Texture2D(camTexture.width, camTexture.height);
            source.SetPixels(colors);
            source.Apply();

            // Resize
            int alteredX = camTexture.width;
            int alteredY = camTexture.height;

            source.filterMode = FilterMode.Point;

            RenderTexture rt = RenderTexture.GetTemporary(alteredX, alteredY);
            rt.filterMode = FilterMode.Point;

            RenderTexture.active = rt;
            Graphics.Blit(source, rt);

            Texture2D ntex = new Texture2D(alteredX, alteredY);
            ntex.ReadPixels(new Rect(0, 0, alteredX, alteredY), 0, 0);
            ntex.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            Sprite sprite = Sprite.Create(ntex, new Rect(0, 0, alteredX, alteredY), new Vector2(0.5f, 0.5f));

            fragmentImagePreview.sprite = sprite;

            // currentGateFragment.DataImage = ntex.GetRawTextureData();
            currentGateFragment.DataImage = ntex.EncodeToJPG();
            currentGateFragment.DataImageWidth = ntex.width;
            currentGateFragment.DataImageHeight = ntex.height;

            fragmentImageCamera.rectTransform.sizeDelta = new Vector2(500, alteredY * 500 / alteredX);
            fragmentImagePreview.rectTransform.sizeDelta = new Vector2(500, alteredY * 500 / alteredX);

            camTexture.Stop();
        }

        HideFragmentButtonCamera();
    }

    public void CancelCamera()
    {
        if (camTexture)
        {
            camTexture.Stop();
        }

        HideFragmentButtonCamera();
    }

    public void SaveFragment()
    {
        if (fragmentKeyName.text.Length <= 0)
        {
            WindowMaster.Instance.Show("Nama Fragment harus diisi!");
            return;
        }

        // check fragment key is exist
        for (int i = 0;i < currentGateKey.Fragments.Count;i++)
        {
            if (currentGateKey.Fragments[i].Key == fragmentKeyName.text && currentGateKey.Fragments[i] != currentGateFragment)
            {
                WindowMaster.Instance.Show("Nama Fragment harus berbeda dengan Fragment yang lain!");
                return;
            }
        }

        // if not exist then allow to save

        currentGateFragment.Key = fragmentKeyName.text;
        currentGateFragment.Data = fragmentKeyData.text;

        HideFragmentButtonCamera();
        OpenMainEditor();

        RefreshFragmentContainer();
    }

    private void CloseFragmentKey()
    {
        currentGateFragment = null;

        OpenMainEditor();
    }

    #endregion

    #region All about window

    private void OpenMainEditor()
    {
        mainEditor.SetActive(true);
        fragmentEditor.SetActive(false);
    }

    private void OpenFragmentEditor()
    {
        mainEditor.SetActive(false);
        fragmentEditor.SetActive(true);
    }

    private void ShowFragmentButtonCamera()
    {
        cameraUpdateButton.gameObject.SetActive(false);
        cameraSaveButton.gameObject.SetActive(false);

        cameraSnapButton.gameObject.SetActive(true);
        cameraCancelButton.gameObject.SetActive(true);

        openGalleryButton.gameObject.SetActive(false);

        fragmentImagePreview.gameObject.SetActive(false);
        fragmentImageCamera.gameObject.SetActive(true);
    }

    private void HideFragmentButtonCamera()
    {
        cameraUpdateButton.gameObject.SetActive(true);
        cameraSaveButton.gameObject.SetActive(true);

        cameraSnapButton.gameObject.SetActive(false);
        cameraCancelButton.gameObject.SetActive(false);

        openGalleryButton.gameObject.SetActive(true);

        fragmentImagePreview.gameObject.SetActive(true);
        fragmentImageCamera.gameObject.SetActive(false);
    }

    #endregion
}
