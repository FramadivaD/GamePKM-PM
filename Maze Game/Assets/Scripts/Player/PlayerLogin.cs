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

    public void SubmitName()
    {
        if (inputName.text != "" && inputName.text.Length >= 5)
        {
            loginUI.SetActive(false);

            OnSubmitNameSuccess?.Invoke(inputName.text);
        } else
        {
            OnSubmitNameFailed?.Invoke(inputName.text);
            Debug.Log("Nama setidaknya harus 5 karakter!");
        }
    }
}
