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

    public bool CheckAllActive()
    {
        return pembagian && pengurangan && penjumlahan && perkalian && pecahan;
    }

    public bool CheckAllInactive()
    {
        return !pembagian && !pengurangan && !penjumlahan && !perkalian && !pecahan;
    }
}
