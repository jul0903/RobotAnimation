using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;   
    public float jumpForce = 5f;  
    public float groundDistance = 1.1f;
    public float rotationSpeed = 10;
    public LayerMask groundLayer; 

    private Rigidbody rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        

        if(movement != Vector3.zero)
        {
            Debug.Log("esroy moviendome");
            anim.SetBool("IsWalking", true);
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("esroy quieto");
            anim.SetBool("IsWalking", false);

        }

        
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);
    }

}
