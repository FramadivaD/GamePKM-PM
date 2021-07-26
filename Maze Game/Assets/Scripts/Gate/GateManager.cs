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

    private List<int> fragmentProgress;

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public void OpenGate(Gate gate, InventoryManager inventory)
    {
        if (gate.teamType == player.teamType)
        {
            if (gate != null)
            {
                /*
                if (gate.CheckGateShouldBeOpen())
                {
                    Debug.Log("its match. Open Gate Now!");
                    gate.OpenGate();
                }
                else 
                */
                if (gate.CheckGateIsReadyReordering())
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

            if (Application.isEditor)
            {
                button.GetComponentInChildren<Text>().text = GameManager.PlayersTeam[gate.teamType].FragmentsKey.Fragments[fragmentIndex].Data;
            }

            button.onClick.AddListener(() =>
            {
                SelectAnswerStep(fragmentIndex);
                Destroy(buttonAnswer.gameObject);
            });
        }

        questionContainer.SetActive(true);
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
