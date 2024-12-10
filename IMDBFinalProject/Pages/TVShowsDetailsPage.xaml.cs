using IMDBFinalProject.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static IMDBFinalProject.Pages.TVShowsPage;

namespace IMDBFinalProject.Pages
{
	/// <summary>
	/// Interaction logic for TVShowDetailsPage.xaml
	/// </summary>
	public partial class TVShowDetailsPage : Page
	{
		private TVShow _selectedShow; // Private field to store the selected show

		public TVShowDetailsPage(TVShow selectedShow)
		{
			InitializeComponent();
			_selectedShow = selectedShow; // Assign to the private field

			using (var context = new ImdbContext())
			{
				// Fetch Actors/Actresses
				// Define all possible actor-related job categories
				var actorCategories = new[] { "actor", "actress", "self" };

				// Fetch Actors
				var actors = context.Principals
					.Where(p => p.TitleId == selectedShow.TitleId && actorCategories.Contains(p.JobCategory.ToLower()))
					.Select(p => p.Name.PrimaryName)
					.ToList();

				// Fetch Writers
				var writers = context.Principals
					.Where(p => p.TitleId == selectedShow.TitleId && p.JobCategory == "writer") // Correct property usage
					.Select(p => p.Name.PrimaryName) // Fetch writer names
					.ToList();

				// Fetch Seasons
				var seasons = context.Episodes
					.Where(e => e.ParentTitleId == selectedShow.TitleId) // Use TitleId for parent
					.GroupBy(e => e.SeasonNumber)
					.Select(g => g.Key) // Select distinct season numbers
					.ToList();

				// Set DataContext with all the fetched data
				DataContext = new
				{
					Name = selectedShow.Name, // Title of the TV show
					Description = selectedShow.Description, // TV show description
					Actors = actors, // List of actors/actresses
					Writers = writers, // List of writers
					Seasons = seasons // List of seasons
				};
			}
		}

		// Handle season selection
		private void SeasonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (SeasonsListBox.SelectedItem is int selectedSeason)
			{
				// Navigate to EpisodesPage, passing the season number and TitleId
				NavigationService.Navigate(new EpisodesPage(selectedSeason, _selectedShow.TitleId));
			}
		}
	}
}
