using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuestionDifficulty
{
    public bool pembagian;
    public bool pengurangan;
    public bool penjumlahan;
    public bool perkalian;
    public bool pecahan;

    // New Harder

    public bool persamaanAljabar;
    public bool barisanAritmatika;
    public bool barisanGeometri;
    public bool penyederhanaanPecahan;

    public bool CheckAllActive()
    {
        return pembagian && pengurangan && penjumlahan && perkalian && pecahan && persamaanAljabar && barisanAritmatika && barisanGeometri && penyederhanaanPecahan;
    }

    public bool CheckAllInactive()
    {
        return !pembagian && !pengurangan && !penjumlahan && !perkalian && !pecahan && !persamaanAljabar && !barisanAritmatika && !barisanGeometri && !penyederhanaanPecahan;
    }
}
