using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


scaffold - dbcontext "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Chinook;Integrated Security=True;Trust Server Certificate=False;" Microsoft.EntityFrameworkCore.SqlServer - contextdir Data - outputdir Models / Generated - contextnamespace A2Chinook.Data -namespace A2Chinook.Models -force
namespace IMDBFinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}