using IMDBFinalProject.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static IMDBFinalProject.Pages.TVShowsPage;

namespace IMDBFinalProject.Pages
{
	/// <summary>
	/// Interaction logic for TVShowDetailsPage.xaml
	/// </summary>
	public partial class TVShowDetailsPage : Page
	{
		private TVShow _selectedShow; // Store the selected show
		private CollectionViewSource tvShowViewSource = new CollectionViewSource(); // ViewSource for the data

		public TVShowDetailsPage(TVShow selectedShow)
		{
			InitializeComponent();
			_selectedShow = selectedShow;

			tvShowViewSource = (CollectionViewSource)FindResource(nameof(tvShowViewSource)); // Retrieve the ViewSource

			using (var context = new ImdbContext())
			{
				// Fetch data related to the TV show
				var actorCategories = new[] { "actor", "actress", "self" };

				var tvShowDetails = new
				{
					Name = _selectedShow.Name,
					Description = _selectedShow.Description,
					Actors = context.Principals
						.Where(p => p.TitleId == selectedShow.TitleId && actorCategories.Contains(p.JobCategory.ToLower()))
						.Select(p => p.Name.PrimaryName)
						.ToList(),
					Writers = context.Principals
						.Where(p => p.TitleId == selectedShow.TitleId && p.JobCategory == "writer")
						.Select(p => p.Name.PrimaryName)
						.ToList(),
					Seasons = context.Episodes
						.Where(e => e.ParentTitleId == selectedShow.TitleId)
						.GroupBy(e => e.SeasonNumber)
						.Select(g => g.Key)
						.ToList()
				};

				tvShowViewSource.Source = tvShowDetails; // Set the data source for the CollectionViewSource
			}
		}

		private void SeasonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (SeasonsListBox.SelectedItem is int selectedSeason)
			{
				NavigationService.Navigate(new EpisodesPage(selectedSeason, _selectedShow.TitleId));
			}
		}
	}
}