using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Canvas controlsCanvas;

    [SerializeField] private List<Button> buttons = new List<Button>();

    public void OnStartButtonPressed()
    {
        SetAllButtonsInteractable(false);

        gameManager.SetupGame();
    }

    public void SetAllButtonsInteractable(bool interactable)
    {
        foreach (Button button in buttons)
        {
            button.interactable = interactable;
        }

        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Button[] buttons = GameObject.FindObjectsOfType<Button>();

            for (int i = 9; i < buttons.Length; i++)
            {
                if (buttons[i].interactable)
                {
                    EventSystem.current.SetSelectedGameObject(buttons[i].gameObject);
                }
            }
        }
    }

    public void SetControlsShowing(bool showControls)
    {
        if (showControls)
        {
            TweenObjectPosition("Main Menu Position", this.gameObject, Vector3.zero, new Vector3(0, 10, 0), 1);
            TweenObjectPosition("Controls Menu Position", controlsCanvas.gameObject, new Vector3(0, -10, 0), Vector3.zero, 1);
        }
        else
        {
            TweenObjectPosition("Main Menu Position", this.gameObject, new Vector3(0, 10, 0), Vector3.zero, 1, enableMenuInternal);
            TweenObjectPosition("Controls Menu Position", controlsCanvas.gameObject, Vector3.zero, new Vector3(0, -10, 0), 1);
        }
    }

    private void enableMenuInternal()
    {
        SetAllButtonsInteractable(true);
    }

    private void TweenObjectPosition(string tweenName, GameObject _object, Vector3 from, Vector3 to, float time, System.Action runOnComplete = null)
    {
        System.Action<ITween<Vector3>> updateObjectPosition = (t) =>
        {
            _object.transform.localPosition = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> updatePositionComplete = (t) =>
        {
            if (runOnComplete != null)
            {
                runOnComplete();
            }
        };

        var tween = _object.Tween(tweenName, from, to, time, TweenScaleFunctions.SineEaseInOut, updateObjectPosition, updatePositionComplete);

        tween.ForceUpdate = true;
    }
}
