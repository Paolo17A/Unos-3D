using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionalPlayerMovement : MonoBehaviour
{
    [SerializeField] private StairsGameCore StairsGameCore;
    [SerializeField] private float moveSpeed = 15f; // Adjust this value to control movement speed
    private Rigidbody2D rb;

    [Header("MALE")]
    [SerializeField] private SpriteRenderer MaleSprite;
    [SerializeField] private Animator MaleAnimator;

    [Header("FEMALE")]
    [SerializeField] private SpriteRenderer FemaleSprite;
    [SerializeField] private Animator FemaleAnimator;
    
    [SerializeField][ReadOnly] bool isMoving; // Flag to track if the player is currently moving

    float moveDirection;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if(GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
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

    // Called when the "Left" button is pressed
    public void OnLeftButtonDown()
    {
        isMoving = true;
        MaleSprite.flipX = true;
        MaleAnimator.SetFloat("speed", 1);
        FemaleSprite.flipX = true;
        FemaleAnimator.SetFloat("speed", 1);
    }

    // Called when the "Right" button is pressed
    public void OnRightButtonDown()
    {
        isMoving = true;
        MaleSprite.flipX = false;
        MaleAnimator.SetFloat("speed", 1);
        FemaleSprite.flipX = false;
        FemaleAnimator.SetFloat("speed", 1);
    }

    // Called when either button is released
    public void OnButtonUp()
    {
        isMoving = false;
        
    }

    private void Update()
    {
        if (StairsGameCore.CurrentStairsGameState != StairsGameCore.StairsGameStates.GAMEPLAY) return;
        if (isMoving)
        {
            moveDirection = MaleSprite.flipX ? -1f : 1f;
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop movement when not moving
            MaleAnimator.SetFloat("speed", 0);
            FemaleAnimator.SetFloat("speed", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<DebrisHandler>() != null)
            StairsGameCore.CurrentStairsGameState = StairsGameCore.StairsGameStates.STONED;
        else if (collision.collider.tag == "safety")
            StairsGameCore.CurrentStairsGameState = StairsGameCore.StairsGameStates.SAFE;
    }
}
