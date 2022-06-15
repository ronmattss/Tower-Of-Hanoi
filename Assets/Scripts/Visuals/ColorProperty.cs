using UnityEngine;

namespace Visuals
{
    [RequireComponent(typeof(Renderer))]
    public class ColorProperty : MonoBehaviour
    {
        [SerializeField] private Color color;
        
        void Start()
        {
        GetComponent<MeshRenderer>().material.color = color;
        }

        // Update is called once per frame

    }
}
