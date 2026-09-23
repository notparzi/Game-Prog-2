using UnityEngine;

public class OrbitalTarget : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float movementSpeed = 5f;

    public TurretTracker turret;

    private float timer = 0f;

    private Vector3 positionA;
    private Vector3 positionB;

    public int scoreOnHit = 10;
    public int dangerZonePenalty = 5;

    private bool movingToB = true;
    private bool inSafeZone = false;
    private bool clickedInSafeZone = false;
    private bool attemptResolved = false;


    void Start()
    {
        positionA = new Vector3
        (
            transform.position.x,
            transform.position.y,
            pointA.position.z
        );

        positionB = new Vector3
        (
            transform.position.x,
            transform.position.y,
            pointB.position.z
        );


        timer = Mathf.InverseLerp
        (
            positionA.z,
            positionB.z,
            transform.position.z
        );

        timer = Mathf.Clamp01(timer);


        if (timer >= 1f)
        {
            movingToB = false;
        }
        else
        {
            movingToB = true;
        }
    }


    void Update()
    {
        float distance = Vector3.Distance(positionA, positionB);


        if (distance > 0.001f)
        {
            float timerSpeed = movementSpeed / distance;


            if (movingToB)
            {
                timer += timerSpeed * Time.deltaTime;
            }
            else
            {
                timer -= timerSpeed * Time.deltaTime;
            }

            timer = Mathf.Clamp01(timer);

            transform.position = Vector3.Lerp
            (
                positionA,
                positionB,
                timer
            );

            if (timer >= 1f)
            {
                timer = 1f;
                movingToB = false;
            }

            else if (timer <= 0f)
            {
                timer = 0f;
                movingToB = true;
            }
        }
    }

    void OnMouseDown()
    {
        CheckForClick();
    }

    void CheckForClick()
    {
        if (inSafeZone && !clickedInSafeZone && !attemptResolved)
        {
            GameManager.Instance.AddScore(scoreOnHit);

            clickedInSafeZone = true;
            attemptResolved = true;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            inSafeZone = true;

            clickedInSafeZone = false;
            attemptResolved = false;

            if (turret != null)
            {
                turret.RegisterTarget(transform);
            }
        }

        else if (other.CompareTag("DangerZone"))
        {
            if (!clickedInSafeZone && !attemptResolved)
            {
                GameManager.Instance.AddScore(-dangerZonePenalty);
                attemptResolved = true;
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            inSafeZone = false;

            if (turret != null)
            {
                turret.UnregisterTarget(transform);
            }
        }
    }

    void OnEnable()
    {
        Debug.Log(name + " activated.");
    }

    void OnDisable()
    {
        Debug.Log(name + " deactivated/removed.");
    }
}