using Inputs;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    const float maxDistance = 10;
    [Header("Setup")]
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject feet;

    [Header("Movement")]
    [SerializeField] float minJumpDistance = 0.1f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float dash = 10.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float speedMultiplier = 2.0f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimeCounter;
    private RaycastHit hit;
    private FezManager.FacingDirection facingDirection;
    private float degree = 0;
    private bool isDashInput = false;
    private Vector3 currentMovement;
    public bool isJumping = false;

    private float timeElapsed;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>(); ;
        if (feet == null)
            feet = GetComponent<GameObject>();
    }
    private void Update()
    {
        MainMovement();
    }

    private void MainMovement()
    {
        if (IsGrounded(out hit))
        {
            coyoteTimeCounter = 0;
        }
        else
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        transform.Translate(speed * Time.deltaTime * currentMovement);
    }

    public void OnJump(InputValue input)
    {
        if (coyoteTimeCounter >= 0f)
        {
            StartCoroutine(JumpCoroutine());
            StopCoroutine(JumpCoroutine());
            StartCoroutine(JumpCooldown());
            StopCoroutine(JumpCooldown());
        }
    }

    private IEnumerator JumpCoroutine()
    {
        if (!feet)
            yield break;


        while (true)
        {
            if (IsGrounded(out hit))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = true;
            }
            else if (coyoteTimeCounter <= coyoteTime && Physics.Raycast(feet.transform.position - currentMovement, Vector3.down, out hit, maxDistance))
            {
                isJumping = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                coyoteTimeCounter = 0;
            }
            yield break;
        }        
    }

    private bool IsGrounded(out RaycastHit hit)
    {
        isJumping = false;
        return Physics.Raycast(feet.transform.position, Vector3.down, out hit, maxDistance) && hit.distance <= minJumpDistance;
    }

    public void OnMove(InputValue input)
    {
        var movement = input.Get<Vector2>();
        currentMovement.x = movement.x;
        currentMovement.z = movement.y;
    }

    public void OnSprint(InputValue input)
    {
        if (input.isPressed)
        {
            speed *= speedMultiplier;
        }
        else
        {
            speed /= speedMultiplier;
        }
    }

    public void OnDash(InputValue input)
    {
        isDashInput = true;
    }

    private void FixedUpdate()
    {

        if (isDashInput)
        {
            rb.AddForce(currentMovement * dash, ForceMode.Impulse);
            isDashInput = false;
        }
    }

    private void UpdateToFacingDirection(FezManager.FacingDirection newDirection, float angle)
    {
        facingDirection = newDirection;
        degree = angle;
    }

    private void OnRotateCameraRight()
    {
        transform.Rotate(transform.up, -90);
    }

    private void OnRotateCameraLeft()
    {
        transform.Rotate(transform.up, 90);
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.8f);
        isJumping = false;
    }
}