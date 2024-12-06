using IMDBFinalProject.Data;
using Microsoft.EntityFrameworkCore;
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
        ImdbContext context = new ImdbContext();
        CollectionViewSource moviesViewSource = new CollectionViewSource();
        public MoviesPage()
        {
            InitializeComponent();

            moviesViewSource = (CollectionViewSource)FindResource(nameof(moviesViewSource));

            // Fetch movies with their genres, titles, and release years
            var moviesWithDetails = context.Titles
                .Where(t => t.TitleType == "movie") // Filter only movies
                .Select(t => new
                {
                    t.OriginalTitle,
                    t.StartYear,     
                    Genres = t.Genres.Any() 
                    ? string.Join(", ", t.Genres.Select(g => g.Name)) 
                    : "No Genre" 
                })
                .ToList();

            moviesViewSource.Source = moviesWithDetails;
        }
    }
}
