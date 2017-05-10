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
    public class LinkOsVm : ViewModelBase
    {
        const string SampleZpl = @"^XA^FO20,20^A0N,25,25^FDThis is a ZPL test.^FS^XZ";
            
        IDiscoveryEventHandler _handler;
    
        ObservableCollection<FoundPrinter> _foundPrinters;
        public ObservableCollection<FoundPrinter> FoundPrinters {
            get { return _foundPrinters; }
            private set { SetField(ref _foundPrinters, value); }
        }

        ICommand _loadPrinters;

        public ICommand LoadPrinters {
            get { return _loadPrinters; }
            set { SetField(ref _loadPrinters, value); }
        }
        
        ICommand _printSample;

        public ICommand PrintSample {
            get { return _printSample; }
            set { SetField(ref _printSample, value); }
        }
        
        bool _searchingForPrinters;

        public bool SearchingForPrinters {
            get { return _searchingForPrinters; }
            set { SetField(ref _searchingForPrinters, value); }
        }
        
        string _currentStatus;

        public string CurrentStatus {
            get { return _currentStatus; }
            set { SetField(ref _currentStatus, value); }
        }

        public LinkOsVm() {
            this.Title = "Link-OS";

            _handler = LinkOS.Plugin.DiscoveryHandlerFactory.Current.GetInstance();

            _foundPrinters = new ObservableCollection<FoundPrinter>();

            LoadPrinters = 
                new Command(
                    async _ => {
                        try {
                            FoundPrinters.Clear();
                            SearchingForPrinters = true;
                            CurrentStatus = "Searching For Printers...";
                            _handler.OnFoundPrinter += Handler_OnFoundPrinter;
                            await Task.Run(() => LinkOS.Plugin.NetworkDiscoverer.Current.FindPrinters(_handler));
                        } catch (Exception ex) {
                            System.Diagnostics.Debug.WriteLine(ex);
                        } finally {
                            _handler.OnFoundPrinter += Handler_OnFoundPrinter;
                            SearchingForPrinters = false;
                            CurrentStatus = "Finished Searching...";
                        }
                    },
                    _ => !SearchingForPrinters);
                    
            PrintSample = 
                new Command(
                    _ => {
                        var selectedPrinters = FoundPrinters.Where(x => x.Selected).ToList();

                        foreach (var printer in selectedPrinters) {
                            var conn = LinkOS.Plugin.ConnectionBuilder.Current.Build(printer.IpAddress);
                            try {
                                conn.Open();
                                conn.Write(Encoding.UTF8.GetBytes(SampleZpl));
                            } catch (Exception ex) {
                                CurrentStatus = ex.Message;
                            } 
                            finally {
                                conn.Close();
                            }   
                        }
                    });
        }


        void Handler_OnFoundPrinter(LinkOS.Plugin.Abstractions.IDiscoveryHandler handler, LinkOS.Plugin.Abstractions.IDiscoveredPrinter discoveredPrinter)
        {
            if (!FoundPrinters.Any(x => x.IpAddress.Equals(discoveredPrinter.Address, StringComparison.OrdinalIgnoreCase)))
                FoundPrinters.Add(new FoundPrinter { IpAddress = discoveredPrinter.Address });
        }

        public class FoundPrinter : ViewModelBase { 
            string _IpAddress;

            public string IpAddress {
                get { return _IpAddress; }
                set { SetField(ref _IpAddress, value); }
            }
            
            bool _selected;

            public bool Selected {
                get { return _selected; }
                set { SetField(ref _selected, value); }
            }
        }
    }
}
