using UnityEngine;

[CreateAssetMenu(menuName = "Camera/Config")]
public class CameraConfig : ScriptableObject
{
    [Header("General")]
    [Tooltip("Mouse sensitivity")]
    [Range(1f, 30f)]
    public float rotationSpeed = 2;
    [Tooltip("Distance to observations object")]
    [Range(0f, 20f)]
    public float offsetDistance = 5;
    [Tooltip("Height of camera")]
    public float offsetHeight = 2.3f;
    [Tooltip("Distance towards the right shoulder")]
    [Range(-3f, 3f)]
    public float offsetRight = 0.0f;

    [Header("Clamp angle")]
    [Range(10f, 90f)]
    public float upAngle = 45f;
    [Range(10f, 90f)]
    public float downAngle = 45f;

    [Header("Invert")]
    public bool inversionX = false;
    public bool inversionY = false;

    [Header("Smooth Movement")]
    public bool smooth = true;
    [Tooltip("Smooth speed")]
    [Range(1f, 30f)]
    public float moveSpeed = 8;
}
