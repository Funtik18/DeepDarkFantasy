using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CameraConfig config;

    private float rotationX;
    private int InverseX => config.inversionX ? -1 : 1;
    private int InverseY => config.inversionY ? 1 : -1;

    private void LateUpdate()
    {
        // Offset rotation include right and height offsets
        var offsetRotPos = target.position + (Vector3.up * config.offsetHeight);
        offsetRotPos += target.right * config.offsetRight;

        // Rotate camera around player
        var deltaHor = Input.GetAxis("Mouse X") * config.rotationSpeed * InverseX;
        var deltaVer = ClampVerticalAngle(Input.GetAxis("Mouse Y") * config.rotationSpeed * InverseY);

        transform.RotateAround(offsetRotPos, Vector3.up, deltaHor);
        transform.RotateAround(offsetRotPos, transform.right, deltaVer);

        // Apply distance offset to rotation offset
        var pos = offsetRotPos - (transform.forward * config.offsetDistance);

        // Smooth option
        if (config.smooth)
        {
            transform.position = pos;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, pos, config.moveSpeed * Time.deltaTime);
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
    private Vector3 PositionCorrection(Vector3 target, Vector3 position)
    {
        if (Physics.Linecast(target, position, out var hit))
        {
            float tempDistance = Vector3.Distance(target, hit.point);
            Vector3 pos = target - (transform.rotation * Vector3.forward * tempDistance);
            position = new Vector3(pos.x, position.y, pos.z);
        }

        return position;
    }
}
