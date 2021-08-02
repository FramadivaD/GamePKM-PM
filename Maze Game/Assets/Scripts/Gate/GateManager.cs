using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GateManager : MonoBehaviour
{
    private Player player;

    private Gate selectedGate;

    public GameObject questionContainer;
    public Image questionImage;

    public RectTransform answerContainerParent;
    public GameObject answerImagePrefab;
    public GameObject answerNotifWarning;

    public Button nextButton;

    private List<int> fragmentProgress;

    private float questionImageTargetWidth;
    private float questionImageTargetHeight;

    public void Initialize(Player player)
    {
        this.player = player;

        nextButton.interactable = false;
        answerNotifWarning.SetActive(true);

        questionImageTargetWidth = questionImage.rectTransform.sizeDelta.x;
        questionImageTargetHeight = questionImage.rectTransform.sizeDelta.y;

        questionImage.gameObject.SetActive(false);
    }

    public void OpenGate(Gate gate, InventoryManager inventory)
    {
        if (gate.teamType == player.teamType)
        {
            if (gate != null)
            {
                if (gate.CheckGateZeroFragment())
                {
                    Debug.Log("zero fragment, auto open!");
                    gate.OpenGate();
                }
                else if (gate.CheckGateIsReadyReordering())
                {
                    Debug.Log("open game Reordering now!");
                    StartReorderingMiniGame(gate);
                }
                else
                {
                    Debug.Log("Store item key into the gate!");
                    gate.TryOpenGate(inventory);
                }
            } else
            {
                Debug.Log("Inequal Team Type");
            }
        }
    }

    public void StartReorderingMiniGame(Gate gate)
    {
        selectedGate = gate;

        foreach (RectTransform tr in answerContainerParent)
        {
            Destroy(tr.gameObject);
        }

        if (fragmentProgress ==  null) fragmentProgress = new List<int>();
        fragmentProgress.Clear();

        foreach (int fragmentIndex in selectedGate.CollectedFragmentIndex)
        {
            GameObject buttonAnswer = Instantiate(answerImagePrefab, answerContainerParent);
            Button button = buttonAnswer.GetComponent<Button>();

            /*
            if (Application.isEditor)
            {
                button.GetComponentInChildren<Text>().text = GameManager.PlayersTeam[gate.teamType].FragmentsKey.Fragments[fragmentIndex].Data;
            }
            */

            button.GetComponentInChildren<Text>().text = GameManager.PlayersTeam[gate.teamType].FragmentsKey.Fragments[fragmentIndex].Data;

            button.onClick.AddListener(() =>
            {
                ShowAnswerStep(fragmentIndex, buttonAnswer.gameObject);
            });
        }

        questionContainer.SetActive(true);
    }

    private int selectedFragmentIndex = -1;
    private GameObject selectedButtonAnswer = null;

    private void ShowAnswerStep(int fragmentIndex, GameObject buttonAnswer)
    {
        selectedFragmentIndex = fragmentIndex;
        selectedButtonAnswer = buttonAnswer;

        answerNotifWarning.SetActive(false);
        questionImage.gameObject.SetActive(true);
        nextButton.interactable = true;

        Sprite fragmentImage = GameManager.PlayersTeam[player.teamType].FragmentsKey.Fragments[fragmentIndex].DataImage;
        ChangeFragmentImage(fragmentImage);
    }

    private void ChangeFragmentImage(Sprite fragmentImage)
    {
        float originalX = fragmentImage.texture.width;
        float originalY = fragmentImage.texture.height;

        questionImage.sprite = fragmentImage;

        // questionImage.rectTransform.sizeDelta = new Vector2(questionImageTargetWidth, originalY * questionImageTargetHeight / originalX);
        // questionImage.rectTransform.sizeDelta = new Vector2(questionImageTargetWidth, questionImageTargetWidth * originalY / originalX);
        questionImage.rectTransform.sizeDelta = new Vector2(questionImageTargetHeight * originalX / originalY, questionImageTargetHeight);
    }

    // dipencet tombol next
    public void NextAnswerStep()
    {
        if (selectedFragmentIndex >= 0) {
            SelectAnswerStep(selectedFragmentIndex);
            Destroy(selectedButtonAnswer.gameObject);

            selectedFragmentIndex = -1;
            selectedButtonAnswer = null;

            answerNotifWarning.SetActive(true);
            questionImage.gameObject.SetActive(false);
            nextButton.interactable = false;
        }
    }

    private void SelectAnswerStep(int fragment)
    {
        if (selectedGate != null)
        {
            fragmentProgress.Add(fragment);

            if (fragmentProgress.Count == selectedGate.MainKey.Fragments.Count)
            {
                selectedGate.SetCollectedFragment(fragmentProgress);

                if (selectedGate.CheckGateShouldBeOpen())
                {
                    selectedGate.OpenGate();
                }

                CloseReorderingMiniGame();
            }
        }
    }

    public void CloseReorderingMiniGame()
    {
        foreach (RectTransform tr in answerContainerParent)
        {
            Destroy(tr.gameObject);
        }

        questionContainer.SetActive(false);
    }

    private Sprite GetImageOfMainMenu()
    {
        return null;
    }
}
