using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Camera/Config")]
public class CameraConfig : ScriptableObject
{
    public enum InversionX { Disabled = 0, Enabled = 1 };
    public enum InversionY { Disabled = 0, Enabled = 1 };
    public enum Smooth { Disabled = 0, Enabled = 1 };

    [Header("General")]
    [Tooltip("Mouse sensitivity")]
    public float sensitivity = 2;
    [Tooltip("Distance to observations object")]
    public float distance = 5;
    [Tooltip("Height of camera")]
    public float height = 2.3f;

    [Header("Over the shoulder")]
    public float offsetPosition;

    [Header("Clamp angle")]
    public float minY = 45f;
    public float maxY = 45f;

    [Header("Invert")]
    public InversionX inversionX = InversionX.Disabled;
    public InversionY inversionY = InversionY.Disabled;

    [Header("Smooth Movement")]
    public Smooth smooth = Smooth.Enabled;
    public float speed = 8;
}
