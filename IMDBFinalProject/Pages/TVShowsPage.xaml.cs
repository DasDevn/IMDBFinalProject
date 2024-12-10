using IMDBFinalProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IMDBFinalProject.Pages
{
	public partial class TVShowsPage : Page
	{
		public TVShowsPage()
		{
			InitializeComponent();

			// Fetch TV shows from the database
			using (var context = new ImdbContext())
			{
				var tvShows = context.Titles
					.Where(t => t.TitleType == "tvSeries")
					.Select(t => new
					{
						t.TitleId,
						t.PrimaryTitle,
						t.OriginalTitle,
						Genres = t.Genres.Select(g => g.Name),
						t.StartYear,
						AverageRating = context.Ratings
							.Where(r => r.TitleId == t.TitleId)
							.Select(r => r.AverageRating)
							.FirstOrDefault(),
						Actors = context.Principals
							.Where(p => p.TitleId == t.TitleId &&
										(p.JobCategory.ToLower() == "actor" ||
										 p.JobCategory.ToLower() == "actress" ||
										 p.JobCategory.ToLower() == "self"))
							.Select(p => p.Name.PrimaryName)
							.ToList(),
						Writers = context.Principals
							.Where(p => p.TitleId == t.TitleId && p.JobCategory.ToLower() == "writer")
							.Select(p => p.Name.PrimaryName)
							.ToList(),
						Seasons = context.Episodes
							.Where(e => e.ParentTitleId == t.TitleId)
							.GroupBy(e => e.SeasonNumber)
							.Where(g => g.Key.HasValue) // Ensure non-null seasons
							.Select(g => new
							{
								SeasonNumber = g.Key.Value,
								Episodes = g.OrderBy(e => e.EpisodeNumber)
									.Select(e => e.Title != null && !string.IsNullOrEmpty(e.Title.PrimaryTitle)
										? $"Episode {e.EpisodeNumber}: {e.Title.PrimaryTitle}" // Combine episode number with title
										: $"Episode {e.EpisodeNumber}") // Use episode number if title is missing
									.ToList()

			})
							.OrderBy(s => s.SeasonNumber)
							.ToList()
					})
					.AsEnumerable()
					.Select(t => new TVShow
					{
						TitleId = t.TitleId,
						Name = t.PrimaryTitle ?? "Unknown Title",
						Description = t.OriginalTitle ?? "No description available",
						Genre = t.Genres.Any() ? string.Join(", ", t.Genres) : "Unknown",
						ReleaseDate = t.StartYear.HasValue ? t.StartYear.Value.ToString() : "Unknown",
						Rating = t.AverageRating.HasValue ? t.AverageRating.Value.ToString("0.0") : "Not Rated",
						Actors = t.Actors,
						Writers = t.Writers,
						Seasons = t.Seasons.Select(s => new Season
						{
							SeasonName = $"Season {s.SeasonNumber}",
							Episodes = s.Episodes
						}).ToList()
					})
					.ToList();

				TVShowsListBox.ItemsSource = tvShows;
			}
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			using (var context = new ImdbContext())
			{
				var searchTerm = SearchTextBox.Text.Trim();

				var tvShows = context.Titles
					.Where(t => t.TitleType == "tvSeries" && t.PrimaryTitle.Contains(searchTerm))
					.Select(t => new
					{
						t.TitleId,
						t.PrimaryTitle,
						t.OriginalTitle,
						Genres = t.Genres.Select(g => g.Name),
						t.StartYear,
						AverageRating = context.Ratings
							.Where(r => r.TitleId == t.TitleId)
							.Select(r => r.AverageRating)
							.FirstOrDefault(),
						Actors = context.Principals
							.Where(p => p.TitleId == t.TitleId &&
										(p.JobCategory.ToLower() == "actor" ||
										 p.JobCategory.ToLower() == "actress" ||
										 p.JobCategory.ToLower() == "self"))
							.Select(p => p.Name.PrimaryName)
							.ToList(),
						Writers = context.Principals
							.Where(p => p.TitleId == t.TitleId && p.JobCategory.ToLower() == "writer")
							.Select(p => p.Name.PrimaryName)
							.ToList(),
						Seasons = context.Episodes
							.Where(e => e.ParentTitleId == t.TitleId)
							.GroupBy(e => e.SeasonNumber)
							.Where(g => g.Key.HasValue) // Ensure non-null seasons
							.Select(g => new
							{
								SeasonNumber = g.Key.Value,
								Episodes = g.OrderBy(e => e.EpisodeNumber)
									.Select(e => e.Title != null && !string.IsNullOrEmpty(e.Title.PrimaryTitle)
										? $"Episode {e.EpisodeNumber}: {e.Title.PrimaryTitle}" // Combine episode number with title
										: $"Episode {e.EpisodeNumber}") // Use episode number if title is missing
									.ToList()
			})
							.OrderBy(s => s.SeasonNumber)
							.ToList()
					})
					.AsEnumerable()
					.Select(t => new TVShow
					{
						TitleId = t.TitleId,
						Name = t.PrimaryTitle ?? "Unknown Title",
						Description = t.OriginalTitle ?? "No description available",
						Genre = t.Genres.Any() ? string.Join(", ", t.Genres) : "Unknown",
						ReleaseDate = t.StartYear.HasValue ? t.StartYear.Value.ToString() : "Unknown",
						Rating = t.AverageRating.HasValue ? t.AverageRating.Value.ToString("0.0") : "Not Rated",
						Actors = t.Actors,
						Writers = t.Writers,
						Seasons = t.Seasons.Select(s => new Season
						{
							SeasonName = $"Season {s.SeasonNumber}",
							Episodes = s.Episodes
						}).ToList()
					})
					.ToList();

				TVShowsListBox.ItemsSource = tvShows;
			}
		}

		public class TVShow
		{
			public string TitleId { get; set; }
			public string? Name { get; set; }
			public string? Description { get; set; }
			public string? Genre { get; set; }
			public string? ReleaseDate { get; set; }
			public string? Rating { get; set; }
			public List<string>? Actors { get; set; }
			public List<string>? Writers { get; set; }
			public List<Season> Seasons { get; set; }
		}

		public class Season
		{
			public string SeasonName { get; set; }
			public List<string> Episodes { get; set; }
		}
	}
}