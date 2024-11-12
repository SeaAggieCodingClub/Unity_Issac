using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Movement : MonoBehaviour
{
    

    [Header("Movement")]
    
    bool readyToJump = true;
    public float speed;
    public float jumpForce;
    public float groundDrag = 0.5f;
    
    float jumpCoolDown = 0.1f;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    bool jumpInput;

    Vector3 moveDirection;

    Rigidbody rb;

// Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

//recives player inputs
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        
    }

//moves the player
    private void Moveplayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        transform.rotation = orientation.rotation;
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

//runs a frame before update
    private void FixedUpdate()
    {
        Moveplayer();
    }


// runs every frame
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);


        MyInput();
        speedCap();


        //player jump
        if (jumpInput && readyToJump && grounded)
        {
            readyToJump = true;
            playerJump();
            Invoke(nameof(resetPlayerJump), jumpCoolDown);
        }

        //drag
        if (grounded)
            rb.drag = groundDrag;
        else 
            rb.drag = 0;
    }

//caps the player velocity
    private void speedCap() 
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void playerJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f,rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetPlayerJump()
    {
            readyToJump = true;
    }
}   

