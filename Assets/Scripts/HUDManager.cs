using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void GameOverHandler();
public delegate void GameWinHandler();

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

    public event GameOverHandler OnGameOver;
    public event GameWinHandler OnGameWin;
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Player.OnEntityHealthChanged += UpdatePlayerHealthUI;
        OnGameWin += DisplayGameWin;
        OnGameWin += () => OnGameOver.Invoke();
        InitPlayerHealthUI();
        score = 0;
        UpdateScore(0);
        gameOverScreen.SetActive(false);
        gameWinScreen.SetActive(false);
    }

    private void Update()
    {
        if (endGame)
        {
            OnGameOver.Invoke();
            DisplayGameOver();
        }
    }

    private void OnDestroy()
    {
        PlayerController.Player.OnEntityHealthChanged -= UpdatePlayerHealthUI;
        OnGameWin += DisplayGameWin;
    }

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

    private void InitPlayerHealthUI()
    {
        lives = new RectTransform[PlayerController.Player.MaxHealth];
        for (int i = 0; i < lives.Length; i++)
        {
            lives[i] = Instantiate(lifePrefab, livesContainer);
            lives[i].anchoredPosition = new Vector2(i * lifePrefab.rect.width, 0);
        }
    }
    
    public void UpdateScore(int deltaScore)
    {
        score += deltaScore;
        scoreText.text = score.ToString();
    }

    private void DisplayGameOver()
    {
        gameOverScreen.SetActive(true);
        gameOverScoreText.text = "Score: " + score;
    }

    public void GameWin()
    {
        OnGameWin.Invoke();
    }
    
    private void DisplayGameWin()
    {
        gameWinScreen.SetActive(true);
        gameWinScoreText.text = "Level Score: " + score;
        gameWinLifeText.text = "Life Bonus: " + PlayerController.Player.Health * 1000;
        gameWinFinalScoreText.text = "Final Score: " + (score + PlayerController.Player.Health * 1000);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
