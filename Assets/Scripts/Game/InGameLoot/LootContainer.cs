using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.InGameLoot
{
    public class LootContainer : MonoBehaviour
    {
        public bool Avaiability = false;

        public IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            SetAvailable(true);
        }

        private void SetAvailable(bool value)
        {
            Avaiability = value;
        }

        public void Collect(Transform target)
        {
            SetAvailable(false);
            transform.DOJump(target.position, 0.5f, 1, 0.2f).OnComplete(() =>
            {
                Destroy(gameObject, 0.1f);
            });
        }
    }
}