using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ZebraForms
{
    public partial class ScanningPage : ContentPage
    {            
        public ScanningPage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.ScanningPageVm(); 
        }
    }
}
