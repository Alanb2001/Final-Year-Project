using UnityEngine;

namespace VoxelTerrain
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform playerCamera = null;
        [SerializeField] private float mouseSensitivity = 3.5f;
        private void Update()
        {
            UpdateMouseLook();
            
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
            Vector2 mousDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            
            transform.Rotate(Vector3.up * mousDelta.x * mouseSensitivity);
        }
    }
}
