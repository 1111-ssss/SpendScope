using admin.Core.Enums;

namespace admin.Features.Metrics;

public partial class MetricsViewModel
{
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
            < 5 => $"{counter} проблемы",
            _ => $"{counter} проблем"
        };

        return finalString;
    }
}