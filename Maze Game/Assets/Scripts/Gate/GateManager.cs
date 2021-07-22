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

    private List<MainGateFragment> fragmentProgress;

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

        if (fragmentProgress ==  null) fragmentProgress = new List<MainGateFragment>();
        fragmentProgress.Clear();

        foreach (MainGateFragment fragment in selectedGate.CollectedFragment)
        {
            GameObject buttonAnswer = Instantiate(answerImagePrefab, answerContainerParent);
            Button button = buttonAnswer.GetComponent<Button>();

            if (Application.isEditor)
            {
                button.GetComponent<Text>().text = fragment.Data;
            }

            button.onClick.AddListener(() =>
            {
                SelectAnswerStep(fragment);
                Destroy(buttonAnswer.gameObject);
            });
        }

        questionContainer.SetActive(true);
    }

    private void SelectAnswerStep(MainGateFragment fragment)
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
