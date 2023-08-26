using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreelookHandler : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private Joystick lookStick;

    private void Update()
    {
        freeLookCamera.m_XAxis.m_InputAxisValue = lookStick.Horizontal * 0.5f;
        freeLookCamera.m_YAxis.m_InputAxisValue = lookStick.Vertical * 0.5f;
    }
}
