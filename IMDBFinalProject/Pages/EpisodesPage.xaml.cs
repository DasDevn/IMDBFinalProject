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
	/// Interaction logic for EpisodesPage.xaml
	/// </summary>
	public partial class EpisodesPage : Page
	{
		public EpisodesPage(int season, string parentTitleId)
		{
			InitializeComponent();

			using (var context = new ImdbContext())
			{
				var episodes = context.Episodes
					.Where(e => e.ParentTitleId == parentTitleId && e.SeasonNumber == season)
					.Select(e => new
					{
						e.EpisodeNumber,
						e.Title.PrimaryTitle
					})
					.ToList();

				DataContext = new { Episodes = episodes };
			}
		}
	}
}
