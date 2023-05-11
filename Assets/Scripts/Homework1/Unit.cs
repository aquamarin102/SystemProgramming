using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homework1
{
    public class Unit : MonoBehaviour
    {
        private int _health = 50;
        private const int _maxHealth = 100;

        private void Start()
        {
            ReceiveHealing();
        }

        private void ReceiveHealing()
        {
            StartCoroutine(Regeneration(0.5f, 5));
        }

        private IEnumerator Regeneration(float waitTime, int valueHpRegeneration)
        {
            float regenerationTime = 3f;
            float elapsedTime = 0f;

            while (_health < _maxHealth)
            {
                if (elapsedTime == regenerationTime)
                    StopAllCoroutines();

                yield return new WaitForSeconds(waitTime);
                elapsedTime += waitTime;
                _health += valueHpRegeneration;

                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                    yield break;
                }

                Debug.Log(_health);
            }
        }
    }
}