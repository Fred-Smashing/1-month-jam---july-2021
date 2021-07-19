using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game"), Space]
    [SerializeField] private Spawner spawner;
    [SerializeField] private PlayerController player;

    [Header("Menus"), Space]
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas scoreCanvas;

    Vector3 playerStartPos;

    float maxHeight;
    float minHeight;

    private void Awake()
    {
        maxHeight = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        playerStartPos = player.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void StartGame()
    {
        TweenObjectPosition("Tween Menu Position", mainMenuCanvas.gameObject, Vector3.zero, new Vector3(-15, 0, 0), 3f, toggleMainMenuEnabled);
        TweenObjectPosition("Tween Player Position", player.gameObject, playerStartPos, Vector3.zero, 3f, SetupPlayer);
        TweenObjectPosition("Tween Score Position", scoreCanvas.gameObject, new Vector3(0, -10, 0), Vector3.zero, 3f);
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

        var tween = player.gameObject.Tween(tweenName, from, to, time, TweenScaleFunctions.SineEaseInOut, updateObjectPosition, updatePositionComplete);

        tween.ForceUpdate = true;
    }

    private void SetupPlayer()
    {
        player.Init();
        player.SetPlayerEnabled(true);

        spawner.Init(maxHeight, minHeight);
    }

    private void toggleMainMenuEnabled()
    {
        mainMenuCanvas.gameObject.SetActive(!mainMenuCanvas.gameObject.activeInHierarchy);
    }
}

