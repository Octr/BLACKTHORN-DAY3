using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform modelTransform;
    public LayerMask groundLayer;
    public float acceleration = 5f;
    public float topSpeed = 15f;
    public float reverseSpeed = 5f;
    public AnimationCurve accelerationCurve;
    public float rotationSpeed = 20f;
    public float gravityAcceleration = 9.81f;
    public float maxGravity = 15f;
    public float jumpInitialForce = 3f;
    public float jumpForce = 3f;
    public float maxJumpDuration = 0.15f;
    public float maxExternalForcePerFrame = 10f;

    CharacterController characterController;
    Vector3 moveDirection;
    float horizontalInput;
    float verticalInput;
    bool jump = false;
    bool isJumping = false;
    bool isGrounded = false;
    Vector3 jumpDirection;
    float jumpTime = 0f;
    float gravity = 0f;
    Vector3 externalForce;
    public float currentSpeed;
    Vector3 lastVelocity;

    public bool DEBUG;

    public float GetSpeed()
    {
        return currentSpeed;
    }

    public Vector3 GetVelocity()
    {
        return lastVelocity;
    }

    public float GetCurrentSpeed01()
    {
        return Mathf.Clamp01(currentSpeed / topSpeed);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CollectInput();
    }

    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        if (isGrounded)
        {
            HandleInputGround();
        }
        else
        {
            HandleInputAir();
        }
        ApplyGravity();
        HandleJump();
        HandleExternalForce();
        Move();
    }

    void CollectInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    public void AddExternalForce(Vector3 force)
    {
        externalForce += force;
    }

    void HandleInputGround()
    {

        // Rotate the model left or right based on horizontal input
        if (horizontalInput != 0f)
        {
            float rotationAngle = horizontalInput * rotationSpeed * Time.fixedDeltaTime * Mathf.Clamp01(Mathf.Abs(currentSpeed - 25) / 50);
            if(currentSpeed < 0)
            {
                rotationAngle *= -1;
            }
            modelTransform.Rotate(Vector3.up, rotationAngle, Space.Self);
        }
        if (Mathf.Abs(verticalInput) > 0) {
            currentSpeed = Mathf.Clamp(currentSpeed + verticalInput * accelerationCurve.Evaluate(currentSpeed / topSpeed) * acceleration * Time.fixedDeltaTime, reverseSpeed, topSpeed);
            if (verticalInput < 0 && currentSpeed > 0)
            {
                // Apply Brakes
                currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.fixedDeltaTime * 2);
            }
        } else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.fixedDeltaTime);
        }
        moveDirection = modelTransform.forward * (currentSpeed * Time.fixedDeltaTime);
    }

    void HandleInputAir()
    {
        if (horizontalInput != 0f)
        {
            float rotationAngle = horizontalInput * rotationSpeed * Time.fixedDeltaTime;
            if (currentSpeed < 0)
            {
                rotationAngle *= -1;
            }
            modelTransform.Rotate(Vector3.up, rotationAngle, Space.Self);
        }
        transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.fixedDeltaTime * 5f);
        moveDirection = jumpDirection.normalized * currentSpeed * Time.fixedDeltaTime;
    }


    void ApplyGravity()
    {
        if (!isGrounded)
        {
            // Add the acceleration of gravity, scaled per second
            gravity = Mathf.Clamp(gravity + (gravityAcceleration * Time.fixedDeltaTime), 0, maxGravity);

            // Add the accumulated speed, scaled per second
            moveDirection.y -= gravity * Time.fixedDeltaTime;
        } else
        {
            gravity = 0;
        }
    }

    bool CanJump()
    {
        return !isJumping && isGrounded;
    }

    void HandleJump()
    {
        if (jump)
        {
            Debug.Log("Trying to jump");
            jump = false;

            if (CanJump())
            {
                isJumping = true;
                isGrounded = false;
                jumpTime = Time.time;
                jumpDirection = modelTransform.forward * moveDirection.magnitude;
                moveDirection = moveDirection + (Vector3.up * jumpInitialForce);
            }
        }
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            // Check if the jump button is still held within the max jump time
            if (Time.time - jumpTime < maxJumpDuration)
            {
                // Add additional force while the jump button is held
                moveDirection += Vector3.up * (jumpForce * Time.fixedDeltaTime);
            }
        }
    }

    void HandleExternalForce()
    {
        if(externalForce != Vector3.zero)
        {
            moveDirection += (externalForce);
            if(isGrounded && Vector3.Dot(externalForce, modelTransform.forward) > .5f)
            {
                if(externalForce.magnitude > currentSpeed)
                {
                    currentSpeed = Mathf.Clamp(externalForce.magnitude, currentSpeed, topSpeed);
                }
            }
            externalForce = Vector3.Lerp(externalForce, Vector3.zero, Time.fixedDeltaTime);
            if(externalForce.sqrMagnitude < 1)
            {
                externalForce = Vector3.zero;
            }
        }
    }

    void Move()
    {
        lastVelocity = moveDirection;
        characterController.Move(moveDirection * Time.fixedDeltaTime);
        moveDirection = Vector3.zero;
    }

    bool CheckGrounded()
    {
        // Perform a simple grid  pattern raycast to check if the player is grounded
        Vector3 raycastOrigin = transform.position + (Vector3.up * characterController.skinWidth);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = 1; y >= -1; y--)
            {
                RaycastHit hit;
                Vector3 raycastOffset = ((modelTransform.forward * y) + (modelTransform.right * x)).normalized * characterController.radius;
                if(Physics.Raycast(new Ray(raycastOrigin + raycastOffset, Vector3.down), out hit, .1f , groundLayer)) {
                    transform.up = Vector3.Lerp(transform.up, hit.normal, Time.fixedDeltaTime * 5);
                    return true;
                }
            }
        }
        if(isGrounded && !jump && !isJumping)
        {
            jumpDirection = modelTransform.forward;
        }
        return false;
    }
}
