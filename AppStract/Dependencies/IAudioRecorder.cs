using System;
namespace AppStract.Dependencies
{
    public interface IAudioRecorder
    {
        void StartRecord();
        void StopRecord();
        string AudioFileName { get; set; }
        string AudioFilePath { get; set; }
    }
}
