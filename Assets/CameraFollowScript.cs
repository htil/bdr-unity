using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {
    public Transform target;
    public float followSpeed = 10f;
    public float rotationSpeed = 3f;

    float distance;
    Vector3 position;
    Vector3 newPos;
    Quaternion rotation;
    Quaternion newRot;

    void Start() {
        distance = transform.position.y - target.position.y;
        position = new Vector3(target.position.x, target.position.y + distance, target.position.z);
        rotation = Quaternion.Euler(new Vector3(20, target.rotation.eulerAngles.y, 0f));
    }
    
    void FixedUpdate() {
        newPos = target.position;
        newPos.y += distance;
        newRot = Quaternion.Euler(new Vector3(20, target.rotation.eulerAngles.y, 0f));
        position = Vector3.Lerp(position, newPos, followSpeed * Time.deltaTime);
        rotation = Quaternion.Lerp(rotation, newRot, rotationSpeed * Time.deltaTime);
        transform.position = position;
        transform.rotation = rotation;
    }
}
