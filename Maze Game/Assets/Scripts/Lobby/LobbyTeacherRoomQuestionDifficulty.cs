using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyTeacherRoomQuestionDifficulty : MonoBehaviour
{
    [SerializeField] private GameObject difficultyMenu;

    [SerializeField] private Toggle togglePecahan;
    [SerializeField] private Toggle togglePengurangan;
    [SerializeField] private Toggle togglePenjumlahan;
    [SerializeField] private Toggle togglePerkalian;
    [SerializeField] private Toggle togglePembagian;

    [SerializeField] private Toggle togglePersamaanAljabar;
    [SerializeField] private Toggle toggleBarisanAritmatika;
    [SerializeField] private Toggle toggleBarisanGeometri;
    [SerializeField] private Toggle togglePenyederhanaanPecahan;

    public static QuestionDifficulty SelectedDifficulty = new QuestionDifficulty()
    {
        pecahan = true,
        pengurangan = true,
        penjumlahan = true,
        perkalian = true,
        pembagian = true,
        persamaanAljabar = true,
        barisanAritmatika = true,
        barisanGeometri = true,
        penyederhanaanPecahan = true
    };
    public static string SelectedDifficultyJson { get { return JsonUtility.ToJson(SelectedDifficulty); } }

    private void Awake()
    {
        CloseDifficultyMenu();
    }

    private void Start()
    {
        togglePecahan.onValueChanged.AddListener(TogglePecahan);
        togglePengurangan.onValueChanged.AddListener(TogglePengurangan);
        togglePenjumlahan.onValueChanged.AddListener(TogglePenjumlahan);
        togglePerkalian.onValueChanged.AddListener(TogglePerkalian);
        togglePembagian.onValueChanged.AddListener(TogglePembagian);
        togglePersamaanAljabar.onValueChanged.AddListener(TogglePersamaanAljabar);
        toggleBarisanAritmatika.onValueChanged.AddListener(ToggleBarisanAritmatika);
        toggleBarisanGeometri.onValueChanged.AddListener(ToggleBarisanGeometri);
        togglePenyederhanaanPecahan.onValueChanged.AddListener(TogglePenyederhanaanPecahan);


        togglePecahan.isOn = SelectedDifficulty.pecahan;
        togglePengurangan.isOn = SelectedDifficulty.pengurangan;
        togglePenjumlahan.isOn = SelectedDifficulty.penjumlahan;
        togglePerkalian.isOn = SelectedDifficulty.perkalian;
        togglePembagian.isOn = SelectedDifficulty.pembagian;
        togglePersamaanAljabar.isOn = SelectedDifficulty.persamaanAljabar;
        toggleBarisanAritmatika.isOn = SelectedDifficulty.barisanAritmatika;
        toggleBarisanGeometri.isOn = SelectedDifficulty.barisanGeometri;
        togglePenyederhanaanPecahan.isOn = SelectedDifficulty.penyederhanaanPecahan;
    }

    public void OpenDifficultyMenu()
    {
        difficultyMenu.SetActive(true);
    }

    public void CloseDifficultyMenu()
    {
        difficultyMenu.SetActive(false);
    }

    public void TogglePecahan(bool toggle)
    {
        SelectedDifficulty.pecahan = toggle;
    }

    public void TogglePengurangan(bool toggle)
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

    public void TogglePembagian(bool toggle)
    {
        SelectedDifficulty.pembagian = toggle;
    }

    public void TogglePersamaanAljabar(bool toggle)
    {
        SelectedDifficulty.persamaanAljabar = toggle;
    }

    public void ToggleBarisanAritmatika(bool toggle)
    {
        SelectedDifficulty.barisanAritmatika = toggle;
    }

    public void ToggleBarisanGeometri(bool toggle)
    {
        SelectedDifficulty.barisanGeometri = toggle;
    }

    public void TogglePenyederhanaanPecahan(bool toggle)
    {
        SelectedDifficulty.penyederhanaanPecahan = toggle;
    }
}
