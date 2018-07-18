﻿using System;
namespace AppStract.Dependencies
{
    public interface IAudioPlayer
    {
        void PlayAudio();
        void StopAudio();
        void PauseAudio();
        string AudioFileName { get; set; }
        string AudioFilePath { get; set; }
    }
}
