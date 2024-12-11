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

            // Fetch movies with their genres, titles, release years, and ratings
            var moviesWithDetails = context.Titles
                .Where(t => t.TitleType == "movie") 
                .Select(t => new
                {
                    t.TitleId,
                    t.OriginalTitle, 
                    t.StartYear,     
                    Genres = t.Genres.Any()
                        ? string.Join(", ", t.Genres.Select(g => g.Name)) 
                        : "No Genre", 
                    Rating = t.Rating != null 
                        ? $"{t.Rating.AverageRating:0.0}/10"
                        : "Not Rated",
                    Actors = context.Principals
                        .Where(p => p.TitleId == t.TitleId)
                        .Select(p => p.Name.PrimaryName)
                        .ToList(),
                    Writers = t.Names1
                        .Select(n => n.PrimaryName)
                        .ToList(),
                    Directors = t.Names.Select(n => n.PrimaryName).ToList()
                })
                .ToList();




            moviesViewSource.Source = moviesWithDetails;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the search text from the TextBox
            string searchText = SearchTextBox.Text.ToLower();

            // Fetch movies with filtering based on the search text
            var moviesQuery = context.Titles
                .Where(t => t.TitleType == "movie");

            if (!string.IsNullOrEmpty(searchText))
            {
                moviesQuery = moviesQuery.Where(t => t.OriginalTitle.ToLower().Contains(searchText));
            }

            var filteredMovies = moviesQuery
                .Select(t => new
                {
                    t.TitleId,
                    t.OriginalTitle,
                    t.StartYear,
                    Genres = t.Genres.Any()
                        ? string.Join(", ", t.Genres.Select(g => g.Name))
                        : "No Genre",
                    Rating = t.Rating != null
                        ? $"{t.Rating.AverageRating:0.0}/10"
                        : "Not Rated",
                    Actors = context.Principals
                        .Where(p => p.TitleId == t.TitleId)
                        .Select(p => p.Name.PrimaryName)
                        .ToList(),
                    Writers = t.Names1
                        .Select(n => n.PrimaryName)
                        .ToList(),
                    Directors = t.Names.Select(n => n.PrimaryName).ToList()
                })
                .ToList();

            // Update the CollectionViewSource with the filtered data
            moviesViewSource.Source = filteredMovies;
        }
    }
}