using System;
using Gameplay;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        public GameObject selectedObject;

        [SerializeField] LayerMask layerMask;


        [Header("Debug")] [SerializeField] Vector3 m_lastPoint;
        [SerializeField] Vector3 m_lastPointInWorldSpace;

        public UnityEvent onSelectObject;
        public UnityEvent onReleaseObject;

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
            {
                //draw invisible ray cast/vector
                //    Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                //    Debug.Log(hit.collider.transform.parent.parent.name);
            }
            m_lastPointInWorldSpace = hit.point;

            if (Input.GetMouseButtonUp(0))
            {
                if (selectedObject == null)
                {
                    if (hit.collider.transform.parent == null) return;
                    if (hit.collider.transform.parent.parent.gameObject
                        .CompareTag("Disk")) // Disk has a compound Collider of capsules, nested in the object, 
                    {
                        selectedObject = hit.collider.transform.parent.parent.gameObject;
                        selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                        onSelectObject?.Invoke();
                    }

                }
                else
                {
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    selectedObject = null;
                    GameManager.Instance.numberOfMoves++;
                    UIManager.Instance.ChangeMovesText(GameManager.Instance.numberOfMoves);
                    onReleaseObject?.Invoke();
                    return;
                }
            }

            OnMouseDrag(hit);
        }

        void OnMouseDrag(RaycastHit hit)
        {
            if (selectedObject == null) return;

            m_lastPoint = hit.point;
            Vector3 newPos;
            // Debug.Log(hit.point);
            if (selectedObject.GetComponent<Disk>().movementRestricted)
            {
                newPos = new Vector3(selectedObject.transform.position.x,
                    hit.point.y, selectedObject.transform.position.z);
            }
            else
            {

                newPos = new Vector3(hit.point.x,
                    hit.point.y, selectedObject.transform.position.z);
            }

            selectedObject.transform.position = newPos;
        }
    }
}