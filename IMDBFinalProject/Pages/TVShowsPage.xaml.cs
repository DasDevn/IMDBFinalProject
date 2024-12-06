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
    /// Interaction logic for TVShowsPage.xaml
    /// </summary>
    public partial class TVShowsPage : Page
    {
        public TVShowsPage()
        {
            InitializeComponent();
			// Fetch TV shows from the database
			using (var context = new ImdbContext())
			{
				// Query to get TV shows from the Titles table
				var tvShows = context.Titles
					.Where(t => t.TitleType == "tvSeries") // Filter for TV series
					.Select(t => new TVShow
					{
						Name = t.PrimaryTitle,
						Description = t.OriginalTitle ?? "No description available" // Use original title as a fallback
					})
					.ToList();

				TVShowsListBox.ItemsSource = tvShows;
			}
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			// Fetch TV shows from the database
			using (var context = new ImdbContext())
			{
				// Query to get TV shows from the Titles table
				var tvShows = context.Titles
					.Where(t => t.TitleType == "tvSeries") // Filter for TV series
					.Where(t => t.PrimaryTitle.Contains(SearchTextBox.Text)) // Filter for search term
					.Select(t => new TVShow
					{
						Name = t.PrimaryTitle,
						Description = t.OriginalTitle ?? "No description available" // Use original title as a fallback
					})
					.ToList();

				TVShowsListBox.ItemsSource = tvShows;
			}
		}
		// TVShow class to represent items in the ListBox
		public class TVShow
		{
			public string? Name { get; set; }
			public string? Description { get; set; }
		}
	}
}
