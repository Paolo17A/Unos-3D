using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity;
    public Transform playerBody;

    float xRotation;
    float mouseX = 0;
    float mouseY = 0;
    [SerializeField] Joystick joystick;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && Mouse.current != null)
        {
            mouseX = Mouse.current.delta.ReadValue().x;
            mouseY = Mouse.current.delta.ReadValue().y;
        }

        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");

        else
        {
            mouseX = joystick.Horizontal;
            mouseY = joystick.Vertical;
        }
        /*else if(Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].isInProgress)
        {
            if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
                return;

            mouseX = Touchscreen.current.touches[0].delta.ReadValue().x;
            mouseY = Touchscreen.current.touches[0].delta.ReadValue().y;
        }*/

        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(Vector3.right * mouseY * Time.deltaTime);
        mouseX = 0;
        mouseY = 0;
    }
}