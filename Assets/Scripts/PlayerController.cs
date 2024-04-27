using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField]
    private Rigidbody2D rigidbody2D;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerInventoryData inventory;
    [SerializeField]
    private CustomClothes customClothes;

    [Space]
    [Header("Movement")]
    [SerializeField]
    private float speed = 5f;

    private static readonly int Horizontal = Animator.StringToHash("Horizontal");

    #endregion

    #region Inputs

    private float HorizontalInput => Input.GetAxis("Horizontal");
    private float SprintInput => Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;
    private Vector2 MovementInput => new Vector2(HorizontalInput*SprintInput, 0);

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        if (!rigidbody2D) TryGetComponent(out rigidbody2D);
        if (!animator) TryGetComponent(out animator);
    }

    private void Update()
    {
        Move();
    }

    #endregion

    #region Movement

    private void Move()
    {
        rigidbody2D.velocity = MovementInput * speed;
        animator.SetFloat(Horizontal, MovementInput.x);
        transform.localScale = MovementInput.x switch
        {
            > 0 => new Vector3(1, 1, 1),
            < 0 => new Vector3(-1, 1, 1),
            _ => transform.localScale
        };
    }

    #endregion
}
