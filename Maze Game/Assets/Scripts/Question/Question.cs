using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public string question;
    public string answer;

    public Question()
    {
        bool[] questionType = {
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.pecahan,
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.pengurangan,
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.penjumlahan,
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.perkalian,
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.pembagian
        };

        // Random soal
        int randType = Random.Range(0, questionType.Length);

        // if the randomed value is false, then find the true
        for (int i = 0;i < questionType.Length;i++)
        {
            int ind = (randType + i) % questionType.Length;
            if (questionType[ind])
            {
                randType = ind;
                break;
            }
        }

        if (randType == 0) GenerateSoalPecahan();
        else if (randType == 1) GenerateSoalPengurangan();
        else if (randType == 2) GenerateSoalPenjumlahan();
        else if (randType == 3) GenerateSoalPerkalian();
        else if (randType == 4) GenerateSoalPembagian();
    }

    private void GenerateSoalPembagian()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        int val = a * b;

        question = "Berapa hasil dari perhitungan berikut ini :\n" + val + " / " + a + " = ?";
        answer = b.ToString();
    }

    private void GenerateSoalPerkalian()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        int val = a * b;

        question = "Berapa hasil dari perhitungan berikut ini :\n" + a + " x " + b + " = ?";
        answer = val.ToString();
    }

    private void GenerateSoalPenjumlahan()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        int val = a + b;

        question = "Berapa hasil dari perhitungan berikut ini :\n" + a + " + " + b + " = ?";
        answer = val.ToString();
    }

    private void GenerateSoalPengurangan()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        int val = a + b;

        question = "Berapa hasil dari perhitungan berikut ini :\n" + a + " - " + b + " = ?";
        answer = val.ToString();
    }

    private void GenerateSoalPecahan()
    {
        int a = Random.Range(1, 6);
        int b = Random.Range(1, 6);

        int a_ = Random.Range(1, 6);
        int b_ = Random.Range(1, 6);

        int pembilang = a * b_ + b * a_;
        int penyebut = a_ * b_;

        int gcd = GCD(pembilang, penyebut);

        pembilang /= gcd;
        penyebut /= gcd;

        question = "Berapa hasil dari perhitungan berikut ini :\n" + a + "/" + a_ + " " + "+" + " " + b + "/" + b_ + " = ??";
        answer = (penyebut == 1 ? pembilang.ToString() : (pembilang + "/" + penyebut));
    }

    private static int GCD(int a, int b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a | b;
    }

    /*
    float koef1, koef2, konst;
    int pembilang1, pembilang2, pembilang3, penyebut1, penyebut2, penyebut3;
    int FPB;
    public string question;
    char[] plusminus = {'+', '-'};
    int randplsmin;
    public string answer;
    public int tipeSoal = 1;

    public Question()
    {
        switch(tipeSoal){
            case 0:
                randplsmin = UnityEngine.Random.Range(0,2);
                do{
                    koef1 = UnityEngine.Random.Range(1, 11);
                    do{
                        koef2 = UnityEngine.Random.Range(1, 11);
                    }while(koef1 == koef2);
                    konst = UnityEngine.Random.Range(11, 21);
                    question = "Tentukan nilai x dari persamaan berikut ini :\n" + koef1 + "x " + plusminus[randplsmin] + " " +konst + " = " + koef2 + "x";
                }while(koef1 % 2 != 0 || koef2 % 2 != 0 || konst % 2 != 0);
                if(randplsmin == 0){
                    answer = Math.Round((-konst/(koef1 - koef2)), 2).ToString();
                }else answer = Math.Round((konst/(koef1 - koef2)), 2).ToString();
                break;
            case 1:
                randplsmin = UnityEngine.Random.Range(0,2);
                pembilang1 = UnityEngine.Random.Range(1, 11);
                pembilang2 = UnityEngine.Random.Range(1, 11);
                penyebut1 = UnityEngine.Random.Range(1, 11);
                penyebut2 = UnityEngine.Random.Range(1, 11);
                question = "Berapa hasil dari perhitungan berikut ini :\n" + pembilang1 + "/" + penyebut1 + " " + plusminus[randplsmin] + " " + pembilang2 + "/" + penyebut2 + " = ??";
                penyebut3 = penyebut1 * penyebut2;
                if(randplsmin == 0){
                    pembilang3 = (pembilang1 * penyebut2) + (pembilang2 * penyebut1);
                    FPB = perhitunganFPB(pembilang3, penyebut3);
                    penyebut3 /= FPB;
                    pembilang3 /= FPB;
                    answer = pembilang3.ToString() + "/" + penyebut3.ToString();
                }else {
                    pembilang3 = (pembilang1 * penyebut2) - (pembilang2 * penyebut1);
                    FPB = perhitunganFPB(pembilang3, penyebut3);
                    penyebut3 /= FPB;
                    pembilang3 /= FPB;
                    answer = pembilang3.ToString() + "/" + penyebut3.ToString();
                }
                break;
        }
        
    }

    private int perhitunganFPB(int a, int b){
        if(a < 0){
            a *= -1;
        }
        int i;
        if(a > b){
            i = b;
            while (i > 1){
                if(a % i == 0 && b % i == 0){
                    break;
                }
                i--;
            }
            return i;
        }else if(a < b){
            i = a;
            while (i > 1){
                if(a % i == 0 && b % i == 0){
                    break;
                }
                i--;
            }
            return i;
        }else return a;
    }

    void perhitunganKPK(float a, float b){

    }
    */
}
