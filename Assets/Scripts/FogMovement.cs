using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMovement : MonoBehaviour
{
    private Transform _transform;
    
    
    public bool isMoving = true;

    [SerializeField] private float moveSpeed;

    [SerializeField] private float startPositionY;


    void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        if (isMoving)
        {
            _transform.position = new Vector3(
                _transform.position.x,
                _transform.position.y + moveSpeed * Time.deltaTime,
                _transform.position.z
            );
        }
    }

    public void ResetPosition()
    {
        _transform.position = new Vector3(_transform.position.x, startPositionY, _transform.position.z);
    }
}
