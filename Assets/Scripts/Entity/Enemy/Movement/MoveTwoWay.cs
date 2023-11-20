using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Enemy
{
    public class MoveTwoWay : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private LayerMask mask;
        [SerializeField] private float maxHorizontalDistance = 1f;
        [SerializeField] private float maxVerticalDistance = 1f;
        [SerializeField] private float flipCooldown = 0.5f;
        private bool _canFlip;

        private Transform currentPoint;

        private TransformController transformController;

        private Rigidbody2D enemyRb;

        private void Awake()
        {
            transformController = GetComponent<TransformController>();
            enemyRb = GetComponent<Rigidbody2D>();
            _canFlip = true;
        }

        private void FixedUpdate()
        {
            enemyRb.velocity = transform.right * speed + new Vector3(0, enemyRb.velocity.y, 0);

            if (!_canFlip) return;
            if (CheckFlip())
            {
                StartCoroutine(CountCooldown());
                transformController.Flip();
            }
        }

        private IEnumerator CountCooldown()
        {
            _canFlip = false;
            yield return new WaitForSeconds(flipCooldown);
            _canFlip = true;
        }

        private bool CheckFlip()
        {
            var currentTransform = transform;
            if (Physics2D.Raycast(currentTransform.position, currentTransform.right, 2 * maxHorizontalDistance, mask).collider != null)
                return true;
            else if (Physics2D.Raycast(currentTransform.position, -currentTransform.right, 2 * maxHorizontalDistance, mask).collider != null)
                return true;
            return (Physics2D.Raycast(currentTransform.position + currentTransform.right * maxHorizontalDistance,
                -currentTransform.up,
                maxVerticalDistance, mask).collider == null);
        }
    }
}