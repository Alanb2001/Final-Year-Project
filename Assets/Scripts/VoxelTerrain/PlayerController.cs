using UnityEngine;

namespace VoxelTerrain
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform playerCamera = null;
        [SerializeField] private float mouseSensitivity = 3.5f;
        [SerializeField] private float walkSpeed = 6.0f;
        [SerializeField] private float gravity = -13.0f;
        [SerializeField][Range(0.0f, 0.5f)] private float moveSmoothTime = 0.3f;
        [SerializeField][Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.03f;
        
        [SerializeField] private bool lockCursor = true;
        
        private float cameraPitch = 0.0f;
        private float velocityY = 0.0f;
        private CharacterController controller = null;
        
        Vector2 currentDir = Vector2.zero;
        Vector2 currentDirVelocity = Vector2.zero;
        
        Vector2 currentMouseDelta = Vector2.zero;
        Vector2 currentMouseDeltaVelocity = Vector2.zero;
        
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            UpdateMouseLook();
            UpdateMovement();
                
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                {
                    EditTerrain.SetBlock(hit, new BlockAir());
                }
            }
        }

        void UpdateMouseLook()
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
            
            cameraPitch -= currentMouseDelta.y * mouseSensitivity;

            cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

            playerCamera.localEulerAngles = Vector3.right * cameraPitch;
            
            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        }

        void UpdateMovement()
        {
            Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetDir.Normalize();

            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

            if (controller.isGrounded)
            {
                velocityY = 0.0f;
            }

            velocityY += gravity * Time.deltaTime;
            
            Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}
