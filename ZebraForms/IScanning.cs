using System;
namespace ZebraForms
{
    public interface IScanning
    {
        event EventHandler<ScanResultEventArgs> ScanResultFound;
        event EventHandler<ScanStatusEventArgs> ScanStatusChanged;
    
        bool Initialize();

        void StartScanning();

        void StopScanning();
    }

    public class ScanResultEventArgs : EventArgs { 
        public string ScannedText { get; set; }
    }
    
    public class ScanStatusEventArgs : EventArgs { 
        public string ScanStatus { get; set; }
    }
}
