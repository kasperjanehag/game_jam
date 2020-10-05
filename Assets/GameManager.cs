using System.Collections;
using System.Collections.Generic;
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
    }

    public void SetGameOver()
    {
        StartCoroutine(SetCanContinueAfterDelay());
        m_gameOverText.gameObject.SetActive(true);
    }

    private IEnumerator SetCanContinueAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        m_canContinue = true;
    }

    public void Update()
    {
        if (m_canContinue && (Input.GetMouseButtonDown(0) || Input.GetButton("XboxRBPlayer1") || Input.GetButton("XboxRBPlayer2")))
        {
            SceneManager.LoadScene("NovaTestScene");
        }
    }
}
