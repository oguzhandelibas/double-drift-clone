using System;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleDrift
{
    public class SlideControl : MonoBehaviour
    {
        #region Event

        public Action<float> OnSlide;
        public Action OnSlideEnd;

        #endregion
        #region Serialized Variables
        
        [SerializeField] [Tooltip("You can choose slide factor to make it slow or fast")] [Range(0f, 1f)]
        private float slidingFactor = .1f;

        #endregion

        #region Private Variables

        private const int ResolutionReferenceY = 1920;
        private const int ResolutionReferenceX = 1080;

        private float resolutionFactorX = 1;
        private float resolutionFactorY = 1;

        private Vector2 touchStart = Vector2.zero;
        private Vector2 touchEnd = Vector2.zero;
        private float upDownSlide;
        private float leftRightSlide;

        #endregion

        #region Initialization

        private void Start()
        {
            resolutionFactorX = (float) ResolutionReferenceX / Screen.width;
            resolutionFactorY = (float) ResolutionReferenceY / Screen.height;

            Debug.Log("Screen width: " + Screen.width);
            Debug.Log("Screen height: " + Screen.height);
            Debug.Log("ResolutionFactorX: " + resolutionFactorX);
            Debug.Log("ResolutionFactorY: " + resolutionFactorY);

            touchStart = Vector2.zero;
            touchEnd = Vector2.zero;
        }

        #endregion

        #region Control Loop

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                leftRightSlide = Mathf.Clamp((touchEnd.x - touchStart.x) * resolutionFactorX * slidingFactor, -90f, 90f);
                
                OnSlide?.Invoke(leftRightSlide);
                
                touchEnd = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnSlideEnd?.Invoke();
            }
        }

        #endregion

        #region Public Variables

        /// <summary>
        /// Returns Up-Down slide value
        /// </summary>
        /// <returns></returns>
        public float GetUpDownSlide()
        {
            return upDownSlide;
        }

        /// <summary>
        /// Returns Left-Right slide value
        /// </summary>
        /// <returns></returns>
        public float GetLeftRightSlide()
        {
            return leftRightSlide;
        }

        #endregion

        #region Debug

        public void DebugSlide()
        {
            Debug.Log("Slide invoked!");
        }

        public void DebugRelease()
        {
            Debug.Log("Release invoked!");
        }

        public void DebugUpDownSlide()
        {
            Debug.Log(GetUpDownSlide());
        }

        public void DebugLeftRightSlide()
        {
            Debug.Log(GetLeftRightSlide());
        }

        #endregion
    }
}