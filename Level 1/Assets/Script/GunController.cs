using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    [System.Serializable]
    public class Limit
    {
        public float MinAngle = 0f;
        public float MaxAngle = 360f;
    }

    public Transform target;
    public Limit Limits;
    public float initialForwardAngle = 0; // initial angle of your "gun barrel"
    public float maxRotationSpeed = 60;
    public float threshold = 4;
    public bool IsFlipped = false;

    bool CanRotate;


    private void Start()
    {
        angleToTarget = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        _currentAngle = angleToTarget;
        transform.eulerAngles = new Vector3(0, 0, _currentAngle - initialForwardAngle);
    }

    void Update()
    {
        if (CanRotate)
            RotateGradually2D();
    }
    public void RotateGradually2D()
    {
        angleToTarget = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        signToTarget = Mathf.Sign(angleToTarget - _currentAngle);
        if (Mathf.Abs(angleToTarget - _currentAngle) > threshold)
        {
            _currentAngle += signToTarget * maxRotationSpeed * Time.deltaTime;
        }
        else
        {
            _currentAngle = angleToTarget;
        }

        //transform.eulerAngles = new Vector3(0, 0, _currentAngle - initialForwardAngle);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(_currentAngle - initialForwardAngle, Limits.MinAngle, Limits.MaxAngle));
    }
    private float angleToTarget; // Destination angle
    private float _currentAngle = 0; // Current angle
    private float signToTarget;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.tag == "Player" && collider is BoxCollider2D)
            CanRotate = true;
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.tag == "Player" && collider is BoxCollider2D)
            CanRotate = false;
    }
}
