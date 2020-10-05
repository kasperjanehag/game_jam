using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameConfig Config { get => m_gameConfig; }

    [SerializeField] private Text m_gameOverText;
    [SerializeField] private GameConfig m_gameConfig;
    [SerializeField] private GameObject m_levelOrigin;
    [SerializeField] private GameObject[] m_levels;
    [SerializeField] private Image m_image;
    [SerializeField] private Text m_text;

    [SerializeField]
    private GameObject[] m_healthPlayerOne;
    [SerializeField]
    private GameObject[] m_healthPlayerTwo;

    [SerializeField]
    private Text m_scoreTextPlayerOne;
    [SerializeField]
    private Text m_scoreTextPlayerTwo;

    private static int m_scorePlayerOne = 0;
    private static int m_scorePlayerTwo = 0;

    private bool m_gameOver = false;
    public static bool m_hasStartedFirstTime = false;

    public bool HasStarted = false;
    private bool m_canContinue = false;
    void Awake()
    {
        Instance = this;
    }

    public void SetHealth(int health, bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            m_healthPlayerOne[health].SetActive(false);
        }
        else
        {
            m_healthPlayerTwo[health].SetActive(false);
        }
    }

    private void Start()
    {
        // Load level
        var index = Random.Range(0, m_levels.Length);
        var level = m_levels[index];
        Instantiate(level, m_levelOrigin.transform);

        m_scoreTextPlayerOne.text = "PLAYER ONE \n" + m_scorePlayerOne;
        m_scoreTextPlayerTwo.text = "PLAYER TWO \n" + m_scorePlayerTwo;

        if (m_hasStartedFirstTime)
        {
            StartCoroutine(FadeOutImageAndStartGame());
        }
    }

    public void SetGameOver(bool isPlayerOne)
    {
        m_gameOver = true;
        HasStarted = false;
        StartCoroutine(SetCanContinueAfterDelay());
        m_image.gameObject.SetActive(true);
        m_image.CrossFadeAlpha(1, 0.5f, false);
        m_text.gameObject.SetActive(true);
        m_text.gameObject.SetActive(true);

        if (isPlayerOne)
        {
            m_scorePlayerOne++;
        }
        else
        {
            m_scorePlayerTwo++;
        }

        m_scoreTextPlayerOne.text = "PLAYER ONE \n" + m_scorePlayerOne;
        m_scoreTextPlayerTwo.text = "PLAYER TWO \n" + m_scorePlayerTwo;

        var playerOneText = isPlayerOne ? "PLAYER 1" : "PLAYER 2";
        m_text.text = playerOneText + " WON! PRESS ANY BUTTON TO RESTART!";
    }

    private IEnumerator SetCanContinueAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        m_canContinue = true;
    }

    private IEnumerator FadeOutImageAndStartGame()
    {
        m_hasStartedFirstTime = true;
        //Fully fade in Image (1) with the duration of 2
        m_image.CrossFadeAlpha(0, 0.5f, false);
        m_text.gameObject.SetActive(false);
        yield return  new WaitForSeconds( 0.5f);
        HasStarted = true;
    }

    private bool IsButtonDown()
    {
        return (Input.GetMouseButtonDown(0) || Input.GetButton("XboxRBPlayer1") || Input.GetButton("XboxRBPlayer2"));
    }

    public void Update()
    {
        if (!HasStarted && IsButtonDown() && !m_gameOver)
        {
            StartCoroutine(FadeOutImageAndStartGame());
        }

        if (m_canContinue && IsButtonDown())
        {
            SceneManager.LoadScene("NovaTestScene");
        }
    }
}
