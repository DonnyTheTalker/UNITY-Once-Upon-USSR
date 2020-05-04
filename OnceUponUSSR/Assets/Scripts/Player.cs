using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;

    private Animator _animator;
    private int food;

    protected override void Start()
    {
        _animator = GetComponent<Animator>();

        food = GameManager.Instance.PlayerFoodPoints;

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

        horizontal = Mathf.Min(horizontal, 1);
        vertical = Mathf.Min(vertical, 1);

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);

    }  

    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.Instance.GameOver();
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = food;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit") {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        } else if (collision.tag == "food") {
            food += PointsPerFood;
            collision.gameObject.SetActive(false);
        } else if (collision.tag == "soda") {
            food += PointsPerSoda;
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
        food--;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        //GameManager.Instance.PlayersTurn = false;

    } 
    public void LoseFood(int loss)
    {
        _animator.SetTrigger("PlayerHit");
        food -= loss;
        CheckIfGameOver();
    }

}
