using System;
using Android.App;
using ZXing.Mobile;

namespace ZebraForms.Droid
{
    public class ZxingScanning : IScanning, IDisposable
    {
        public ZxingScanning()
        {
        }
        
        MobileBarcodeScanner _scanner;

        public event EventHandler<ScanResultEventArgs> ScanResultFound;
        public event EventHandler<ScanStatusEventArgs> ScanStatusChanged;

        public bool Initialize()
        {
            ScanStatusChanged?.Invoke(this, new ScanStatusEventArgs { ScanStatus = "Initialized" });

            return true;
        }

        public async void StartScanning()
        {        
            _scanner = new MobileBarcodeScanner();

            ScanStatusChanged?.Invoke(this, new ScanStatusEventArgs { ScanStatus = "Starting Scan" });
            var result = await _scanner.Scan();

            if (result != null) {
                ScanStatusChanged?.Invoke(this, new ScanStatusEventArgs { ScanStatus = "Scan Result Received" });
                ScanResultFound?.Invoke(this, new ScanResultEventArgs { ScannedText = result.Text });
            } else { 
                ScanStatusChanged?.Invoke(this, new ScanStatusEventArgs { ScanStatus = "Scan Cancelled" });
            }
        }

        public void StopScanning()
        {
            ScanStatusChanged?.Invoke(this, new ScanStatusEventArgs { ScanStatus = "Stopping Scanning" });
            
            if (_scanner != null) {
                try {
                    _scanner?.Cancel();
                    _scanner = null;
                } catch (ObjectDisposedException) {

                }
            }   
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    _scanner = null;
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
