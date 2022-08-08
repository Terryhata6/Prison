using System;
using System.Collections;
using Game.Logic;
using UnityEngine;

namespace Game.Hero
{
    public class NavigationArrow : MonoBehaviour
    {
        private CaveExit _target;
        private HeroMove _heroMove;
        public Transform _arrowView;
        [Header("Offset")]
        public float Height = 0.75f;
        public float Distance = 1f;


        public void ActivateNavigationToCaveExit(HeroMove heroMove)
        {
            _target = FindObjectOfType<CaveExit>();
            ActivateNavigation(heroMove);
        }

        private void ActivateNavigation(HeroMove heroMove)
        {
            _heroMove = heroMove;
            StartCoroutine(NavigationArrowDrive(_target.transform));
        }

        private IEnumerator NavigationArrowDrive(Transform target)
        {
            _arrowView.gameObject.SetActive(true);
            while (true)
            {
                var heroPosition = _heroMove.transform.position;
                Height = 0.75f;
                Distance = 1.5f;
                transform.position = heroPosition + (target.transform.position - heroPosition).normalized * Distance + Vector3.up * Height;
                transform.LookAt(target:target);
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}