using Xamarin.Forms;

namespace ZebraForms
{
    public partial class ZebraFormsPage : ContentPage
    {
        IScanning _scanning;
    
        public ZebraFormsPage()
        {
            InitializeComponent();
            
            _scanning = DependencyService.Get<IScanning>();
            _scanning.ScanResultFound += Scanning_ScanResultFound;
            _scanning.ScanStatusChanged += Scanning_ScanStatusChanged;
            var result = _scanning.Initialize();
            
            
            System.Diagnostics.Debug.WriteLine($"Scanning Initialized: {result}");
        }

        void Scanning_ScanResultFound(object sender, ZebraForms.ScanResultEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => lblResult.Text = e.ScannedText);
        }

        void Scanning_ScanStatusChanged(object sender, ZebraForms.ScanStatusEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => lblResult.Text = $"Scan Status: {e.ScanStatus}");
        }

        void StartScanning(object sender, System.EventArgs e)
        {
            _scanning.StartScanning();
        }
        
        void StopScanning(object sender, System.EventArgs e)
        {
            _scanning.StopScanning();
        }
    }
}
