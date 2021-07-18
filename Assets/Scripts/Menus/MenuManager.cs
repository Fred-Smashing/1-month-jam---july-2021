using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private List<Button> buttons = new List<Button>();

    public void OnStartButtonPressed()
    {
        SetAllButtonsInteractable(false);

        gameManager.StartGame();
    }

    private void SetAllButtonsInteractable(bool interactable)
    {
        foreach (Button button in buttons)
        {
            button.interactable = interactable;
        }
    }
}
