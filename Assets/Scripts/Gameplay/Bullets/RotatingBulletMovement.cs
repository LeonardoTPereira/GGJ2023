using Gameplay.Bullets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Gameplay.Bullets
{
    public class RotatingBulletMovement : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, 30));
        }
    }
}