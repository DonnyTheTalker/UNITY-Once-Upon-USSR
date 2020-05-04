using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int Damage;

    private Animator _animator;
    private Transform _target;
    private bool _skipMove;

    protected override void Start()
    {
        GameManager.Instance.AddEnemyToList(this);
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _skipMove = false;

        _currectDirection = -1;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (_skipMove) {
            _skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        _skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0; 

        if (Mathf.Abs(_target.transform.position.x - transform.position.x) <= 0.003) {
            yDir = _target.transform.position.y < transform.position.y ? -1 : 1;
        } else {
            xDir = _target.transform.position.x < transform.position.x ? -1 : 1;
        }

        if (xDir != 0 && xDir != _currectDirection)
            ChangeDirection();

        AttemptMove<Player>(xDir, yDir);

    }

    protected override void OnCanMove<T>(T component)
    {
        Player player = component as Player;

        _animator.SetTrigger("EnemyAttack");

        player.LoseFood(Damage); 

    }

}
