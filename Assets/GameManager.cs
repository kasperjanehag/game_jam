using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameConfig Config { get => m_gameConfig; }

    [SerializeField] private GameConfig m_gameConfig;

    void Awake()
    {
        Instance = this;
    }
}
