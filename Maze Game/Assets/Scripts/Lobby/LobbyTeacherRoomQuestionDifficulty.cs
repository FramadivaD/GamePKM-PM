using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyTeacherRoomQuestionDifficulty : MonoBehaviour
{
    [SerializeField] private GameObject difficultyMenu;

    [SerializeField] private Toggle toggleBangunDatar;
    [SerializeField] private Toggle togglePecahan;
    [SerializeField] private Toggle togglePenjumlahan;
    [SerializeField] private Toggle togglePerkalian;
    [SerializeField] private Toggle togglePersamaan;

    public static QuestionDifficulty SelectedDifficulty = new QuestionDifficulty()
    {
        bangunDatar = true,
        pecahan = true,
        penjumlahan = true,
        perkalian = true,
        persamaan = true
    };
    public static string SelectedDifficultyJson { get { return JsonUtility.ToJson(SelectedDifficulty); } }

    private void Awake()
    {
        CloseDifficultyMenu();
    }

    private void Start()
    {
        toggleBangunDatar.onValueChanged.AddListener(ToggleBangunDatar);
        togglePecahan.onValueChanged.AddListener(TogglePecahan);
        togglePenjumlahan.onValueChanged.AddListener(TogglePenjumlahan);
        togglePerkalian.onValueChanged.AddListener(TogglePerkalian);
        togglePersamaan.onValueChanged.AddListener(TogglePersamaan);

        toggleBangunDatar.isOn = SelectedDifficulty.bangunDatar;
        togglePecahan.isOn = SelectedDifficulty.pecahan;
        togglePenjumlahan.isOn = SelectedDifficulty.penjumlahan;
        togglePerkalian.isOn = SelectedDifficulty.perkalian;
        togglePersamaan.isOn = SelectedDifficulty.persamaan;
    }

    public void OpenDifficultyMenu()
    {
        difficultyMenu.SetActive(true);
    }

    public void CloseDifficultyMenu()
    {
        difficultyMenu.SetActive(false);
    }

    public void ToggleBangunDatar(bool toggle)
    {
        SelectedDifficulty.bangunDatar = toggle;
    }

    public void TogglePecahan(bool toggle)
    {
        SelectedDifficulty.pecahan = toggle;
    }

    public void TogglePenjumlahan(bool toggle)
    {
        SelectedDifficulty.penjumlahan = toggle;
    }

    public void TogglePerkalian(bool toggle)
    {
        SelectedDifficulty.perkalian = toggle;
    }

    public void TogglePersamaan(bool toggle)
    {
        SelectedDifficulty.persamaan = toggle;
    }
}
