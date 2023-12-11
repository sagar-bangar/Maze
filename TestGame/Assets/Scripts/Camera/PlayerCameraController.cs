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

    //Smooth trasitioning the camera parent object to character position so that cmaera looks like its following with damp
    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        transform.Rotate(Vector3.up * sensitivity * mouseX * Time.fixedDeltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, _playerTransform.position, ref velocity, smoothTime);
    }
}
