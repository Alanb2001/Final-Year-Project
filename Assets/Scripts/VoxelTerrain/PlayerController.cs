using UnityEngine;

namespace VoxelTerrain
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform playerCamera;
        [SerializeField] private float mouseSensitivity = 3.5f;
        [SerializeField] private float walkSpeed = 6.0f;
        [SerializeField] private float gravity = -13.0f;
        [SerializeField][Range(0.0f, 0.5f)] private float moveSmoothTime = 0.3f;
        [SerializeField][Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.03f;
        [SerializeField] private float jumpForce = 8.0f;
        
        [SerializeField] private bool lockCursor = true;
        [SerializeField] private bool canJump = true;

        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        
        private float _cameraPitch;
        private float _velocityY;
        private CharacterController _controller;

        private Camera _camera;
        
        private bool ShouldJump => Input.GetKeyDown(jumpKey);  
        
        private Vector2 _currentDir = Vector2.zero;
        private Vector2 _currentDirVelocity = Vector2.zero;
       
        private Vector2 _currentMouseDelta = Vector2.zero;
        private Vector2 _currentMouseDeltaVelocity = Vector2.zero;
        
        private void Start()
        {
            _camera = Camera.main;
            _controller = GetComponent<CharacterController>();
            
            if (!lockCursor) return;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            UpdateMouseLook();
            UpdateMovement();
            RemoveBlock();
        }

        private void UpdateMouseLook()
        {
            var targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetMouseDelta, ref _currentMouseDeltaVelocity, mouseSmoothTime);
            
            _cameraPitch -= _currentMouseDelta.y * mouseSensitivity;

            _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

            playerCamera.localEulerAngles = Vector3.right * _cameraPitch;
            
            transform.Rotate(Vector3.up * _currentMouseDelta.x * mouseSensitivity);
        }

        private void UpdateMovement()
        {
            var targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetDir.Normalize();

            _currentDir = Vector2.SmoothDamp(_currentDir, targetDir, ref _currentDirVelocity, moveSmoothTime);

            if (_controller.isGrounded)
            {
                _velocityY = 0.0f;
                
                if (canJump && ShouldJump)
                {
                    _velocityY = jumpForce;
                }
            }

            _velocityY += gravity * Time.deltaTime;
            
            var velocity = (transform.forward * _currentDir.y + transform.right * _currentDir.x) * walkSpeed + Vector3.up * _velocityY;
            
            _controller.Move(velocity * Time.deltaTime);
        }

        private void RemoveBlock()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, 6))
                {
                    EditTerrain.SetBlock(hit, new BlockAir());
                }
            }
        }
    }
}
