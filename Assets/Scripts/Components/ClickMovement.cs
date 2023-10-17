using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Components
{
    public class ClickMovement : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;
        public float speed = 3.5f;
        private InputAction mouseClickAction;
        private NavMeshAgent navMeshAgent;

        void Awake()
        {
            mouseClickAction = inputActionAsset.FindAction("MouseClick");
            mouseClickAction.performed += OnMouseClick;
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = speed;
        }

        void OnEnable()
        {
            mouseClickAction.Enable();
        }

        void OnDisable()
        {
            mouseClickAction.Disable();
        }

        void OnMouseClick(InputAction.CallbackContext context)
        {
            if (Camera.main == null) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                navMeshAgent.SetDestination(hit.point);
            }
        }
    }
}