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

    void Awake()
    {
        Instance = this;
    }

    public void SetGameOver()
    {
        m_gameOverText.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (m_gameOverText.gameObject.activeSelf && (Input.GetButton("XboxRBPlayer1") || Input.GetButton("XboxRBPlayer2")))
        {
            SceneManager.LoadScene("NovaTestScene");
        }
    }
}
