using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game"), Space]
    [SerializeField] private Spawner spawner;

    [Header("Player"), Space]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 playerStartPos = Vector3.zero;
    private PlayerController player;

    [Header("Menus"), Space]
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas scoreCanvas;

    private TMPro.TMP_Text scoreText;

    public bool gameOver = false;

    private float difficultyMultiplier = 1f;

    public int enemiesDestroyed;

    float maxHeight;
    float minHeight;

    private void Awake()
    {
        maxHeight = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        scoreText = GameObject.Find("Score Text").GetComponent<TMPro.TMP_Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && gameOver)
        {
            Time.timeScale = 1;
            toggleMainMenuEnabled();
            TweenToMenu();
        }

        if (gameOver)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += 0.1f * Time.deltaTime;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void SetupGame()
    {
        var playerObject = Instantiate(playerPrefab);
        playerObject.transform.position = playerStartPos;
        player = playerObject.GetComponent<PlayerController>();
        SetupPlayer();

        gameOver = false;

        TweenStartOfGame();
    }

    public void EndGame()
    {
        var time = Time.time - timeStarted;
        var timeScore = CalculateTimeScore(time);

        var enemyScore = CalculateEnemyScore(enemiesDestroyed);

        var score = Mathf.Round(timeScore + enemyScore * difficultyMultiplier);

        scoreText.text = string.Format("Score: {0}", score);

        TweenEndOfGame();

        gameOver = true;

        spawner.StopAllCoroutines();
        spawner.CleanupLeftoverObjects();
    }

    private float CalculateTimeScore(float time)
    {
        var multiplier = 1 - 1 / (1 + 0.1f * (time * 0.5f));

        var score = time + (time * multiplier);

        return score;
    }

    private float CalculateEnemyScore(int enemies)
    {
        var scaling = Mathf.Sqrt(1 / Mathf.Sqrt(enemies * Mathf.Sqrt(enemies)));

        return enemiesDestroyed * (10 * scaling); 
    }

    private void TweenStartOfGame()
    {
        TweenObjectPosition("Tween Menu Position", mainMenuCanvas.gameObject, Vector3.zero, new Vector3(-15, 0, 0), 3f, toggleMainMenuEnabled);
        TweenObjectPosition("Tween Player Position", player.gameObject, playerStartPos, Vector3.zero, 3f, EnablePlayer);

    }

    private void TweenEndOfGame()
    {
        TweenObjectPosition("Tween Score Position", scoreCanvas.gameObject, new Vector3(15, 0, 0), Vector3.zero, 1f);
    }

    private void TweenToMenu()
    {
        TweenObjectPosition("Tween Menu Position", mainMenuCanvas.gameObject, new Vector3(15, 0, 0), Vector3.zero, 3f, EnableMainMenuButtons);
        TweenObjectPosition("Tween Score Position", scoreCanvas.gameObject, Vector3.zero, new Vector3(-15, 0, 0), 3f);
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

    private void SetupPlayer()
    {
        player.Init();
    }

    private void EnablePlayer()
    {
        player.SetPlayerEnabled(true);
        StartGame();
    }

    float timeStarted;
    private void StartGame()
    {
        spawner.Init(maxHeight, minHeight);
        timeStarted = Time.time;
    }

    private void toggleMainMenuEnabled()
    {
        mainMenuCanvas.gameObject.SetActive(!mainMenuCanvas.gameObject.activeInHierarchy);
    }

    private void EnableMainMenuButtons()
    {
        mainMenuCanvas.GetComponent<MenuManager>().SetAllButtonsInteractable(true);
    }
}

