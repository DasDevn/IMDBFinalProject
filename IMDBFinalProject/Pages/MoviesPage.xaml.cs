using IMDBFinalProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

            // Fetch all movies and their details
            var moviesWithDetails = context.Titles
                // Filter only titles where the type is "movie"
                .Where(t => t.TitleType == "movie")
                .Select(t => new
                {
                // Fetch the original title of the movie
                t.OriginalTitle,

                // Fetch the starting year of the movie
                t.StartYear,

                // Fetch and format the genres:
                // - If the movie has genres, join them into a comma-separated string
                // - If no genres exist, default to "No Genre"
                Genres = t.Genres.Any()
                    ? string.Join(", ", t.Genres.Select(g => g.Name))
                    : "No Genre",

                // Fetch and format the rating:
                // - If the movie has a rating, format it as "X.X/10"
                // - If no rating exists, default to "Not Rated"
                Rating = t.Rating != null
                    ? $"{t.Rating.AverageRating:0.0}/10"
                    : "Not Rated",

                // Fetch the actors/actresses associated with the movie:
                // - Filter principals by matching the TitleId
                // - Select their names and convert to a list
                Actors = context.Principals
                    .Where(p => p.TitleId == t.TitleId)
                    .Select(p => p.Name.PrimaryName)
                    .ToList(),

                // Fetch the writers associated with the movie:
                // - Select their primary names and convert to a list
                Writers = t.Names1
                    .Select(n => n.PrimaryName)
                    .ToList(),

                // Fetch the directors associated with the movie:
                // - Select their primary names and convert to a list
                Directors = t.Names
                    .Select(n => n.PrimaryName)
                    .ToList()
            })
            .ToList();

            // Assign the fetched and formatted movie data to the CollectionViewSource
            moviesViewSource.Source = moviesWithDetails;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the search text from the TextBox
            string searchText = SearchTextBox.Text.ToLower();

            // Fetch movies filtered by the search text
            var filteredMovies = context.Titles
                .Where(t => t.TitleType == "movie" && t.OriginalTitle.ToLower().Contains(searchText))
                .Select(t => new
                {
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
