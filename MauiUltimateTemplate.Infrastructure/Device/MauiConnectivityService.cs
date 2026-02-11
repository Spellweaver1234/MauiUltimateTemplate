using IConnectivity = Application.Domain.Interfaces.IConnectivity;

namespace MauiUltimateTemplate.Infrastructure.Device
{
    public class MauiConnectivityService : IConnectivity, IDisposable
    {
        public MauiConnectivityService()
        {
            // Подписываемся на системное событие MAUI
            Connectivity.Current.ConnectivityChanged += OnMauiConnectivityChanged;
        }

        public bool IsConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        public NetworkType CurrentNetworkType => MapNetworkType(Connectivity.Current.ConnectionProfiles);

        public event Action<bool> ConnectivityChanged;

        private void OnMauiConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            // Уведомляем наш Domain о том, что статус изменился
            ConnectivityChanged?.Invoke(e.NetworkAccess == NetworkAccess.Internet);
        }

        // Вспомогательный маппинг типов (из MAUI в наш Domain)
        private NetworkType MapNetworkType(IEnumerable<ConnectionProfile> profiles)
        {
            if (!profiles.Any()) return NetworkType.None;

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
            Connectivity.Current.ConnectivityChanged -= OnMauiConnectivityChanged;
        }
    }
}
