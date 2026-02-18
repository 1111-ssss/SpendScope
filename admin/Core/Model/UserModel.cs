using System.Windows.Media.Imaging;

namespace admin.Core.Model;

public class UserModel
{
    public int UserId { get; }
    public string DisplayName { get; }
    public string Username { get; }
    public BitmapImage? Avatar { get; set; }
    public DateTime LastOnline { get; }

    public UserModel(
        int userId,
        string displayName,
        string username,
        BitmapImage? avatar,
        DateTime lastOnline
    )
    {
        UserId = userId;
        DisplayName = displayName;
        Username = username;
        Avatar = avatar;
        LastOnline = lastOnline;
    }
}
