using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;

    private Animator _animator;
    private int _food;
    public Text FoodText;

    public AudioClip MoveSound1;
    public AudioClip MoveSound2;
    public AudioClip EatSound1;
    public AudioClip EatSound2;
    public AudioClip DrinkSound1;
    public AudioClip DrinkSound2;
    public AudioClip DieSound;

    protected override void Start()
    {
        _animator = GetComponent<Animator>();

        _food = GameManager.Instance.PlayerFoodPoints;
        FoodText.text = "Songs left: " + _food;

        base.Start();
    }

    private void Update()
    {
        if (!GameManager.Instance.PlayersTurn) return;

        int horizontal = 0, vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != _currectDirection && horizontal != 0)
            ChangeDirection();

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);

    }  

    private void CheckIfGameOver()
    {
        if (_food <= 0) {

            SoundManager.Instance.MusicSource.Stop();
            SoundManager.Instance.PlaySingle(DieSound);
            GameManager.Instance.GameOver();

        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = _food;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit") {

            Invoke("Restart", RestartLevelDelay);
            enabled = false;

        } else if (collision.tag == "Food") {

            SoundManager.Instance.RandomizeSfx(EatSound1, EatSound2);
            _food += PointsPerFood;
            FoodText.text = "+ " +  PointsPerFood + " Songs left: " + _food;
            collision.gameObject.SetActive(false);

        } else if (collision.tag == "Soda") {

            SoundManager.Instance.RandomizeSfx(DrinkSound1, DrinkSound2);
            _food += PointsPerSoda;
            FoodText.text = "+ " + PointsPerSoda + " Songs left: " + _food;
            collision.gameObject.SetActive(false);

        }
    }

    protected override void OnCanMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.TakeDamage(WallDamage);

        _animator.SetTrigger("PlayerChop");
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    { 
        FoodText.text = "Songs left: " + _food;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit; 
        _food--;

        if (Move(xDir, yDir, out hit)) { 
            SoundManager.Instance.RandomizeSfx(MoveSound1, MoveSound2);
        }

        CheckIfGameOver();

        GameManager.Instance.PlayersTurn = false;

    } 
    public void LoseFood(int loss)
    {  
        _animator.SetTrigger("PlayerHit");
        _food -= loss;
        FoodText.text = "- " + loss + " Songs left: " + _food;
        CheckIfGameOver();
    }

}
