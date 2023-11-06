using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player s_Player; 

    CharacterController characterController;
    
    bool _gamePlaying = true;

    Vector3 _StartPos => GameManager.s_instance.SpawnPoint.position;
    Quaternion _StartRot => GameManager.s_instance.SpawnPoint.rotation;

    public float movementSpeed;
   // public float sprintFactor = 1.3f;
    public float playerHealth;

    private void Awake()
    {
        if (s_Player is null) s_Player = this;
        else Destroy(this);
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        GameManager.s_instance.onGameStateChange += SwitchGameState;

        mainCameraTransform = Camera.main.transform;

        GameManager.s_instance.onDeath += Respawn;
        GameManager.s_instance.onWin += Respawn;
    }

    void Update()
    {
        PauseGame();

        if (!_gamePlaying) return;
        
        MouseRotation();
        PlayerMovement();
        Crouch();
    }

    #region Movement
    public float gravity;
    public float jumpHeight;
    Vector3 verticalMovement = Vector3.zero;
    float movementX;
    float movementZ;

    void PlayerMovement()
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
    #endregion

    #region Camera Movement
    Transform mainCameraTransform;
    float verticalRotation;

    public float maxVerticalRotation;
    public float mouseSensitivity;

    float mouseX;
    float mouseY;
    void MouseRotation()
    {
        if(_gamePlaying == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            //Rotates the player characters body 
            transform.Rotate(Vector3.up, mouseX);

            //Tilts the main camera vertically
            verticalRotation -= mouseY; //if inverted vertical rotation, add mousY instead of subtract
            verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalRotation, maxVerticalRotation);
            mainCameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    #endregion

    #region Crouch
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale = new Vector3(1, 1f, 1);

    // Update is called once per frame
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))

        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
    }
    #endregion

    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_gamePlaying) GameManager.s_instance.PauseGame();
            else GameManager.s_instance.ResumeGame();
        }
    }

    public void SetRotation(float playerRotation, float cameraRotation)
    {
        transform.eulerAngles = new Vector3(0, playerRotation, 0);
        verticalRotation = cameraRotation;
    }

    void SwitchGameState(bool myisPlaying)
    {
        _gamePlaying = myisPlaying;
    }

    void Respawn()
    {
        characterController.enabled = false;
        transform.position = _StartPos;
        transform.rotation = _StartRot;
        characterController.enabled = true;
    }
}

