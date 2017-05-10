using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LinkOS.Plugin.Abstractions;
using Xamarin.Forms;

namespace ZebraForms.ViewModels
{
    public class ScanningPageVm : ViewModelBase
    {
        IScanning _scanning;
        
        ICommand _startScanning;

        public ICommand StartScanning {
            get { return _startScanning; }
            set { SetField(ref _startScanning, value); }
        }

        ICommand _stopScanning;

        public ICommand StopScanning {
            get { return _stopScanning; }
            set { SetField(ref _stopScanning, value); }
        }

        string _scanStatus;

        public string ScanStatus {
            get { return _scanStatus; }
            set { SetField(ref _scanStatus, value); }
        }

        string _scanResult;

        public string ScanResult {
            get { return _scanResult; }
            set { SetField(ref _scanResult, value); }
        }

        public ScanningPageVm() {
            this.Title = "Scanning";

            _scanning = DependencyService.Get<IScanning>();
            _scanning.ScanResultFound += Scanning_ScanResultFound;
            _scanning.ScanStatusChanged += Scanning_ScanStatusChanged;
            var result = _scanning.Initialize();

            StartScanning = new Command(_ => _scanning.StartScanning());
            StopScanning = new Command(_ => _scanning.StopScanning());
            
            ScanStatus = $"Scanning Initialized: {result}";
        }
        
        void Scanning_ScanResultFound(object sender, ZebraForms.ScanResultEventArgs e)
        {
            ScanResult = e.ScannedText;
        }

        void Scanning_ScanStatusChanged(object sender, ZebraForms.ScanStatusEventArgs e)
        {
            ScanStatus = e.ScanStatus;
        }
    }
}
