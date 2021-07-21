using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestionManager : MonoBehaviour
{
    private Player player;

    public ChestContainer SelectedChest { get; private set; }

    public GameObject questionContainer;

    public Text questionTitleText;
    public InputField questionAnswerText;

    private void Start()
    {
        CloseQuestion();
    }

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public void OpenQuestion(ChestContainer chest)
    {
        if (chest.teamType == player.teamType) {
            if (chest.question == null)
            {
                chest.Initialize();
            }
            questionContainer.SetActive(true);
            SelectedChest = chest;
            RefreshQuestionUI();

            Debug.Log("Answer is " + SelectedChest.question.answer);
        }
    }

    public void RefreshQuestionUI()
    {
        if (SelectedChest && SelectedChest.question != null)
        {
            questionTitleText.text = SelectedChest.question.question;
            questionAnswerText.text = "";
        } else
        {
            questionTitleText.text = "No Question ?";
            questionAnswerText.text = "";
        }
    }

    public void CloseQuestion()
    {
        SelectedChest = null;
        questionContainer.SetActive(false);
    }

    public void TryAnswer()
    {
        if (SelectedChest.CheckAnswer(questionAnswerText.text))
        {
            OnAnswerTrue();
        } else
        {
            OnAnswerFalse();
        }
    }

    private void OnAnswerTrue()
    {
        SelectedChest.UnlockChest();
        RefreshQuestionUI();
        CloseQuestion();
    }

    private void OnAnswerFalse()
    {
        RefreshQuestionUI();
    }
}
