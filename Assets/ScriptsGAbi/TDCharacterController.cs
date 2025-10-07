using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TDCharacterController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float gridSize = 1f;
        
        private Vector3 targetPosition;
        private bool isMoving = false;

        private Animator anim;

        private void Start()
        {
            targetPosition = transform.position;
            anim = GetComponent<Animator>();
            anim.SetFloat("LastX", 0);
            anim.SetFloat("LastY", -1);
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
                anim.SetFloat("LastX", 0);
                anim.SetFloat("LastY", 1);
                targetPosition += Vector3.up * gridSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                anim.SetFloat("LastX", 0);
                anim.SetFloat("LastY", -1);
                targetPosition += Vector3.down * gridSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                anim.SetFloat("LastX", -1);
                anim.SetFloat("LastY", 0);
                targetPosition += Vector3.left * gridSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                anim.SetFloat("LastX", 1);
                anim.SetFloat("LastY", 0);
                targetPosition += Vector3.right * gridSize;
                isMoving = true;
            }
        }

        private void MoveToTarget()
        {
            if (!isMoving) return;

            anim.SetBool("IsMoving", true);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                isMoving = false;
                anim.SetBool("IsMoving", false);
            }
        }
    }
}