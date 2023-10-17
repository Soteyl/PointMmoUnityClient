using UnityEngine;

namespace Components
{
    public class TargetCamera : MonoBehaviour
    {
        public Transform target; 
        public Vector3 distance = new Vector3(0.0f, 10.0f, -10.0f);  
        public Vector3 offset = new Vector3(0.0f, 5.0f, 0.0f); 
        public Vector3 rotation = new Vector3(45.0f, 0.0f, 0.0f); 
        public float cameraSpeed = 2.0f;

        void Update()
        {
            if (target)
            {
                Vector3 targetPosition = target.position + (target.rotation * distance) + offset;
                transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);

                Quaternion targetRotation = Quaternion.LookRotation(target.position + offset - transform.position, Vector3.up);
                targetRotation *= Quaternion.Euler(rotation);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraSpeed * Time.deltaTime);
            }
        }
    }
}