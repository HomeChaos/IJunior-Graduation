﻿using UnityEngine;

namespace Scripts.Settings
{
    public static class SoundUtils
    {
        private static SoundSettings _soundSettings;
        
        public static SoundSettings FindSoundSettings()
        {
            if (_soundSettings == null)
                _soundSettings = GameObject.FindObjectOfType<SoundSettings>();

            return _soundSettings;
        }
    }
}