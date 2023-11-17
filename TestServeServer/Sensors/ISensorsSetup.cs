namespace TestIServe.Server.Sensors;

/// <summary>
/// Интерфейс настройки датчиков
/// </summary>
public interface ISensorsSetup
{
    /// <summary>
    /// Загрузка датчиков
    /// </summary>
    void LoadSensors();
}