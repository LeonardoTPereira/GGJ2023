using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scenario;

namespace Assets.Scripts.Scenario
{    
    public class Parallax : MonoBehaviour
    {
        enum BackgroundType
        {
            Farthest, Far, Middle, Close, Closest
        }

        [SerializeField] private BackgroundType _backgroundType;

        private float _backgroundSlotSize; // background sprite size in X axis
        private GameObject _camera;
        private float _parallaxFactorInX;
        private float _parallaxFactorInY;

        private float _xCentralPos;
        private float _yCentralPos;
        private float _temp;
        private float _xDistance;
        private float _yDistance;

        void Start()
        {
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
            _xCentralPos = this.transform.position.x;
            _yCentralPos = this.transform.position.y;

            GetParallaxConfigs();

           _backgroundSlotSize = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;    // has to have a gameobject with a spriterender used as background
        }

        private void GetParallaxConfigs()
        {
            ParallaxConfigs _parallaxConfigs = ParallaxConfigs.Instance;

            switch (_backgroundType)
            {
                case BackgroundType.Farthest:
                    _parallaxFactorInX = _parallaxConfigs.farthestX;
                    _parallaxFactorInY = _parallaxConfigs.farthestY;
                    break;
                case BackgroundType.Close:
                    _parallaxFactorInX = _parallaxConfigs.closeX;
                    _parallaxFactorInY = _parallaxConfigs.closeY;
                    break;
                case BackgroundType.Closest:
                    _parallaxFactorInX = _parallaxConfigs.closestX;
                    _parallaxFactorInY = _parallaxConfigs.closestY;
                    break;
            }
        }

        void Update()
        {
            GetParallaxConfigs();
            _temp = _camera.transform.position.x * (1 - _parallaxFactorInX);
            _xDistance = _camera.transform.position.x * _parallaxFactorInX;
            _yDistance = _camera.transform.position.y * _parallaxFactorInY;

            transform.position = new Vector3(_xCentralPos + _xDistance, _yCentralPos + _yDistance, transform.position.z);


            // Repositioning of the background sprites
            if (_temp > _xCentralPos + _backgroundSlotSize * 2)
                _xCentralPos += _backgroundSlotSize * 3;
            else if (_temp < _xCentralPos - _backgroundSlotSize * 2)
                _xCentralPos -= _backgroundSlotSize * 3;
        }
    }
}