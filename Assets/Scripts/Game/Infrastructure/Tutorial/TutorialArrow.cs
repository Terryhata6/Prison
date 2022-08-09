using System;
using System.Collections;
using Game.Hero;
using UnityEngine;

namespace Game.Infrastructure.Tutorial
{
    public class TutorialArrow : MonoBehaviour
    {
        public int LevelToShow = 0;
        public GameObject Arrow;

        private void Awake()
        {
            if (PlayerPrefs.GetInt("TriesPlayed", 0) != 0)
            {
                Destroy(gameObject);
            }
            else
            {
                ShowArrow();
            }
        }

        private void ShowArrow()
        {
            StartCoroutine(ArrowDriveCoroutine());
        }

        private IEnumerator ArrowDriveCoroutine()
        {
            Arrow.SetActive(true);
            Vector3 basePosition = Arrow.transform.position;
            while (true)
            {
                Arrow.transform.position = basePosition + Vector3.up * Mathf.Cos(Time.unscaledTime);
                yield return null;
            }
        }

        private void TurnOffArrow()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TurnOffArrow();
            }
        }
    }
}