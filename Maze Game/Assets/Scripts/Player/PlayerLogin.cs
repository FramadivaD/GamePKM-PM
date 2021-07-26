using UnityEngine;
using UnityEngine.UI;

public class PlayerLogin : MonoBehaviour
{
    private Player player;

    public GameObject loginUI;
    public InputField inputName;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public delegate void SubmitEventHandler(string playerName);
    public event SubmitEventHandler OnSubmitNameSuccess;
    public event SubmitEventHandler OnSubmitNameFailed;

    public void ChangeName(string str)
    {
        OnSubmitNameSuccess?.Invoke(str);
    }

    public void SubmitName()
    {
        string name = inputName.text.Trim();

        loginUI.SetActive(false);

        OnSubmitNameSuccess?.Invoke(name);
    }

    [System.Obsolete]
    public void SubmitNameLegacy()
    {
        string name = inputName.text.Trim();
        if (name != "" && name.Length >= 3)
        {
            loginUI.SetActive(false);

            OnSubmitNameSuccess?.Invoke(inputName.text);
        }
        else
        {
            OnSubmitNameFailed?.Invoke(inputName.text);
            Debug.Log("Nama setidaknya harus 3 karakter!");
        }
    }
}
