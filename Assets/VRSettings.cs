using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRSettings : MonoBehaviour
{
    private void Start()
    {
        XRSettings.eyeTextureResolutionScale = 1.5f;
    }
}