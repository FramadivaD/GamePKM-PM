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

    public void SubmitName()
    {
        if (inputName.text != "" && inputName.text.Length >= 5)
        {
            player.PlayerName = inputName.text;
            player.AllowMove = true;

            loginUI.SetActive(false);
        } else
        {
            Debug.Log("Nama setidaknya harus 5 karakter!");
        }
    }
}
