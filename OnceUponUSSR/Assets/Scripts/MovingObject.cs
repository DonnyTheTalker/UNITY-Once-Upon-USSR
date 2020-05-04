using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{

    public float MoveTime = 0.1f;
    public LayerMask BlockingLayer;

    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb2D;
    private float _inverseMoveTime;

    protected float _currectDirection = 1;

    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
        _inverseMoveTime = 1f / MoveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        _boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, BlockingLayer);
        _boxCollider.enabled = true;

        if (hit.transform == null) {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected void ChangeDirection()
    {
        Vector3 newDirection = transform.localScale;
        newDirection.x = -newDirection.x;
        transform.localScale = newDirection;
        _currectDirection = -_currectDirection;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDitance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDitance > 0.004) {

            Vector3 newPosition = Vector3.MoveTowards(_rb2D.position, end, _inverseMoveTime * Time.deltaTime);
            _rb2D.MovePosition(newPosition);
            sqrRemainingDitance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        _rb2D.MovePosition(end);

    } 

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCanMove(hitComponent);

    }

    protected abstract void OnCanMove<T>(T component)
        where T : Component;

}
