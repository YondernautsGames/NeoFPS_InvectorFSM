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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeoFPS;
using Invector;
using Invector.vCharacterController;

namespace NeoFPS.InvectorFSM
{
    public class NeoFpsInvector_DamageHandler : vDamageReceiver, IDamageHandler
    {
        public DamageFilter inDamageFilter { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private vHealthController m_HealthController = null;
		
		public IHealthManager healthManager
		{
			get { return null; }
		}
		
        protected override void Start()
        {
            base.Start();
            m_HealthController = GetComponentInParent<vHealthController>();
        }

        public DamageResult AddDamage(float damage)
        {
            if (m_HealthController != null)
            {
                var d = new vDamage();
                d.damageValue = Mathf.CeilToInt(damage);
                TakeDamage(d);
                return DamageResult.Standard;
            }
            else
                return DamageResult.Ignored;
        }

        public DamageResult AddDamage(float damage, RaycastHit hit)
        {
            if (m_HealthController != null)
            {
                var d = new vDamage();
                d.damageValue = Mathf.CeilToInt(damage);
                d.hitPosition = hit.point;
                TakeDamage(d);
                return DamageResult.Standard;
            }
            else
                return DamageResult.Ignored;
        }

        public DamageResult AddDamage(float damage, IDamageSource source)
        {
            if (m_HealthController != null)
            {
                var d = new vDamage();
                d.damageValue = Mathf.CeilToInt(damage);
                d.sender = source.damageSourceTransform;
                TakeDamage(d);
				
				var result = DamageResult.Standard;

                // Report damage dealt
                if (damage > 0f)
                    DamageEvents.ReportDamageHandlerHit(this, source, transform.position, result, damage);

                return result;
            }
            else
                return DamageResult.Ignored;
        }

        public DamageResult AddDamage(float damage, RaycastHit hit, IDamageSource source)
        {
            if (m_HealthController != null)
            {
                var d = new vDamage();
                d.damageValue = Mathf.CeilToInt(damage);
                d.hitPosition = hit.point;
                d.sender = source.damageSourceTransform;
                TakeDamage(d);
				
				var result = DamageResult.Standard;

                // Report damage dealt
                if (damage > 0f)
                    DamageEvents.ReportDamageHandlerHit(this, source, transform.position, result, damage);

                return result;
            }
            else
                return DamageResult.Ignored;
        }
    }
}