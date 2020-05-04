using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public BoardManager BoardScript;
    public int PlayerFoodPoints = 100;
    public float TurnDelay = 0.1f;
    [HideInInspector] public bool PlayersTurn = true;


    private int _level = 15;
    private List<Enemy> _enemies;
    private bool _enemiesMoving = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        _enemies = new List<Enemy>();
        BoardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void Update()
    {
        if (PlayersTurn || _enemiesMoving)
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void GameOver()
    {
        enabled = false;
    }

    void InitGame()
    {
        _enemies.Clear();
        BoardScript.SetupScene(_level);
    }

    IEnumerator MoveEnemies()
    {
        _enemiesMoving = true;
        yield return new WaitForSeconds(TurnDelay);
        if (_enemies.Count == 0) {
            yield return new WaitForSeconds(TurnDelay);
        }

        for (int i = 0; i < _enemies.Count; i++) {
            _enemies[i].MoveEnemy();
            yield return new WaitForSeconds(TurnDelay);
        }

        PlayersTurn = true;
        _enemiesMoving = false;

    }

}
