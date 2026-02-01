using admin.Core.Abstractions;
using admin.Core.Enums;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace admin.Features.Home;
public partial class HomeViewModel : BaseViewModel
{
    //DataBindings - metrics
    public string PingValue => CalculatePing();
    public string HealthStatus => CurrentHealth switch
    {
        null => "Загрузка",
        { IsHealthy: true } => "Активен",
        _ => "Имеются проблемы"
    };
    public Brush AccentBrush => CurrentHealth switch
    {
        null => Brushes.DarkRed,
        { IsHealthy: true } => ApplicationAccentColorManager.SystemAccentBrush,
        _ => Brushes.DarkRed
    };
    public string Uptime => GetUptime();
    public string TotalRequests => CurrentHealth switch
    {
        null => "Загрузка",
        _ => CurrentHealth.TotalRequests.ToString()
    };
    public string ActiveConnections => CurrentHealth switch
    {
        null => "Загрузка",
        _ => CurrentHealth.ActiveConnections.ToString()
    };
    public string Problems => GetProblems();

    //Bindings - profile
    public string Username => ProfileInfo?.DisplayName ?? (string.IsNullOrEmpty(_currentUserService.UserName)
        ? $"@{_currentUserService.UserName}"
        : "@username");
    public string UsernameSub => string.IsNullOrEmpty(Username)
        ? ProfileInfo?.Username ?? "@username"
        : string.Empty;
    public string UserRole => _currentUserService.IsAdmin ? "Админ" : string.Empty;

    //Funcs
    private string CalculatePing()
    {
        var serverTime = CurrentHealth?.CurrentTime ?? DateTime.UtcNow;
        return $"{(int)DateTime.UtcNow.Subtract(serverTime).TotalMilliseconds} мс";
    }
    private string GetUptime()
    {
        if (CurrentHealth == null)
            return "Загрузка";

        int days = (int)CurrentHealth.Uptime.TotalDays;
        int hours = (int)CurrentHealth.Uptime.TotalHours % 24;
        int mins = (int)CurrentHealth.Uptime.TotalMinutes % 60;

        var dayWord = days switch
        {
            0 => "дней",
            1 => "день",
            < 5 => "дня",
            _ => "дней"
        };

        return $"{days} {dayWord}, {(hours < 10 ? "0" + hours : hours)}:{(mins < 10 ? "0" + mins : mins)}";
    }
    private string GetProblems()
    {
        if (CurrentHealth == null)
            return "";

        int counter = -1;

        foreach (var res in Enum.GetValues<UnhealthyReason>())
        {
            if ((CurrentHealth.UnhealthyReasons & res) == res)
                counter++;
        }

        var finalString = counter switch
        {
            0 => "Нет проблем",
            1 => $"{counter} проблема",
            <5 => $"{counter} проблемы",
            _ => $"{counter} проблем"
        };

        return finalString;
    }
}