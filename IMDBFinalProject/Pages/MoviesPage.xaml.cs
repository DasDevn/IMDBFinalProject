using IMDBFinalProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IMDBFinalProject.Pages
{
    /// <summary>
    /// Interaction logic for MoviesPage.xaml
    /// </summary>
    public partial class MoviesPage : Page
    {
        private readonly ImdbContext _context = new ImdbContext();
        private CollectionViewSource moviesViewSource;
        public MoviesPage()
        {
            InitializeComponent();

            moviesViewSource = (CollectionViewSource)FindResource(nameof(moviesViewSource));

            moviesViewSource.Source = _context.Titles.Local.ToObservableCollection();
        }
    }
}
