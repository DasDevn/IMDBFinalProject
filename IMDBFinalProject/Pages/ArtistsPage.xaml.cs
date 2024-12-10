using IMDBFinalProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
using static IMDBFinalProject.Pages.TVShowsPage;

namespace IMDBFinalProject.Pages
{
    /// <summary>
    /// Interaction logic for ArtistsPage.xaml
    /// </summary>
    public partial class ArtistsPage : Page
    {

        ImdbContext context = new ImdbContext();
        CollectionViewSource artistsViewSource = new CollectionViewSource();
        public ArtistsPage()
        {
            InitializeComponent();

            artistsViewSource = (CollectionViewSource)FindResource(nameof(artistsViewSource));

            //get from Names table
            var artistsResult = context.Names
                .Select(a => new
                {
                    a.NameId, //will be used to get artists works from Titles, via Known_For
                    a.PrimaryName,
                    a.BirthYear,
                    DeathYear = (a.DeathYear != null) ? a.DeathYear.ToString() : "not dead yet"
                })
                .ToList(); //using Async as the OrderBy can take some time, and causes program to hang

            //order after the fetch has happened, will hang app too long if done in-fetch
            //artistsViewSource.Source = artistsResult.OrderBy(a => a.PrimaryName ?? string.Empty); //also checking string.empty, as primary name is a nullable field 
            artistsViewSource.Source = artistsResult;
        }

        //private void SearchArtists_Click(object sender, RoutedEventArgs e)
        //{
        //    using (var context = new ImdbContext())
        //    {
        //        var nameToSearch = SearchTextBox.Text.Trim();

        //        var filteredArtists = context.Names
        //            .Where(a => a.PrimaryName.Contains(nameToSearch) && !a.PrimaryName.IsNullOrEmpty())
        //            .Select(a => new
        //            {
        //                a.NameId, //will be used to get artists works from Titles, via Known_For
        //                a.PrimaryName,
        //                a.BirthYear,
        //                a.DeathYear
        //            }).ToList();

        //        artistsViewSource.Source = filteredArtists;
        //    }
        //}
    }
}
