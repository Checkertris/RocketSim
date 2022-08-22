using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
        //Public
        public GameObject target;
        public Vector3 initialOffset;
        public float smoothSpeed;

        //Private
        private Transform targetTransform;
        private Vector3 Offset;
        private ControlPanelInfo controlPanel;


        void Start()
        {
            //Settings
            smoothSpeed = 1f;
            initialOffset = new Vector3(10f,0f,0f);

            //Constants
            controlPanel = FindObjectOfType<ControlPanelInfo>();
            targetTransform = target.transform;

            //StartMethods
            StartLookAtTarget();

        }

        void Update()
        {
            if (controlPanel.GetViewMode())
            {
                transform.position = targetTransform.position + initialOffset;
            }

            else
            {
                ToggleZoom();
                transform.position = targetTransform.position + Offset;
            }

        }
    


        void ToggleZoom()
        {
            Offset = initialOffset * controlPanel.GetZoom();

        }

  
        private void StartLookAtTarget()
        {
            gameObject.transform.LookAt(targetTransform);
        }

        private void SmoothFollow()
        {
            Vector3 desiredPosition = targetTransform.position + Offset;
            Vector3 smoothPosition = Vector3.Lerp(base.transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothPosition;
        }
}
