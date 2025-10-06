using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TDCharacterController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float gridSize = 1f;
        
        private Vector3 targetPosition;
        private bool isMoving = false;

        private void Start()
        {
            targetPosition = transform.position;
        }

        private void Update()
        {
            if (isMoving)
            {
                MoveToTarget();
                return;
            }
            
            HandleInput();
            MoveToTarget();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                targetPosition += Vector3.up * gridSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                targetPosition += Vector3.down * gridSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                targetPosition += Vector3.left * gridSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                targetPosition += Vector3.right * gridSize;
                isMoving = true;
            }
        }

        private void MoveToTarget()
        {
            if (!isMoving) return;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                isMoving = false; 
            }
        }
    }
}