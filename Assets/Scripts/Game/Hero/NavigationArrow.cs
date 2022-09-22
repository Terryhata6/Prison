using System;
using System.Collections;
using Game.Logic;
using Game.Logic.EventIndicator;
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
            foreach (var exits in FindObjectsOfType<CaveExit>())
            {
                if (exits.IsEscape)
                {
                    _target = exits;
                    break;
                }
            }
            ActivateNavigation(heroMove);
        }
        
        public void ActivateNavigationToReturn(HeroMove heroMove)
        {
            foreach (var exits in FindObjectsOfType<CaveExit>())
            {
                if (!exits.IsEscape)
                {
                    _target = exits;
                    break;
                }
            }
            ActivateNavigation(heroMove);
        }

        private void ActivateNavigation(HeroMove heroMove)
        {
            _heroMove = heroMove;
            StartCoroutine(NavigationArrowDrive(_target.transform));
        }

        public void ActivateNavigationTo(Transform target)
        {
            StartCoroutine(NavigationArrowDrive(target));
        }

        private IEnumerator NavigationArrowDrive(Transform target)
        {
            _arrowView.gameObject.SetActive(true);
            if (_heroMove == null)
            {
                _heroMove = GetComponentInParent<HeroMove>();
            }
            bool show = true;
            while (show)
            {
                var heroPosition = _heroMove.transform.position;
                transform.position = heroPosition + (target.transform.position - heroPosition).normalized * Distance + Vector3.up * Height;
                transform.LookAt(target:target);
                if (Vector3.Distance(_arrowView.transform.position,target.position) < 1f)
                {
                    show = false;
                }
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