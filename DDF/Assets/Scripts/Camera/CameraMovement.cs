using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask mask;
    [SerializeField] private CameraConfig config;

    private float rotationX;
    private int InverseX => config.inversionX ? -1 : 1;
    private int InverseY => config.inversionY ? 1 : -1;

    private void LateUpdate()
    {
        // Offset rotation include right and height offsets
        var offsetPosition = target.position + (Vector3.up * config.offsetHeight);
        offsetPosition += target.right * config.offsetRight;

        // Rotate camera around player
        var deltaHor = Input.GetAxis("Mouse X") * config.rotationSpeed * InverseX;
        var deltaVer = ClampVerticalAngle(Input.GetAxis("Mouse Y") * config.rotationSpeed * InverseY);

        transform.RotateAround(offsetPosition, Vector3.up, deltaHor);
        transform.RotateAround(offsetPosition, transform.right, deltaVer);

        // Apply distance offset to rotation offset
        var position = offsetPosition - (transform.forward * config.offsetDistance);
        position = DistanceCorrection(offsetPosition, position);

        // Smooth option
        if (config.smooth)
        {
            transform.position = position;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, position, config.moveSpeed * Time.deltaTime);
        }
    }

    // Constrain the delta of the vertical angle to the upper and lower boundaries
    private float ClampVerticalAngle(float angle)
    {
        var upBound = config.upAngle - rotationX;
        var downBound = -config.downAngle - rotationX;

        var result = Mathf.Clamp(angle, downBound, upBound);
        rotationX += result;

        return result;
    }

    // Check for collider in the path of ray from camera to player
    private Vector3 DistanceCorrection(Vector3 target, Vector3 position)
    {
        if (Physics.Linecast(target, position, out var hit, mask))
        {
            float tempDistance = Vector3.Distance(target, hit.point) * 0.8f;
            position = target - (transform.forward * tempDistance);
        }

        return position;
    }
}
