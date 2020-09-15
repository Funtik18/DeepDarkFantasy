using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CameraConfig config;

    private float rotationY;
    private int inverseY, inverseX;

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

    private void LateUpdate()
    {
        inverseX = config.inversionX == CameraConfig.InversionX.Disabled ? 1 : -1;
        inverseY = config.inversionY == CameraConfig.InversionY.Disabled ? -1 : 1;

        // Offset rotation include right and height offsets
        var offsetRotPos = target.position + new Vector3(config.offsetRight, config.offsetHeight);

        // Rotate camera around player
        transform.RotateAround(offsetRotPos, Vector3.up, Input.GetAxis("Mouse X") * config.rotSpeed * inverseX);
        transform.RotateAround(offsetRotPos, transform.right, Input.GetAxis("Mouse Y") * config.rotSpeed * inverseY);

        // Apply distance offset to rotation offset
        var pos = offsetRotPos - (transform.forward * config.offsetDistance);

        // Smooth option
        if (config.smooth == CameraConfig.Smooth.Disabled)
        {
            transform.position = pos;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, pos, config.movSpeed * Time.deltaTime);
        }
    }
}
