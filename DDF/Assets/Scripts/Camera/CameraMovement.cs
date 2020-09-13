using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public CameraConfig config;

    private float rotationY;
    private int inversY, inversX;
    [SerializeField] private Transform player;

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
        if (player != null)
        {
            inversX = config.inversionX == CameraConfig.InversionX.Disabled ? 1 : -1;
            inversY = config.inversionY == CameraConfig.InversionY.Disabled ? -1 : 1;

            // Rotate camera around player
            transform.RotateAround(player.position, Vector3.up, Input.GetAxis("Mouse X") * config.sensitivity * inversX);

            Vector3 position = player.position - (transform.rotation * Vector3.forward * config.distance);
            position += transform.rotation * Vector3.right * config.offsetPosition; // horizontal offset
            position = new Vector3(position.x, player.position.y + config.height, position.z); // height offset
            position = PositionCorrection(player.position, position);

            // Rotate camera along Y axis
            rotationY += Input.GetAxis("Mouse Y") * config.sensitivity;
            rotationY = Mathf.Clamp(rotationY, -Mathf.Abs(config.minY), Mathf.Abs(config.maxY));
            transform.localEulerAngles = new Vector3(rotationY * inversY, transform.localEulerAngles.y, 0);

            // Smooth option
            if (config.smooth == CameraConfig.Smooth.Disabled)
            {
                transform.position = position;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, position, config.speed * Time.deltaTime);
            }
        }
    }
}
