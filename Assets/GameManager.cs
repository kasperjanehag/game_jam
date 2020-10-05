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

    private bool m_gameOver = false;
    public static bool m_hasStartedFirstTime = false;

    public bool HasStarted = false;
    private bool m_canContinue = false;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Load level
        var index = Random.Range(0, m_levels.Length);
        var level = m_levels[index];
        Instantiate(level, m_levelOrigin.transform);

        if (m_hasStartedFirstTime)
        {
            StartCoroutine(FadeOutImageAndStartGame());
            //HasStarted = true;

            //m_image.gameObject.SetActive(false);
            //m_text.gameObject.SetActive(false);
        }
    }

    public void SetGameOver()
    {
        m_gameOver = true;
        HasStarted = false;
        StartCoroutine(SetCanContinueAfterDelay());
        m_image.gameObject.SetActive(true);
        m_image.CrossFadeAlpha(1, 0.5f, false);
        m_text.gameObject.SetActive(true);
        m_text.text = "GAME OVER! PRESS ANY BUTTON TO RESTART!";
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

    public void Update()
    {
        if (!HasStarted && Input.GetMouseButtonDown(0) && !m_gameOver)
        {
            StartCoroutine(FadeOutImageAndStartGame());
        }

        if (m_canContinue && (Input.GetMouseButtonDown(0) || Input.GetButton("XboxRBPlayer1") || Input.GetButton("XboxRBPlayer2")))
        {
            SceneManager.LoadScene("NovaTestScene");
        }
    }
}
