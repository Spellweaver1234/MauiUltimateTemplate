using MauiUltimateTemplate.Domain.Interfaces;

using MauiConnectivity = Microsoft.Maui.Networking.Connectivity;

namespace MauiUltimateTemplate.Infrastructure.Device
{
    public class MauiConnectivityService : Domain.Interfaces.IConnectivity, IDisposable
    {
        public MauiConnectivityService()
        {
            MauiConnectivity.Current.ConnectivityChanged += OnMauiConnectivityChanged;
        }

        public bool IsConnected =>
            MauiConnectivity.Current.NetworkAccess == NetworkAccess.Internet;

        public NetworkType CurrentNetworkType =>
            MapNetworkType(MauiConnectivity.Current.ConnectionProfiles);

        public event Action<bool> ConnectivityChanged;

        private void OnMauiConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            ConnectivityChanged?.Invoke(e.NetworkAccess == NetworkAccess.Internet);
        }

        private NetworkType MapNetworkType(IEnumerable<ConnectionProfile> profiles)
        {
            // Проверка на null и пустоту для безопасности
            if (profiles == null || !profiles.Any()) return NetworkType.None;

            var profile = profiles.First();
            return profile switch
            {
                ConnectionProfile.WiFi => NetworkType.WiFi,
                ConnectionProfile.Cellular => NetworkType.Cellular,
                ConnectionProfile.Ethernet => NetworkType.Ethernet,
                _ => NetworkType.Unknown
            };
        }

        public void Dispose()
        {
            MauiConnectivity.Current.ConnectivityChanged -= OnMauiConnectivityChanged;
        }
    }
}
