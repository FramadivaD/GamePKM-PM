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
        pecahan = true,
        pengurangan = true,
        penjumlahan = true,
        perkalian = true,
        pembagian = true
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

        toggleBangunDatar.isOn = SelectedDifficulty.pecahan;
        togglePecahan.isOn = SelectedDifficulty.pengurangan;
        togglePenjumlahan.isOn = SelectedDifficulty.penjumlahan;
        togglePerkalian.isOn = SelectedDifficulty.perkalian;
        togglePersamaan.isOn = SelectedDifficulty.pembagian;
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
        SelectedDifficulty.pecahan = toggle;
    }

    public void TogglePecahan(bool toggle)
    {
        SelectedDifficulty.pengurangan = toggle;
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
        SelectedDifficulty.pembagian = toggle;
    }
}
