using UnityEngine;

public class PlayerCameraController  : MonoBehaviour
{
    // Reference
    public Transform _playerTransform;

    // Parameters
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public float sensitivity;

    void Start()
    {
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        /*// Calculate the target position based on the player's position with the offset
        Vector3 targetPosition = _playerTransform.position + offset;

        //Smoothly interpolate between the current position and the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        transform.LookAt(_camOrientationTransform);*/

        float mouseX = Input.GetAxisRaw("Mouse X");
        transform.Rotate(Vector3.up * sensitivity * mouseX * Time.fixedDeltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, _playerTransform.position, ref velocity, smoothTime);
    }
}
