using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace admin.Features.Common.Controls;

public partial class ProfileCard : UserControl
{
    public static readonly DependencyProperty AvatarProperty = DependencyProperty.Register(
       nameof(Avatar),
       typeof(BitmapImage),
       typeof(ProfileCard),
       new PropertyMetadata(null)
   );

    public BitmapImage Avatar
    {
        get => (BitmapImage)GetValue(AvatarProperty);
        set => SetValue(AvatarProperty, value);
    }

    public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
       nameof(DisplayName),
       typeof(string),
       typeof(ProfileCard),
       new PropertyMetadata("")
   );

    public string DisplayName
    {
        get => (string)GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(
       nameof(Username),
       typeof(string),
       typeof(ProfileCard),
       new PropertyMetadata("")
   );

    public string Username
    {
        get => (string)GetValue(UsernameProperty);
        set => SetValue(UsernameProperty, value);
    }

    public static readonly DependencyProperty UserRoleProperty = DependencyProperty.Register(
       nameof(UserRole),
       typeof(string),
       typeof(ProfileCard),
       new PropertyMetadata("")
   );

    public string UserRole
    {
        get => (string)GetValue(UserRoleProperty);
        set => SetValue(UserRoleProperty, value);
    }
}
