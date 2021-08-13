/*
* Copyright 2021 Yondernauts Game Studios Ltd
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*       http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using Invector;
using Invector.vCharacterController;
using Invector.vEventSystems;
using UnityEngine;
using NeoFPS;
using NeoFPS.ModularFirearms;
using System.Collections.Generic;

namespace NeoFPS.InvectorFSM
{
    public class NeoFpsInvector_AmmoEffect : BaseAmmoEffect
    {
        [SerializeField, Tooltip("The damage the bullet does.")]
        private vDamage m_Damage = new vDamage(25);

        [SerializeField, Tooltip("The size of the bullet. Used to size decals.")]
        private float m_BulletSize = 1f;

        [SerializeField, Tooltip("The force to be imparted onto the hit object. Requires either a [Rigidbody][unity-rigidbody] or an impact handler.")]
        private float m_ImpactForce = 15f;

        private static List<IDamageHandler> s_DamageHandlers = new List<IDamageHandler>(4);

#if UNITY_EDITOR
        void OnValidate()
        {
            if (m_BulletSize < 0.1f)
                m_BulletSize = 0.1f;
            if (m_ImpactForce < 0f)
                m_ImpactForce = 0f;
        }
#endif

        public override void Hit(RaycastHit hit, Vector3 rayDirection, float totalDistance, float speed, IDamageSource damageSource)
        {
            // Show effect
            SurfaceManager.ShowBulletHit(hit, rayDirection, m_BulletSize, hit.rigidbody != null);

            

            // Apply damage
            var invectorDamageHandler = hit.collider.GetComponent<vDamageReceiver>();
            if (invectorDamageHandler !=null)
            {
                // Sort invectordamage
                m_Damage.force = -hit.normal * m_ImpactForce;
                m_Damage.sender = damageSource.damageSourceTransform;

                // Apply damage
                invectorDamageHandler.TakeDamage(m_Damage);

                // Show hit marker
                if (damageSource != null && damageSource.controller != null)
                    damageSource.controller.currentCharacter.ReportTargetHit(false);


            }
            else
            {
                // Apply damage
                hit.collider.GetComponents(s_DamageHandlers);
                for (int i = 0; i < s_DamageHandlers.Count; ++i)
                    s_DamageHandlers[i].AddDamage(m_Damage.damageValue, hit, damageSource);
                s_DamageHandlers.Clear();

                // Apply force (nb check collider in case the damage resulted in the object being destroyed)
                if (hit.collider != null && m_ImpactForce > 0f)
                {
                    IImpactHandler impactHandler = hit.collider.GetComponent<IImpactHandler>();
                    if (impactHandler != null)
                        impactHandler.HandlePointImpact(hit.point, rayDirection * m_ImpactForce);
                    else
                    {
                        if (hit.rigidbody != null)
                            hit.rigidbody.AddForceAtPosition(rayDirection * m_ImpactForce, hit.point, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}