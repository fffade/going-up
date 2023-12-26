using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private Transform targetTransform;
    
    [SerializeField] private float followSpeed;
    
    // The minimum y value this camera MUST have
    // Used for player falling into fog
    [SerializeField] private float minYClamp;
    
    public Vector3 DesiredPosition { get; private set; }



    void Awake()
    {
        _transform = transform;
    }
    

    void Update()
    {
        DesiredPosition = new Vector3(_transform.position.x, Mathf.Max(targetTransform.position.y, minYClamp), _transform.position.z);

        Vector3 newPosition = Vector3.Lerp(_transform.position, DesiredPosition, followSpeed * Time.deltaTime);

        _transform.position = newPosition;
    }
}
