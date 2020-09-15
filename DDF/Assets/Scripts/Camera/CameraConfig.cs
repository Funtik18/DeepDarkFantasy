using UnityEngine;

[CreateAssetMenu(menuName = "Camera/Config")]
public class CameraConfig : ScriptableObject
{
    [Header("General")]
    [Tooltip("Mouse sensitivity")]
    public float rotationSpeed = 2;
    [Tooltip("Distance to observations object")]
    public float offsetDistance = 5;
    [Tooltip("Height of camera")]
    public float offsetHeight = 2.3f;
    [Tooltip("Distance towards the right shoulder")]
    public float offsetRight = 0.0f;

    [Header("Clamp angle")]
    public float upAngle = 45f;
    public float downAngle = 45f;

    [Header("Invert")]
    public bool inversionX = false;
    public bool inversionY = false;

    [Header("Smooth Movement")]
    public bool smooth = true;
    [Tooltip("Smooth speed")]
    public float moveSpeed = 8;
}
