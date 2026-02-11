using MauiUltimateTemplate.Domain.Interfaces;

namespace MauiUltimateTemplate.Infrastructure.Device
{
    public class ConsoleConnectivityService : Domain.Interfaces.IConnectivity
    {
        // В консоли просто всегда возвращаем true 
        public bool IsConnected => true;

        public NetworkType CurrentNetworkType => NetworkType.WiFi;

        public event Action<bool>? ConnectivityChanged;

        // Пустой метод, так как в консоли мы не слушаем системные события сети
        public void Dispose() { }
    }
}
