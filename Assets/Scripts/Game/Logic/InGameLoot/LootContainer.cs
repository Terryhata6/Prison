using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Logic.InGameLoot
{
    public class LootContainer : MonoBehaviour
    {
        public bool Avaiability = false;
        public CurrencyType Type;
        private MeshCollider _triggerCollder;
        private Rigidbody _rigidbody;

        public IEnumerator Start()
        {
            
            
            _triggerCollder = GetComponent<MeshCollider>();
            _rigidbody = GetComponent<Rigidbody>();
            
            
            
            
            yield return new WaitForSeconds(1f);
            SetAvailable(true);
        }

        private void SetAvailable(bool value)
        {
            Avaiability = value;
        }

        public void CollectToBackBag(Transform target, Action<LootContainer, int> addToStuck, int currentStackCount)
        {
            SetAvailable(false);
            SetPhysics(false);
            transform.DOJump(target.position, 0.5f, 1, 0.2f).OnComplete(() =>
            {
                addToStuck?.Invoke(this, currentStackCount);
            });
        }

        private void SetPhysics(bool value)
        {
            _triggerCollder.enabled = value;
            _rigidbody.isKinematic = !value;
        }

        public void SetParentAndPosition(Transform parent, Vector3 position)
        {
            transform.parent = parent;
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(transform.parent.forward);
        }
    }
}