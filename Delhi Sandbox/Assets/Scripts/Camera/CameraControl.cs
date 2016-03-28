using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dampTime = 0.2f;
    public float screenEdgeBuffer = 4f;
    public float minSize = 6.5f;
    public bool followPlayer = true;
    public bool useStreetView = true;
    [HideInInspector] public Transform[] targets;
    public Camera overheadMapCamera;

    private Camera m_Camera;
    private float zoomSpeed;
    private Vector3 moveVelocity;
    private Vector3 desiredPosition;


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;

            averagePos += targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;

        desiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        float size = 0f;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
        }

        size += screenEdgeBuffer;

        size = Mathf.Max(size, minSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        if (!followPlayer)
        {
            transform.position = desiredPosition;
        }

        m_Camera.orthographicSize = FindRequiredSize();
    }

    public void SetPlayerTransform (Transform playerTransform)
    {
        if (followPlayer)
        {
            transform.parent = playerTransform;
            if (useStreetView)
            {
                Vector3 position = transform.position;
                Quaternion rotation = new Quaternion (0, 0, 0, 0);
                position.y = 5;
                position.z = -20;
                m_Camera.transform.position = position;
                m_Camera.transform.rotation = rotation;

                Quaternion overheadRotation = new Quaternion(90, 0, 0, 0);
                overheadMapCamera.transform.rotation = overheadRotation;
            }
        }
    }
}
