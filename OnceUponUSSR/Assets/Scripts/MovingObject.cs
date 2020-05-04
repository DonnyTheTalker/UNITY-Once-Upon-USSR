﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{

    public float MoveTime = 0.1f;
    public LayerMask BlockingLayer;

    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb2D;
    private float _inverseMoveTime;

    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
        _inverseMoveTime = 1f / MoveTime;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDitance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDitance > float.Epsilon) {

            Vector3 newPosition = Vector3.MoveTowards(_rb2D.position, end, _inverseMoveTime * Time.deltaTime);
            _rb2D.MovePosition(newPosition);
            sqrRemainingDitance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        _boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, BlockingLayer);
        _boxCollider.enabled = true;

        if (hit == null) {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
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
