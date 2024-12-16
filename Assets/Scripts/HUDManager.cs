using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void GameOverHandler();
public delegate void GameWinHandler();

/// <summary>
/// class managing the UI elements for the game.
/// </summary>
public class HUDManager : MonoBehaviour
{
    // Singleton
    private static HUDManager instance;
    public static HUDManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<HUDManager>();
            }
            
            return instance;
        }
    }

    // fields
    [SerializeField] private bool endGame = false;
    
    [Header("HUD")]
    [SerializeField] private RectTransform[] lives;
    [SerializeField] private RectTransform lifePrefab;
    [SerializeField] private RectTransform livesContainer;
    [SerializeField] private TMP_Text scoreText;
    public int score = 0;

    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text gameOverScoreText;
    
    [Header("Game Win Screen")]
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private TMP_Text gameWinScoreText;
    [SerializeField] private TMP_Text gameWinLifeText;
    [SerializeField] private TMP_Text gameWinFinalScoreText;

    [Header("Boss HP")]
    [SerializeField] private GameObject bossHPContainer;
    [SerializeField] private Slider bossHPBar;

    public event GameOverHandler OnGameOver; // called whenever the game end
    public event GameWinHandler OnGameWin; // called when player wins specifically
    
    // methods
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // initialize UI elements
        PlayerController.Player.OnEntityHealthChanged += UpdatePlayerHealthUI;
        OnGameWin += DisplayGameWin;
        OnGameWin += () => OnGameOver.Invoke();
        InitPlayerHealthUI();
        score = 0;
        UpdateScore(0);
        gameOverScreen.SetActive(false);
        gameWinScreen.SetActive(false);
        bossHPContainer.SetActive(false);
    }

    private void Update()
    {
        // debug flag to test ending the game.
        if (endGame)
        {
            OnGameOver.Invoke();
            DisplayGameOver();
        }
    }
    
    private void OnDestroy()
    {
        // unsubscribe from events if object is destroyed.
        PlayerController.Player.OnEntityHealthChanged -= UpdatePlayerHealthUI;
        OnGameWin -= DisplayGameWin;
    }

    /// <summary>
    /// Update the player life display UI.
    /// </summary>
    /// <param name="newHealth"></param>
    private void UpdatePlayerHealthUI(int newHealth)
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < newHealth)
            {
                lives[i].gameObject.SetActive(true);
            }
            else
            {
                lives[i].gameObject.SetActive(false);
            }
        }

        if (newHealth < 0)
        {
            OnGameOver.Invoke();
            DisplayGameOver();
        }
    }

    /// <summary>
    /// Initialize the player life display by creating UI elements for it and arranging them.
    /// </summary>
    private void InitPlayerHealthUI()
    {
        lives = new RectTransform[PlayerController.Player.MaxHealth];
        for (int i = 0; i < lives.Length; i++)
        {
            lives[i] = Instantiate(lifePrefab, livesContainer);
            lives[i].anchoredPosition = new Vector2(i * lifePrefab.rect.width, 0);
        }
    }
    
    /// <summary>
    /// Update the score total and display.
    /// </summary>
    /// <param name="deltaScore"></param>
    public void UpdateScore(int deltaScore)
    {
        score += deltaScore;
        scoreText.text = score.ToString();
    }
    
    /// <summary>
    /// Enable the boss HP for the start of a bossfight.
    /// </summary>
    public void DisplayBossHP()
    {
        bossHPContainer.SetActive(true);
        bossHPBar.value = 1;
    }

    /// <summary>
    /// Update value on the boss hp
    /// </summary>
    /// <param name="newValue">Range from 0 to 1, percentage of the HP bar.</param>
    public void UpdateBossHP(float newValue)
    {
        bossHPBar.value = newValue;
    }

    /// <summary>
    /// Show the game over score screen.
    /// </summary>
    private void DisplayGameOver()
    {
        gameOverScreen.SetActive(true);
        gameOverScoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Called to invoke the Game Win event when the player beats the level.
    /// </summary>
    public void GameWin()
    {
        OnGameWin.Invoke();
    }
    
    /// <summary>
    /// Show the game win score screen.
    /// </summary>
    private void DisplayGameWin()
    {
        gameWinScreen.SetActive(true);
        gameWinScoreText.text = "Level Score: " + score;
        // give the player a bonus to score on remaining lives.
        gameWinLifeText.text = "Life Bonus: " + PlayerController.Player.Health * 1000;
        gameWinFinalScoreText.text = "Final Score: " + (score + PlayerController.Player.Health * 1000);
    }

    // Restart the game.
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
