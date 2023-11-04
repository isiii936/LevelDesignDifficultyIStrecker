using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    public float movementSpeed;
   // public float sprintFactor = 1.3f;
    public float playerHealth;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public float gravity;
    public float jumpHeight;
    Vector3 verticalMovement = Vector3.zero;
    float movementX;
    float movementZ;

    void Update()
    {
        if(GameController.instance.gamePlaying == true)
        {
            movementX = Input.GetAxis("Horizontal");
            movementZ = Input.GetAxis("Vertical");

            Vector3 movementVector = transform.right * movementX + transform.forward * movementZ;
            if (movementVector.magnitude > 1) movementVector = movementVector.normalized;
            movementVector = movementVector * movementSpeed * Time.deltaTime;

            if (characterController.isGrounded)
            {
                verticalMovement.y = -2f;
            }
            else
            {
                verticalMovement.y -= gravity * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Space) && characterController.isGrounded)
            {
                verticalMovement.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
            }

            characterController.Move(movementVector + (verticalMovement * Time.deltaTime));
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GameController.instance.gamePlaying == true)
        {
            GameController.instance.PauseGame();
        }
    }
}

