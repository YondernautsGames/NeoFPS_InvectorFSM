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
using Invector.vCharacterController.AI;
using NeoFPS.ModularFirearms;
using System;

namespace NeoFPS.InvectorFSM
{
    public class NeoFpsInvector_FirearmNoise : vNoiseObject
    {
        private IModularFirearm m_Firearm = null;
        private IShooter m_Shooter = null;

        private void Awake()
        {
            m_Firearm = GetComponentInParent<IModularFirearm>();
        }

        private void OnEnable()
        {
            // Subscribe to shooter
            if (m_Firearm != null)
            {
                m_Firearm.onShooterChange += OnShooterChanged;
                OnShooterChanged(m_Firearm, m_Firearm.shooter);
            }
        }

        private void OnDisable()
        {
            if (m_Shooter != null)
            {
                // Unsubscribe to shooter
                m_Shooter.onShoot -= OnShoot;
                m_Shooter = null;

                // Stop checking for shooter changes
                m_Firearm.onShooterChange -= OnShooterChanged;
            }
        }

        private void OnShooterChanged(IModularFirearm f, IShooter s)
        {
            // Unsubscribe from the old one
            if (m_Shooter != null)
                m_Shooter.onShoot -= OnShoot;

            m_Shooter = s;

            // Subscribe to the new one
            if (m_Shooter != null)
                m_Shooter.onShoot += OnShoot;
        }

        void OnShoot(IModularFirearm f)
        {
            TriggerNoise();
        }
    }
}