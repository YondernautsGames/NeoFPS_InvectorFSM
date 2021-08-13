using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.InvectorFSM
{
    public class NeoFpsInvector_UiBillboard : MonoBehaviour
    {
        private Transform m_Target = null;
        private Transform m_LocalTransform = null;

        void Start()
        {
            m_LocalTransform = transform;
            FirstPersonCamera.onCurrentCameraChanged += OnCurrentCameraChanged;
            OnCurrentCameraChanged(FirstPersonCamera.current);
        }

        void OnDestroy()
        {
            FirstPersonCamera.onCurrentCameraChanged -= OnCurrentCameraChanged;
        }

        void LateUpdate()
        {
            if (m_Target != null)
                m_LocalTransform.LookAt(m_Target, Vector3.up);
        }

        void OnCurrentCameraChanged(FirstPersonCamera cam)
        {
            if (cam == null)
            {
                var main = Camera.main;
                if (main != null)
                    m_Target = main.transform;
                else
                    m_Target = null;
            }
            else
                m_Target = cam.unityCamera.transform;
        }
    }
}