using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidbody;
    public float rotationSpeed = 10f;
    public float speed = 2f;
    public Transform groundCheckerTransform;
    public LayerMask notPlayerMask;
    public float jumpForce = 2f;
    private bool isGrounded;

    private CapsuleCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 directionVector = new Vector3(-v, 0, h);
        if (directionVector.magnitude > Mathf.Abs(0.05f))
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * rotationSpeed);

        animator.SetFloat("Speed", Vector3.ClampMagnitude(directionVector, 1).magnitude);
        Vector3 moveDir = Vector3.ClampMagnitude(directionVector, 1) * speed;
        rigidbody.velocity = new Vector3(moveDir.x, rigidbody.velocity.y, moveDir.z);
        rigidbody.angularVelocity = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Sprint();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            UnSprint();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            UnCrouch();
        }

        if (Physics.CheckSphere(groundCheckerTransform.position, 0.2f, notPlayerMask))
        {
            animator.SetBool("IsInAir", false);
            isGrounded = true;

        }
        else
        {
            animator.SetBool("IsInAir", true);
            isGrounded = false;
        }

    }

    private void UnCrouch()
    {
        animator.SetBool("isCrouching", false);
        speed = 4.5f;
        _collider.height = 3.8f;
        _collider.center = new Vector3(_collider.center.x, 1.89661f, _collider.center.z);
    }

    private void Crouch()
    {
        if (isGrounded)
        {
            animator.SetBool("isCrouching", true);
            speed = 2.3f;
            _collider.height = 2.8f;
            _collider.center = new Vector3(_collider.center.x, 1.429439f, _collider.center.z);
        }
    }

    private void Sprint()
    {
        speed = 7.5f;
        animator.SetBool("IsRunning", true);
    }
    private void UnSprint()
    {
        speed = 3.5f;
        animator.SetBool("IsRunning", false);
    }

    void Jump()
    {
        if (animator.GetBool("isCrouching")) return;

        if (isGrounded)
        {
            animator.SetTrigger("Jump");
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("Did not find ground layer");
        }
    }
}
