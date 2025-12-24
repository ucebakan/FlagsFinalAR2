using UnityEngine;

public class FlagInteraction : MonoBehaviour
{
    [Header("Settings")]
    public string targetLandmarkName;
    public static bool isAnyFlagBeingDragged = false;

    private bool isDragging = false;
    private bool isMatched = false;
    private float hoverTimer = 0f;
    private float requiredTime = 1.0f;
    private Transform fingerTransform;

    void Start()
    {
        // Physics Check: Ensure flag has a Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; // Set to kinematic to move via script
    }

    void Update()
    {
        if (isMatched) return;

        if (isDragging && fingerTransform != null)
        {
            // Follow finger smoothly
            transform.position = Vector3.Lerp(transform.position, fingerTransform.position, Time.deltaTime * 40f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isMatched) return;

        // 1. GRAB LOGIC
        if (other.CompareTag("Finger") && !isDragging && !isAnyFlagBeingDragged)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= requiredTime)
            {
                isDragging = true;
                isAnyFlagBeingDragged = true;
                fingerTransform = other.transform;
                Debug.Log("<color=cyan>Flag Grabbed: </color>" + gameObject.name);
            }
        }

        // 2. MATCHING LOGIC
        if (isDragging && other.CompareTag("Landmark"))
        {
            // LOG EVERYTHING: See what the flag is touching in real-time
            Debug.Log("<color=orange>Touching Landmark: </color>" + other.gameObject.name);

            // AUTOMATIC DEPTH ALIGNMENT: 
            // This forces the flag to the same Z depth as the landmark when they overlap
            Vector3 pos = transform.position;
            pos.z = other.transform.position.z;
            transform.position = pos;

            if (other.gameObject.name == targetLandmarkName)
            {
                MatchFound(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finger") && !isDragging)
        {
            hoverTimer = 0f;
        }
    }

    void MatchFound(Transform target)
    {
        isDragging = false;
        isAnyFlagBeingDragged = false;
        isMatched = true;

        // Final snap and parent
        transform.position = target.position;
        transform.SetParent(target);

        Debug.Log("<color=green>MATCH SUCCESS: </color>" + gameObject.name);
    }
}