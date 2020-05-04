using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;
    public BoardManager BoardScript;
    public int PlayerFoodPoints = 100;
    [HideInInspector] public bool PlayersTurn = true;

    private int _level = 2;

    private void Awake()
    { 
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        BoardScript = GetComponent<BoardManager>();
        InitGame(); 
    }

    public void GameOver()
    {
        enabled = false;
    }

    void InitGame()
    {
        BoardScript.SetupScene(_level);
    }

}
