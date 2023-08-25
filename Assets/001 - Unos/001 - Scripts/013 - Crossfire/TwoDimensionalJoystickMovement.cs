using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionalJoystickMovement : MonoBehaviour
{
    [SerializeField] private CrossfireCore CrossfireCore;
    public float moveSpeed = 5f;
    public Joystick stick;
    private Rigidbody2D rb;

    [Header("MALE")]
    [SerializeField] private SpriteRenderer MaleSprite;
    [SerializeField] private Animator MaleAnimator;

    [Header("FEMALE")]
    [SerializeField] private SpriteRenderer FemaleSprite;
    [SerializeField] private Animator FemaleAnimator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
        {
            MaleSprite.gameObject.SetActive(true);
            FemaleSprite.gameObject.SetActive(false);
        }
        else
        {
            MaleSprite.gameObject.SetActive(false);
            FemaleSprite.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (CrossfireCore.CurrentCrossFireGameState != CrossfireCore.CrossfireGameStates.GAMEPLAY) return;

        // Get input from the joystick-like input axes
        float horizontalInput = stick.Horizontal;
        float verticalInput = stick.Vertical;

        // Calculate the movement direction
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Calculate the movement velocity
        Vector2 moveVelocity = moveDirection * moveSpeed;

        if (moveVelocity == Vector2.zero)
        {
            MaleAnimator.SetFloat("speed", 0);
            FemaleAnimator.SetFloat("speed", 0);
        }
        else
        {
            MaleAnimator.SetFloat("speed", 1);
            FemaleAnimator.SetFloat("speed", 1);

            // Flip sprite based on movement direction
            if (moveDirection.x < 0)
            {
                MaleSprite.flipX = true;
                FemaleSprite.flipX = true;
            }
            else if (moveDirection.x > 0)
            {
                MaleSprite.flipX = false;
                FemaleSprite.flipX = false; 
            }

        }

        // Apply the velocity to the Rigidbody2D
        rb.velocity = moveVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "selectable")
            CrossfireCore.CurrentCrossFireGameState = CrossfireCore.CrossfireGameStates.BURNED;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "safety")
            CrossfireCore.CurrentCrossFireGameState = CrossfireCore.CrossfireGameStates.SAFE;
    }
}
