using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    public Vector3 offset = new Vector3(1.65f, 0f, -10f);
    public float smoothFactor;
    Vector3 targetPosition;
    public GameManager gmScript;

    private void Start() {

    }
    void FixedUpdate() {
        if(!IsOwner) return;
            targetPosition = transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
            transform.position = smoothedPosition;
        
        
    }

    
}
