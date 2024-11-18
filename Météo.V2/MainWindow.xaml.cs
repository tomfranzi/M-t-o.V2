using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace MeteoApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            GetWeather();
        }

        public async Task GetWeather()
        {
            HttpClient client = new HttpClient();
            // Utilisez l'URL correcte pour obtenir des données JSON et assurez-vous d'utiliser un format d'API correct.
            HttpResponseMessage response = await client.GetAsync("https://www.prevision-meteo.ch/api/forecast"); // Remplacer par la bonne URL si nécessaire

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                WeatherForecast forecast = JsonConvert.DeserializeObject<WeatherForecast>(content);

                // Extraire les prévisions pour 00h00 et 10h00
                var forecastAt0h00 = forecast.Forecast.FirstOrDefault(f => f.Time == "00:00");
                var forecastAt10h00 = forecast.Forecast.FirstOrDefault(f => f.Time == "10:00");

                if (forecastAt0h00 != null)
                {
                    // Mettre à jour les TextBlock pour 00h00
                    TemperatureTextBlock.Text = $"00h00 Temp: {forecastAt0h00.Temperature} °C";
                    FeelsLikeTextBlock.Text = $"Ressenti : {forecastAt0h00.Temperature} °C"; // Remplacer si vous avez des données de ressenti
                    WeatherDescription.Text = forecastAt0h00.Description;
                    HumidityTextBlock.Text = $"Humidité : {forecastAt0h00.Humidity}%";
                }

                if (forecastAt10h00 != null)
                {
                    // Mettre à jour les TextBlock pour 10h00
                    MaxTemperatureTextBlock.Text = $"10h00 Temp: {forecastAt10h00.Temperature} °C";
                    WeatherDescription.Text += $" | 10h00: {forecastAt10h00.Description}"; // Vous pouvez concaténer ou utiliser d'autres TextBlocks
                    HumidityTextBlock.Text = $"Humidité à 10h00 : {forecastAt10h00.Humidity}%";
                }
            }
            else
            {
                // Gestion d’erreur si la requête API échoue
                TemperatureTextBlock.Text = "Erreur de chargement des données";
                FeelsLikeTextBlock.Text = "";
                WeatherDescription.Text = "";
                HumidityTextBlock.Text = "";
                MaxTemperatureTextBlock.Text = "";
            }
        }

        public class WeatherForecast
        {
            public Forecast[] Forecast { get; set; }
        }

        public class Forecast
        {
            public string Time { get; set; }
            public double Temperature { get; set; }
            public int Humidity { get; set; }
            public string Description { get; set; }
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
    }
}