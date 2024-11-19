using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
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

namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        private const string FavoritesFilePath = "cities.txt";

        public MainWindow()
        {
            InitializeComponent();
            LoadFavorites();  // Charger les villes favorites au démarrage
        }

        // Méthode pour récupérer la météo d'une ville
        public async Task GetWeatherForCity(string cityName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://www.prevision-meteo.ch/services/json/{cityName}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    MessageBox.Show("Aucune donnée reçue.");
                    return;
                }

                Root root;
                try
                {
                    root = JsonConvert.DeserializeObject<Root>(content);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de désérialisation : {ex.Message}");
                    return;
                }

                // Affichage des données de la météo
                UpdateWeatherUI(root);
            }
            else
            {
                MessageBox.Show("Erreur lors de la récupération des données.");
            }
        }

        // Mise à jour des éléments UI avec les données météo
        private void UpdateWeatherUI(Root root)
        {
            // Récupération des informations météorologiques
            CityInfo city_info = root.city_info;
            CurrentCondition current_condition = root.current_condition;
            FcstDay1 fcst_fday = root.fcst_day_1;
            FcstDay2 fcst_sday = root.fcst_day_2;
            FcstDay3 fcst_tday = root.fcst_day_3;

            // Mise à jour des TextBlocks avec les données
            TB_City.Text = city_info.name;
            TB_Temp.Text = $"{current_condition.tmp}°";
            TB_Fcst_Fday.Text = fcst_fday.day_long;
            TB_Fcst_Sday.Text = fcst_sday.day_long;
            TB_Fcst_Tday.Text = fcst_tday.day_long;
            TB_Tempmin_Fday.Text = $"{fcst_fday.tmin}°";
            TB_Tempmin_Sday.Text = $"{fcst_sday.tmin}°";
            TB_Tempmin_Tday.Text = $"{fcst_tday.tmin}°";
            TB_Tempmax_Fday.Text = $"{fcst_fday.tmax}°";
            TB_Tempmax_Sday.Text = $"{fcst_sday.tmax}°";
            TB_Tempmax_Tday.Text = $"{fcst_tday.tmax}°";

            // Mise à jour des icônes
            Uri uriCurrentIcon = new Uri(current_condition.icon_big);
            ImgDay.Source = new BitmapImage(uriCurrentIcon);

            Uri uriFDayIcon = new Uri(fcst_fday.icon_big);
            ImgDay1.Source = new BitmapImage(uriFDayIcon);

            Uri uriSDayIcon = new Uri(fcst_sday.icon_big);
            ImgDay2.Source = new BitmapImage(uriSDayIcon);

            Uri uriTDayIcon = new Uri(fcst_tday.icon_big);
            ImgDay3.Source = new BitmapImage(uriTDayIcon);
        }

        // Événement pour gérer la sélection de ville dans la ComboBox
        private async void CB_Cities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Cities.SelectedItem != null)
            {
                string selectedCity = CB_Cities.SelectedItem.ToString();
                await GetWeatherForCity(selectedCity);  // Récupérer la météo pour la ville sélectionnée
            }
        }

        // Méthode pour ajouter une nouvelle ville à la ComboBox et au fichier
        private void BTN_Add_Click(object sender, RoutedEventArgs e)
        {
            string newCity = TB_EnterCity.Text.Trim(); // Récupérer le texte saisi

            if (!string.IsNullOrEmpty(newCity))
            {
                if (!CB_Cities.Items.Contains(newCity))
                {
                    CB_Cities.Items.Add(newCity); // Ajouter la ville à la ComboBox
                    AddCityToFile(newCity);  // Ajouter la ville au fichier
                    TB_EnterCity.Clear(); // Effacer le champ de saisie
                    MessageBox.Show($"{newCity} a été ajoutée aux favoris.");
                }
                else
                {
                    MessageBox.Show("Cette ville est déjà dans les favoris.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer une ville valide.");
            }
        }

        // Méthode pour ajouter la ville au fichier des favoris
        private void AddCityToFile(string cityName)
        {
            string path = FavoritesFilePath;
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(cityName);
            }
        }

        // Méthode pour charger les villes favorites depuis le fichier
        private void LoadFavorites()
        {
            string path = FavoritesFilePath;
            if (File.Exists(path))
            {
                var cities = File.ReadAllLines(path);
                foreach (var city in cities)
                {
                    if (!CB_Cities.Items.Contains(city) && !string.IsNullOrWhiteSpace(city))
                    {
                        CB_Cities.Items.Add(city);
                    }
                }
            }
        }
    }

    // Classes pour désérialiser la réponse de l'API

    public class Root
    {
        public CityInfo city_info { get; set; }
        public CurrentCondition current_condition { get; set; }
        public FcstDay1 fcst_day_1 { get; set; }
        public FcstDay2 fcst_day_2 { get; set; }
        public FcstDay3 fcst_day_3 { get; set; }
    }

    public class CityInfo
    {
        public string name { get; set; }
    }

    public class CurrentCondition
    {
        public double tmp { get; set; }
        public string icon_big { get; set; }
    }

    public class FcstDay1
    {
        public string day_long { get; set; }
        public double tmin { get; set; }
        public double tmax { get; set; }
        public string icon_big { get; set; }
    }

    public class FcstDay2
    {
        public string day_long { get; set; }
        public double tmin { get; set; }
        public double tmax { get; set; }
        public string icon_big { get; set; }
    }

    public class FcstDay3
    {
        public string day_long { get; set; }
        public double tmin { get; set; }
        public double tmax { get; set; }
        public string icon_big { get; set; }
    }
}
public class _0H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _10H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _11H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _12H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _13H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _14H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _15H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _16H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public int RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _17H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _18H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _19H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _1H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _20H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _21H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _22H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _23H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _2H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _3H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _4H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _5H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _6H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _7H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _8H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class _9H00
{
    public string ICON { get; set; }
    public string CONDITION { get; set; }
    public string CONDITION_KEY { get; set; }
    public double TMP2m { get; set; }
    public double DPT2m { get; set; }
    public double WNDCHILL2m { get; set; }
    public object HUMIDEX { get; set; }
    public double RH2m { get; set; }
    public double PRMSL { get; set; }
    public double APCPsfc { get; set; }
    public double WNDSPD10m { get; set; }
    public double WNDGUST10m { get; set; }
    public double WNDDIR10m { get; set; }
    public string WNDDIRCARD10 { get; set; }
    public double ISSNOW { get; set; }
    public string HCDC { get; set; }
    public string MCDC { get; set; }
    public string LCDC { get; set; }
    public double HGT0C { get; set; }
    public double KINDEX { get; set; }
    public string CAPE180_0 { get; set; }
    public double CIN180_0 { get; set; }
}

public class CityInfo
{
    public string name { get; set; }
    public string country { get; set; }
    public string latitude { get; set; }
    public string longitude { get; set; }
    public string elevation { get; set; }
    public string sunrise { get; set; }
    public string sunset { get; set; }
}

public class CurrentCondition
{
    public string date { get; set; }
    public string hour { get; set; }
    public int tmp { get; set; }
    public int wnd_spd { get; set; }
    public int wnd_gust { get; set; }
    public string wnd_dir { get; set; }
    public double pressure { get; set; }
    public int humidity { get; set; }
    public string condition { get; set; }
    public string condition_key { get; set; }
    public string icon { get; set; }
    public string icon_big { get; set; }
}

public class FcstDay0
{
    public string date { get; set; }
    public string day_short { get; set; }
    public string day_long { get; set; }
    public int tmin { get; set; }
    public int tmax { get; set; }
    public string condition { get; set; }
    public string condition_key { get; set; }
    public string icon { get; set; }
    public string icon_big { get; set; }
    public HourlyData hourly_data { get; set; }
}

public class FcstDay1
{
    public string date { get; set; }
    public string day_short { get; set; }
    public string day_long { get; set; }
    public int tmin { get; set; }
    public int tmax { get; set; }
    public string condition { get; set; }
    public string condition_key { get; set; }
    public string icon { get; set; }
    public string icon_big { get; set; }
    public HourlyData hourly_data { get; set; }
}

public class FcstDay2
{
    public string date { get; set; }
    public string day_short { get; set; }
    public string day_long { get; set; }
    public int tmin { get; set; }
    public int tmax { get; set; }
    public string condition { get; set; }
    public string condition_key { get; set; }
    public string icon { get; set; }
    public string icon_big { get; set; }
    public HourlyData hourly_data { get; set; }
}

public class FcstDay3
{
    public string date { get; set; }
    public string day_short { get; set; }
    public string day_long { get; set; }
    public int tmin { get; set; }
    public int tmax { get; set; }
    public string condition { get; set; }
    public string condition_key { get; set; }
    public string icon { get; set; }
    public string icon_big { get; set; }
    public HourlyData hourly_data { get; set; }
}

public class FcstDay4
{
    public string date { get; set; }
    public string day_short { get; set; }
    public string day_long { get; set; }
    public int tmin { get; set; }
    public int tmax { get; set; }
    public string condition { get; set; }
    public string condition_key { get; set; }
    public string icon { get; set; }
    public string icon_big { get; set; }
    public HourlyData hourly_data { get; set; }
}

public class ForecastInfo
{
    public object latitude { get; set; }
    public object longitude { get; set; }
    public string elevation { get; set; }
}

public class HourlyData
{
    [JsonProperty("0H00")]
    public _0H00 _0H00 { get; set; }

    [JsonProperty("1H00")]
    public _1H00 _1H00 { get; set; }

    [JsonProperty("2H00")]
    public _2H00 _2H00 { get; set; }

    [JsonProperty("3H00")]
    public _3H00 _3H00 { get; set; }

    [JsonProperty("4H00")]
    public _4H00 _4H00 { get; set; }

    [JsonProperty("5H00")]
    public _5H00 _5H00 { get; set; }

    [JsonProperty("6H00")]
    public _6H00 _6H00 { get; set; }

    [JsonProperty("7H00")]
    public _7H00 _7H00 { get; set; }

    [JsonProperty("8H00")]
    public _8H00 _8H00 { get; set; }

    [JsonProperty("9H00")]
    public _9H00 _9H00 { get; set; }

    [JsonProperty("10H00")]
    public _10H00 _10H00 { get; set; }

    [JsonProperty("11H00")]
    public _11H00 _11H00 { get; set; }

    [JsonProperty("12H00")]
    public _12H00 _12H00 { get; set; }

    [JsonProperty("13H00")]
    public _13H00 _13H00 { get; set; }

    [JsonProperty("14H00")]
    public _14H00 _14H00 { get; set; }

    [JsonProperty("15H00")]
    public _15H00 _15H00 { get; set; }

    [JsonProperty("16H00")]
    public _16H00 _16H00 { get; set; }

    [JsonProperty("17H00")]
    public _17H00 _17H00 { get; set; }

    [JsonProperty("18H00")]
    public _18H00 _18H00 { get; set; }

    [JsonProperty("19H00")]
    public _19H00 _19H00 { get; set; }

    [JsonProperty("20H00")]
    public _20H00 _20H00 { get; set; }

    [JsonProperty("21H00")]
    public _21H00 _21H00 { get; set; }

    [JsonProperty("22H00")]
    public _22H00 _22H00 { get; set; }

    [JsonProperty("23H00")]
    public _23H00 _23H00 { get; set; }
}

public class Root
{
    public CityInfo city_info { get; set; }
    public ForecastInfo forecast_info { get; set; }
    public CurrentCondition current_condition { get; set; }
    public FcstDay0 fcst_day_0 { get; set; }
    public FcstDay1 fcst_day_1 { get; set; }
    public FcstDay2 fcst_day_2 { get; set; }
    public FcstDay3 fcst_day_3 { get; set; }
    public FcstDay4 fcst_day_4 { get; set; }
}
