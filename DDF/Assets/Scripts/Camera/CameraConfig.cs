using UnityEngine;

[CreateAssetMenu(menuName ="Camera/Config")]
public class CameraConfig : ScriptableObject
{
    public enum InversionX { Disabled = 0, Enabled = 1 };
    public enum InversionY { Disabled = 0, Enabled = 1 };
    public enum Smooth { Disabled = 0, Enabled = 1 };

    [Header("General")]
    [Tooltip("Mouse sensitivity")]
    public float rotSpeed = 2;
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
    public InversionX inversionX = InversionX.Disabled;
    public InversionY inversionY = InversionY.Disabled;

    [Header("Smooth Movement")]
    public Smooth smooth = Smooth.Enabled;
    [Tooltip("Smooth speed")]
    public float movSpeed = 8;
}
