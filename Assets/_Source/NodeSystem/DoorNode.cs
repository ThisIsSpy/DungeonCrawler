using DG.Tweening;
using PlayerSystem;
using System.Collections;
using UnityEngine;

namespace NodeSystem 
{
    public class DoorNode : Node
    {
        [SerializeField] private GameObject LeftPart;
        [SerializeField] private GameObject RightPart;
        [SerializeField] private float moveDistance = 0.4f;
        [SerializeField] private float moveTime = 0.25f;
        private bool areAlreadyActive = false;
        private bool areAlreadyOpen = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _) && !IsDeadEnd) Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _) && !IsDeadEnd) Deactivate();
        }

        public void Activate()
        {
            if (areAlreadyActive || areAlreadyOpen) return;
            areAlreadyActive = true;
            StartCoroutine(DoorOpenMovementCoroutine());
        }

        public void Deactivate()
        {
            if (areAlreadyActive || !areAlreadyOpen) return;
            areAlreadyActive = true;
            StartCoroutine(DoorCloseMovementCoroutine());
        }

        private IEnumerator DoorOpenMovementCoroutine()
        {
            LeftPart.transform.DOLocalMoveZ(LeftPart.transform.localPosition.z - moveDistance, moveTime);
            RightPart.transform.DOLocalMoveZ(RightPart.transform.localPosition.z + moveDistance, moveTime);
            yield return new WaitForSeconds(moveTime);
            areAlreadyActive = false;
            areAlreadyOpen = true;
        }

        private IEnumerator DoorCloseMovementCoroutine()
        {
            LeftPart.transform.DOLocalMoveZ(LeftPart.transform.localPosition.z + moveDistance, moveTime);
            RightPart.transform.DOLocalMoveZ(RightPart.transform.localPosition.z - moveDistance, moveTime);
            yield return new WaitForSeconds(moveTime);
            areAlreadyActive = false;
            areAlreadyOpen = false;
        }
    }
}
