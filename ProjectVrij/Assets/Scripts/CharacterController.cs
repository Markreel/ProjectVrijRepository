using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Settings: ")]
    [SerializeField] private float maxVelocity = 10;
    [SerializeField] private float accelerationRate = 3;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    { 

    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float _hor = Input.GetAxis("Horizontal");
        print(_hor);
        //float _vert = Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float _newHor = Mathf.Clamp(rb.velocity.x + _hor * accelerationRate, -maxVelocity, maxVelocity);
            rb.velocity = new Vector3(_newHor, rb.velocity.y, rb.velocity.z);
        }

        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            _hor = 0;
        }
    }
}
