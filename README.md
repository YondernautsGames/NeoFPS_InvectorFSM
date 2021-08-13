# NeoFPS_InvectorFSM
NeoFPS and Invector FSM integration assets 

## Requirements

This repository was created using Unity 2019.4

It requires the assets [NeoFPS](https://assetstore.unity.com/packages/templates/systems/neofps-150179?aid=1011l58Ft) and [Invector FSM AI Template](https://assetstore.unity.com/packages/tools/ai/fsm-ai-template-123618?aid=1011l58Ft).

## Installation

This integration example is intended to be dropped in to a fresh project along with NeoFPS and Rayfire.

1. Import the Invector FSM AI Template asset.

2. Import NeoFPS and apply the required Unity settings using the NeoFPS Settings Wizard. You can find more information about this process [here](https://docs.neofps.com/manual/neofps-installation.html).

3. Clone this repository to a folder inside the project Assets folder such as "NeoFPS_InvectorFSM"

### Setting Up NeoFPS Weapons

There are 2 things you have to do to your firearm prefabs to get them working with Invector FSM AI:
- Firstly, you need to set the firearm to hit trigger colliders. If you are using a hitscan shooter module, this means setting the **Query Trigger Colliders** property to true on the shooter module component. If you are using a ballistic shooter module, then you need to do this on the projectile component on the prefab instead. This allows the bullets to detect the AI physics colliders.
- The other thing to do is to add a **NeoFpsInvector_FirearmNoise** component to the root of the firearm, and set its noise and distance properties. This means the guns will alert nearby AI when you shoot them.

*Note: the next NeoFPS update (1.1.15) will add trigger collider settings to melee weapons and explosions. Until then, they won't work with the Invector AI.*

### Setting Up NeoFPS Character

There are a few requirements for the player character to work with AI:
- Firstly, check that the root object is on the **CharacterControllers** layer, and has the **Player** tag.
- For each of the damage colliders in the player hierarchy, make sure that they are on the **CharacterPhysics** layer, and add a **NeoFpsInvector_PlayerDamageHandler** component.
- Make sure that any SeekerTarget objects in the character hierarchy are on the **AiVisibility** layer and have the **Player** tag.

### Setting Up AI Characters

AI characters have a number of relevant settings specifically for the NeoFPS integration:
- Firstly, select the root object and make sure that the root object is on the **CharacterControllers** layer, and check the tag is either **Enemy** or **CompanionAI** as required.
  - Expand the main AI controller behaviour on the character and select the **Detection** section.
  - **Detect Layer** should be set to: *Default, CharacterControllers, AiVisibility*. This allows the AI to detect other characters and the player.
  - **Detect Tags** should be set to the relevant tags (eg *Player, CompanionAI* for enemies or *Enemy* for companions). This allows the AI to filter which colliders it's interested in.
  - **Obstacles** should be set to *Default, EnvironmentRough, MovingPlatforms, DynamicObjects, Doors*. This allows the environment to block line of sight for the AI.
  - If the character is a shooter, you should also expand the **Shooter Manager** behaviour, and in the **Aim** section, set the **Damage Layer** setting to *Default, CharacterControllers, CharacterPhysics*. This allows the weapon to hit the player characters.
  - Expand the **Ragdoll System** behaviour on the root and in settings, make sure that **Disable Colliders** is switched **off**. If this is on, then all of the character's damage colliders will be disabled on start. We don't want that.
  - You can also add a **SimpleSurface** component to enable blood spray with neo's weapons.
- Go through the character's skeleton hierarchy and make sure that all of the bones with a collider and damage handler are on the **CharacterPhysics** layer. Remove the **vDamageReciever** component, and add a **NeoFpsInvector_DamageHandler** component instead. This adds a wrapper around the Invector FSM damage receiver that captures and translates damage from NeoFPS weapons.
- If the character has a health bar UI (often named something like *enemyHealthUI*), add the **NeoFpsInvector_UiBillboard** behaviour. This tracks the player camera and orients the UI towards it, even if the player respawns or switches to another camera.

### Setting Up AI Weapons

AI guns need to be set to affect the correct layers on the characters. To do this, on the **vShooterWeapon** component, under the **Layer & Tag** section, the **Hit Layer** should be set to *Default, Environment Detail, Moving Platforms, Dynamic Props, CharacterPhysics, Doors*. This also means that environment obstacles will stop the AI bullets.

## Demos

The repo contains a simple demo scene with a "blind" AI that will be alerted by your gunshots. When alerted, it will run and summon a number of enemy AI to attack you.

There are also variants of some of the Invector FSM AI and NeoFPS demo prefabs that are tweaked with the above settings:
- In the *AiCharacters* folder, you will find AI prefabs.
- The *AiWeapons* folder contains the weapon prefabs that are used on the above AI characters.
- The *PlayerWeapons* folder contains variants of the NeoFPS demo weapons that are set up as above too, along with a loadout to add them to the player character.
- The *Examples* folder contains the demo scene, along with a demo player character and a spawner/game mode.

## Issues

- Companion AI require you to use an invector health manager on the player character
- AI thrown weapons like grenades aren't currently picked up by the NeoFPS character's damage handlers
- Some melee AI (especially unarmed) seem to damage themselves when attacking
- The AI are not set up to work with neo's save system yet.

## Future Work

Tweaks to the NeoFPS melee weapons and explosions to allow damaging trigger colliders are due in the next update, and required for these to apply damage to Invector FSM AI. I will also try and fix the issues above, and add some more demo scenes.