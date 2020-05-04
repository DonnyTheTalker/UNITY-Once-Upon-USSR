using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;

    private Animator _animator;
    private int _food;

    protected override void Start()
    {
        _animator = GetComponent<Animator>();

        _food = GameManager.Instance.PlayerFoodPoints;

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
        if (_food <= 0)
            GameManager.Instance.GameOver();
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
            _food += PointsPerFood;
            collision.gameObject.SetActive(false);
        } else if (collision.tag == "Soda") {
            _food += PointsPerSoda;
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
        _food--;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.Instance.PlayersTurn = false;

    } 
    public void LoseFood(int loss)
    {
        _animator.SetTrigger("PlayerHit");
        _food -= loss;
        CheckIfGameOver();
    }

}
