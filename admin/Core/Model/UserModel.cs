using System.Windows.Media.Imaging;

namespace admin.Core.Model;

public class UserModel
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public BitmapImage Avatar { get; set; }
    public string Role { get; set; }
    public UserModel(string username, string displayName, BitmapImage avatar, string role)
    {
        Username = username;
        DisplayName = displayName;
        Avatar = avatar;
        Role = role;
    }
}
