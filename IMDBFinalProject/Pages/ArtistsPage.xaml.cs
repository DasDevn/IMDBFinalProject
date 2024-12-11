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

            using (var context = new ImdbContext())
            {
                try
                {
                    var artists = context.Names
                        .Take(50) //taking too large a number slows down the speed to read from db
                        .Where(n => n.PrimaryName != "") //ignore entries with no name
                        .Where(n =>
                        n.PrimaryProfession != null //where acting-related job category
                        || n.PrimaryProfession.ToLower() == "actor"
                        || n.PrimaryProfession.ToLower() == "actress")
                        .Select(n => new //select artist info
                        {
                            n.NameId, //will be used when getting related info when user selects given artist
                            n.PrimaryName, //artist name
                            n.BirthYear, //artist year of birth
                            n.DeathYear, //artist year of death

                            ArtistsWorkCount = context.Principals
                            .Where(p => p.NameId == n.NameId) //where id matches in 
                            .Where(p => //where acting-related job category
                            p.JobCategory != null
                            || p.JobCategory.ToLower() == "actor"
                            || p.JobCategory.ToLower() == "actress")
                            .Select(p => p.NameId)
                            .Count(), //get the count of all valid jobs

                            ArtistsWork = context.Principals //ArtistsWork, holding characters + work character is from
                            .Where(p => p.NameId == n.NameId) //where id matches in 
                            .Where(p => //where acting-related job category
                            p.JobCategory != null
                        || p.JobCategory.ToLower() == "actor"
                        || p.JobCategory.ToLower() == "actress")
                            .Select(p => new
                            {
                                Media = context.Titles //media character is from
                                .Where(t => t.TitleId == p.TitleId && n.NameId == p.NameId) //where titleids match and nameids match
                                .Where(t => t.PrimaryTitle != null)
                                .Select(t =>
                                    t.PrimaryTitle //media title
                                ).ToList(),
                                Character = context.Titles //media character is from
                                .Where(t => t.TitleId == p.TitleId && n.NameId == p.NameId) //where titleids match and nameids match
                                .Where(t => t.PrimaryTitle != null)
                                .Select(t =>
                                    p.Characters //related character
                                ).ToList()
                            }).ToList()

                        }).ToList()
                        .OrderBy(a => a.PrimaryName) //order by name
                        .ToList();

                    artistsViewSource.Source = artists;
                }
                catch (Microsoft.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }
        private void SearchArtists_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new ImdbContext())
                {
                    var nameToSearch = SearchTextBox.Text.Trim();

                    var filteredArtists = context.Names
                        .Where(a => a.PrimaryName.Contains(nameToSearch))
                        .Where(a =>
                        a.PrimaryProfession.ToLower() == "actor"
                        || a.PrimaryProfession.ToLower() == "actress")
                        .Select(a => new
                        {
                            a.NameId, //will be used to get artists works from Titles, via Known_For
                            a.PrimaryName,
                            a.BirthYear,
                            a.DeathYear
                        })
                        .OrderBy(a => a.PrimaryName)
                        .ToList();

                    artistsViewSource.Source = filteredArtists;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
