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

namespace NeoFPS.InvectorFSM
{ 
    public class NeoFpsInvector_PlayerDamageHandler : MonoBehaviour, vIAttackReceiver
    {
        private IDamageHandler m_DamageHandler = null;

        void Awake()
        {
            m_DamageHandler = GetComponent<IDamageHandler>();
        }

        public void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
        {
            m_DamageHandler.AddDamage(damage.damageValue);
        }
    }
}