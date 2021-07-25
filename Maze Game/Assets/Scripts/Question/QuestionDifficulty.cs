using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuestionDifficulty
{
    public bool persamaan;
    public bool pecahan;
    public bool penjumlahan;
    public bool perkalian;
    public bool bangunDatar;

    public bool CheckAllActive()
    {
        return persamaan && pecahan && penjumlahan && perkalian && bangunDatar;
    }

    public bool CheckAllInactive()
    {
        return !persamaan && !pecahan && !penjumlahan && !perkalian && !bangunDatar;
    }
}
