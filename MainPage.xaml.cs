using Syncfusion.Maui.Graphics.Internals;
using Syncfusion.Maui.SignaturePad;
using Microsoft.Maui.Controls;
//using Microsoft.Maui.Essentials;
using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections.ObjectModel;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace Examen_Grupo2
{
    public partial class MainPage : ContentPage
    {
        Controllers.FirmasController controller;
        private const string GoogleMapsApiKey = "AIzaSyCUM-myzK7lScxEnEDRG2NlbpwXg1A0h0k";

        public MainPage()
        {
            InitializeComponent();
            SfSignaturePad signaturePad = new SfSignaturePad();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

                if (location != null)
                {
                    LatitudEntry.Text = location.Latitude.ToString();
                    LongitudEntry.Text = location.Longitude.ToString();

                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        //PlaceEntry.Text = placemark.Thoroughfare + ", " + placemark.Locality; // Ejemplo de construcción de la descripción del lugar
                    }
                    else
                    {
                        //PlaceEntry.Text = "No location description available";
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Unable to get location", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            await CheckGpsStatusAsync();
        }

        private async System.Threading.Tasks.Task CheckGpsStatusAsync()
        {
            var locationStatus = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));

            if (locationStatus == null)
            {
                await DisplayAlert("GPS Not Enabled", "Please enable GPS to use this app.", "OK");
            }
        }

        private async Task<String> ImageSourceToBase64(ImageSource imageSource)
        {
            if (imageSource is StreamImageSource streamImageSource)
            {
                using (var stream = await streamImageSource.Stream(CancellationToken.None))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        //return Convert.ToBase64String(imageBytes);
                    }
                }
            }
            return null;
        }

        private void OnClearButtonClicked(object? sender, EventArgs e)
        {
            signaturePad.Clear();
        }
    }

}
