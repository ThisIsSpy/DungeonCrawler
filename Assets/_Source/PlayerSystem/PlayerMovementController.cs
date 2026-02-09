using DG.Tweening;
using NodeSystem;
using System.Collections;
using UnityEngine;

namespace PlayerSystem 
{
    public class PlayerMovementController : MonoBehaviour
    {
        private static readonly WaitForSeconds shortWait = new(0.25f);
        [SerializeField] private Node startingNode;
        [SerializeField] private Node currentNode;
        private bool disableMovement = false;

        void Start()
        {
            transform.position = startingNode.transform.position;
            currentNode = startingNode;
        }

        void Update()
        {
            if (disableMovement) return;
            if (Input.GetKeyDown(KeyCode.W)) CheckForNodes(transform.forward);
            if (Input.GetKeyDown(KeyCode.S)) CheckForNodes(-transform.forward);
            if (Input.GetKeyDown(KeyCode.A)) CheckForNodes(-transform.right);
            if (Input.GetKeyDown(KeyCode.D)) CheckForNodes(transform.right);
            if (Input.GetKeyDown(KeyCode.Q)) transform.DOLocalRotate(new(0,transform.eulerAngles.y - 90), 0.25f);
            if (Input.GetKeyDown(KeyCode.E)) transform.DOLocalRotate(new(0, transform.eulerAngles.y + 90), 0.25f);
        }

        private void CheckForNodes(Vector3 direction)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, direction);
            foreach (RaycastHit hit in hits)
            {
                if(hit.distance <= 3.05f && hit.collider.TryGetComponent(out Node node))
                {
                    StartCoroutine(MovementCoroutine(node));
                    return;
                }
            }
        }

        private IEnumerator MovementCoroutine(Node node)
        {
            disableMovement = true;

            if (node.IsDeadEnd)
            {
                transform.DOMove(node.transform.position, 0.25f);
                yield return shortWait;
                transform.DOMove(currentNode.transform.position, 0.25f);
            }
            else 
            {
                transform.DOMove(node.transform.position, 0.5f);
                currentNode = node;
            } 
            disableMovement = false;
        }
    }
}