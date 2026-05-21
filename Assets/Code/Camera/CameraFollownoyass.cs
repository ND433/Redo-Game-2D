using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    [Tooltip("Tag to follow")]
    public string playerTag = "Player";

    [Tooltip("if u want slight offset")]
    public Vector3 offset = new Vector3(0f, 0f, 0f); // Offset is now mainly for X

    [Tooltip("Camera move with object speed")]
    [Range(1f, 10f)]
    public float smoothSpeed = 5f;

    // Store fixed positions to prevent movement on these axes
    private float fixedZDepth;
    private float fixedYHeight;

    void Start()
    {
        // Store the initial Y and Z positions of the camera
        fixedZDepth = transform.position.z;
        fixedYHeight = transform.position.y;

        GameObject playerObject = GameObject.FindWithTag(playerTag);

        if (playerObject != null)
        {
            target = playerObject.transform;
            Debug.Log("CameraFollow: Found thing with tag '" + playerTag + "'.");
        }
        else
        {
            Debug.LogError("CameraFollow: Could not find the thing '" + playerTag + "'. check da settings!");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // We use target.position.x for horizontal following,
        // but we use our stored 'fixed' values for Y and Z.
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            fixedYHeight + offset.y, // Uses the starting Y height + any manual offset
            fixedZDepth
        );

        // Smoothly move camera towards desired position
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }
}