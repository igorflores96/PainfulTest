using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    void Update()
    {
        this.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -1.0f);
    }
}
