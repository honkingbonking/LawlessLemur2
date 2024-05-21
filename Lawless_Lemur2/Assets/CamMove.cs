using System.Collections;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform[] positions; // Array of transforms to move the camera between
    [SerializeField] ManageFlow flow;
    public float transitionSpeed = 1.0f; // Speed of transition
    private int currentPosIndex = 0; // Index of the current position
    private bool isTransitioning = false; // Flag to indicate if transition is in progress

    public void StartMove()
    {
        // Update to the next position index
        currentPosIndex = (currentPosIndex + 1) % positions.Length;
        // Start the transition coroutine
        StartCoroutine(MoveToPosition(positions[currentPosIndex]));
    }
    private IEnumerator MoveToPosition(Transform targetTransform)
    {
        isTransitioning = true; // Set transitioning flag
        Vector3 startPosition = transform.position; // Get current position
        Quaternion startRotation = transform.rotation; // Get current rotation
        Vector3 targetPosition = targetTransform.position; // Get target position
        Quaternion targetRotation = targetTransform.rotation; // Get target rotation
        float time = 0;

        // Interpolate position and rotation over time
        while (time < 1)
        {
            time += Time.deltaTime * transitionSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, time);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time);
            yield return null;
        }

        // Ensure final position and rotation are exactly the target's
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        isTransitioning = false; // Reset transitioning flag
        flow.Next();
    }
}
