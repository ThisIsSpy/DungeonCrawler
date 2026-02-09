using UnityEngine;

namespace NodeSystem 
{
    public class Node : MonoBehaviour
    {
        [field: SerializeField] public bool IsDeadEnd { get; private set; }
    }
}