using System;
using System.Linq;
using Symbol.XamarinEMDK;
using Symbol.XamarinEMDK.Barcode;

namespace ZebraForms.Droid
{
    public class ZebraScanning : Java.Lang.Object, IScanning, EMDKManager.IEMDKListener
    {
        EMDKManager _emdkManager;
        BarcodeManager _barcodeManager;
        Scanner _scanner;

        public event EventHandler<ScanResultEventArgs> ScanResultFound;
        public event EventHandler<ScanStatusEventArgs> ScanStatusChanged;

        public ZebraScanning()
        {
        }

        public bool Initialize()
        {
            var wasSuccessful = false;
            try {
                var results = EMDKManager.GetEMDKManager(Android.App.Application.Context, this);
                wasSuccessful = results.StatusCode == EMDKResults.STATUS_CODE.Success;
            } catch (Exception ex) {
                wasSuccessful = false;
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return wasSuccessful;
        }

        public void OnClosed()
        {
        }

        public void OnOpened(EMDKManager p0)
        {
            _emdkManager = p0;
            _barcodeManager = (BarcodeManager)_emdkManager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (_scanner != null) {
                    _scanner.Data -= ReceivedScanData;
                    _scanner?.Release();
                    _scanner?.Dispose();
                }
                
                _barcodeManager?.Dispose();
                
                _emdkManager?.Release();
                _emdkManager?.Dispose();
            }
        
            base.Dispose(disposing);
        }

        public void StartScanning()
        {
            if (_scanner != null)
                StopScanning();
        
            _scanner = _barcodeManager?.GetDevice(BarcodeManager.DeviceIdentifier.Default);

            if (_scanner != null) {
                _scanner.Data += ReceivedScanData;
                _scanner.Status += ScannerStatusChanged;

                _scanner.TriggerType = Scanner.TriggerTypes.SoftOnce;
                
                _scanner.Enable();
                
                var config = _scanner.GetConfig();
                config.SkipOnUnsupported = ScannerConfig.SkipOnUnSupported.None;
                config.ScanParams.DecodeLEDFeedback = true;
                config.DecoderParams.Code39.Enabled = true;
                config.DecoderParams.Code128.Enabled = false;
                //config.ReaderParams.ReaderSpecific.ImagerSpecific.PickList = ScannerConfig.PickList.Enabled;
                _scanner.SetConfig(config);
                
                _scanner.Read();
            }
        }

        public void StopScanning()
        {
            if (_scanner != null) {
                _scanner.Status -= ScannerStatusChanged;
                _scanner.Data -= ReceivedScanData;
                _scanner?.CancelRead();
                _scanner?.Disable();
                
                _scanner?.Release();
                _scanner?.Dispose();
                _scanner = null;
            }
        }

        void ScannerStatusChanged(object sender, Scanner.StatusEventArgs e)
        {
            if(e.P0 != null)
                ScanStatusChanged?.Invoke(sender, new ScanStatusEventArgs { ScanStatus = e.P0.State.Name() });
        }

        void ReceivedScanData(object sender, Scanner.DataEventArgs e)
        {
            if (e.P0 != null && e.P0.Result == ScannerResults.Success) {
                var resultData = e.P0.GetScanData();
                var firstResult = resultData.FirstOrDefault();

                if(firstResult != null)
                    ScanResultFound?.Invoke(sender, new ScanResultEventArgs { ScannedText = firstResult.Data });

                StopScanning();
            }
        }
    }
}
