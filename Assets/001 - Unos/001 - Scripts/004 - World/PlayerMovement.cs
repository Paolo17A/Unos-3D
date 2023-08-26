using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //=============================================================================================
    [SerializeField] private WorldCore WorldCore;

    [Header("MOVEMENT VARIABLES")]
    [SerializeField] private Animator anim;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float maxSpeed = 15;
    [SerializeField] Joystick joystick;

    [Header("GENDER VARIABLES")]
    [SerializeField] private GameObject MaleCharacter;
    [SerializeField] private GameObject FemaleCharacter;

    float x;
    float z;
    //=============================================================================================
    void Start()
    {
        if(GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
        {
            MaleCharacter.SetActive(true);
            FemaleCharacter.SetActive(false);
        }
        else if (GameManager.Instance.PlayerGender == GameManager.Gender.FEMALE)
        {
            MaleCharacter.SetActive(false);
            FemaleCharacter.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldCore.IsSettingsOpen()) return;
        if(Application.isEditor)
        {
             x = Input.GetAxis("Horizontal");
             z = Input.GetAxis("Vertical");
        }
        else
        {
             x = joystick.Horizontal;
             z = joystick.Vertical;
        }

        Vector3 movementDirection = new Vector3(x, 0, z);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);


        float speed = inputMagnitude * maxSpeed;
        movementDirection = Quaternion.AngleAxis(GameManager.Instance.MainCamera.transform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        Vector3 velocity = movementDirection * speed;

        controller.Move(velocity* Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);
        }

        anim.SetBool("isWalking", speed > 0);

        if(!controller.isGrounded)
            controller.Move(Vector3.down * 9.8f * Time.deltaTime);
        //transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
    }
}