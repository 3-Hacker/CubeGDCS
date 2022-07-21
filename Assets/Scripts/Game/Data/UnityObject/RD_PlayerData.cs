﻿using UnityEngine;

namespace Game.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Player Data", order = 2)]
    public class RD_PlayerData : ScriptableObject
    {
        public float PlayerSpeed;
        public float PlayerTurnSpeed;
        public float PlayerMaxXPos;
        public float PlayerInputSensitivity;
        public bool SharpMode;
    }
}