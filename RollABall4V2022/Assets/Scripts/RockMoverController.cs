using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RockMoverController : MonoBehaviour
{
    public float pushSpeed;
    
    public bool canBePushed;

    private Rigidbody _rigidbody;
    
    private bool _playerInTouch;

    private PlayerController _playerController;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_playerInTouch && Keyboard.current.eKey.isPressed)//Input.GetKey(KeyCode.E))
        {
            canBePushed = true;
        }
        else
        {
            canBePushed = false;
        }
    }

    private void FixedUpdate()
    {
        if (canBePushed && _playerController != null)
        {
            _rigidbody.isKinematic = false;
           // Vector3 pushDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
           Vector3 pushDirection = _playerController.MoveDirection;
           _rigidbody.velocity = pushDirection.normalized * pushSpeed;
        }
        else
        {
            _rigidbody.isKinematic = true;
            //_rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            _playerInTouch = true;
            _playerController = collisionInfo.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTouch = false;
            _playerController = null;
        }
    }
}
