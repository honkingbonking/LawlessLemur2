using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] Transform[] positions; // Array of target positions and rotations
    public float lerpSpeed = 1.0f; // Speed of lerp

    private int currentTargetIndex = 0; // Index of the current target position
    private Animator anim;
    private bool isLerping = false;

    [SerializeField] CamMove cam;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void LemurMove()
    {
        if (positions.Length == 0) return;

        if (!isLerping)
        {
            currentTargetIndex++;
            if (currentTargetIndex < positions.Length)
            {
                StartCoroutine(LerpToPosition(positions[currentTargetIndex]));
            }
            else
            {
                // Reset the index to loop the positions
                currentTargetIndex = 0;
                StartCoroutine(LerpToPosition(positions[currentTargetIndex]));
            }
        }
    }

    private IEnumerator LerpToPosition(Transform targetTransform)
    {
        isLerping = true;
        anim.SetBool("Walk", isLerping);
        cam.StartMove();

        float timeElapsed = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 targetPosition = targetTransform.position;
        Quaternion targetRotation = targetTransform.rotation;

        while (timeElapsed < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed);
            timeElapsed += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        // Ensure the final position and rotation are exactly the target's
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        isLerping = false;
        anim.SetBool("Walk", isLerping);
    }
}
