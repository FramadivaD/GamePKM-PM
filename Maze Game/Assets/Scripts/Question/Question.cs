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
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.pembagian,
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.persamaanAljabar,
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.barisanAritmatika,
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.barisanGeometri,
            LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.penyederhanaanPecahan
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
        else if (randType == 5) GenerateSoalPersamaanAljabar();
        else if (randType == 6) GenerateSoalBarisanAritmatika();
        else if (randType == 7) GenerateSoalBarisanGeometri();
        else if (randType == 8) GenerateSoalPenyederhanaanPecahan();
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
        int val = a - b;

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

    private void GenerateSoalPersamaanAljabar()
    {
        int a = Random.Range(1, 6);
        int b = Random.Range(1, 10);

        int x = Random.Range(0, 10);

        // ax + b = c
        int c = a * x + b;

        question = "Berapakah nilai x dari persamaan berikut :\n" + a + x + " + " + b + " = " + c;
        answer = x.ToString();
    }

    private void GenerateSoalBarisanAritmatika()
    {
        int a = Random.Range(1, 20);
        int b = Random.Range(1, 5);

        question = "Berapakah lanjutan dari barisan aritmatika berikut :\n";

        int proc = a;

        for (int i = 0; i < 4; i++)
        {
            question += proc + ", ";
            proc += b;
        }

        question += "...";
        answer = proc.ToString();
    }

    private void GenerateSoalBarisanGeometri()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 5);

        question = "Berapakah lanjutan dari barisan geometri berikut :\n";

        int proc = a;

        for (int i = 0; i < 4; i++)
        {
            question += proc + ", ";
            proc *= b;
        }

        question += "...";
        answer = proc.ToString();
    }

    private void GenerateSoalPenyederhanaanPecahan()
    {
        int rand = Mathf.RoundToInt(Random.value * 100);

        int a = rand;
        int b = 100;

        int pembilang = rand;
        int penyebut = 100;

        int gcd = GCD(pembilang, penyebut);

        pembilang /= gcd;
        penyebut /= gcd;

        question = "Berapakah pecahan paling sederhana dari bilangan berikut :\n" + a + "/" + b;
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
}
