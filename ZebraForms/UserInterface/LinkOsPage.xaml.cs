using System.Text;
using System.Threading.Tasks;
using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using Xamarin.Forms;

namespace ZebraForms
{
    public partial class LinkOsPage : ContentPage
    {
        ViewModels.LinkOsVm _vm;
 
        public LinkOsPage()
        {
            InitializeComponent();

            this.BindingContext = _vm = new ViewModels.LinkOsVm();
        }
    }
}
