namespace MauiUltimateTemplate.Domain.Interfaces
{
    public interface IConnectivity
    {
        // Свойство: есть интернет или нет
        bool IsConnected { get; }

        // Тип подключения (Wi-Fi, Cellular и т.д.)
        NetworkType CurrentNetworkType { get; }

        // Событие для отслеживания изменений в реальном времени
        event Action<bool> ConnectivityChanged;
    }

    // Перечисление тоже кладем в Domain, так как это часть бизнес-определений
    public enum NetworkType
    {
        None,
        WiFi,
        Cellular,
        Ethernet,
        Unknown
    }
}
