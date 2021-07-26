using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerGameManagerReference : MonoBehaviour
{
    [SerializeField] private Button playerPausePauseButton;
    [SerializeField] private Button playerPauseContinueButton;

    [SerializeField] private Button playerLeaveGameButton;

    private Player player;
    private GameManager gameManager;

    private bool initialized = false;

    public void Initialize(Player player)
    {
        if (!initialized)
        {
            this.player = player;

            gameManager = GameManager.Instance;

            playerPausePauseButton.onClick.AddListener(gameManager.ShowPauseMenu);
            playerPauseContinueButton.onClick.AddListener(gameManager.HidePauseMenu);
            playerLeaveGameButton.onClick.AddListener(gameManager.BackToMainMenu);

            initialized = true;
        }
    }
}
