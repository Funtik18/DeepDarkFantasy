using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CameraConfig config;

    private float rotationY;
    
    private int InverseX => config.inversionX ? -1 : 1;
    private int InverseY => config.inversionY ? 1 : -1;

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
        // Offset rotation include right and height offsets
        var offsetRotPos = target.position + (Vector3.up * config.offsetHeight);
        offsetRotPos += target.right * config.offsetRight;

        // Rotate camera around player
        transform.RotateAround(offsetRotPos, Vector3.up, Input.GetAxis("Mouse X") * config.rotationSpeed * InverseX);
        transform.RotateAround(offsetRotPos, transform.right, Input.GetAxis("Mouse Y") * config.rotationSpeed * InverseY);

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
}
