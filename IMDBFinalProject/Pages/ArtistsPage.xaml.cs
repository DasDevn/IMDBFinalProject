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

            using (var context = new ImdbContext())
            {
                var artists = context.Names
                    .Select(a => new
                    {
                        a.NameId,
                        a.PrimaryName,
                        a.BirthYear,
                        a.DeathYear,
                    })
                    //.OrderBy(a => a.PrimaryName) //order by name
                    .ToList();
                               
                artistsViewSource.Source = artists;
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
}
