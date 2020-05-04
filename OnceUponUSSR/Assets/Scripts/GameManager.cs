using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance = null;
    public BoardManager BoardScript;
    
    public int PlayerFoodPoints = 100;
    public float TurnDelay = 0.1f;
    public float LevelLoadDelay = 2f;

    [HideInInspector] public bool PlayersTurn = false;

    public List<string> EasterEggsText;
    public List<int> EasterEggsLevel;

    private int _level = 1;
    private List<Enemy> _enemies;
    private bool _enemiesMoving = false;

    private Text _levelText;
    private GameObject _levelImage;
    private bool _doingSetup = false;

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

    private void OnLevelWasLoaded(int level)
    {
        _level++;

        InitGame();
    }

    void Update()
    {
        if (PlayersTurn || _enemiesMoving || _doingSetup)
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void GameOver()
    {
        _levelImage.SetActive(true);
        _levelText.text = "After " + _level + " days, you starved.";
        enabled = false;
    }

    void InitGame()
    {
        _doingSetup = true;
        _levelImage = GameObject.Find("Background");
        _levelText = GameObject.Find("LevelText").GetComponent<Text>(); 
        _levelImage.SetActive(true);
        SetLevelText();
        Invoke("HideLevelImage", LevelLoadDelay);

        _enemies.Clear();
        BoardScript.SetupScene(_level);
    }

    void SetLevelText()
    {
        if (EasterEggsLevel.Count > 0 && EasterEggsLevel[0] <= _level) {

            _levelText.text = EasterEggsText[0];
            EasterEggsLevel.RemoveAt(0);
            EasterEggsText.RemoveAt(0);

        } else {
            _levelText.text = "Day " + _level.ToString();
        }
    }

    private void HideLevelImage()
    {
        _levelImage.SetActive(false);
        _doingSetup = false;
        PlayersTurn = true;
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
