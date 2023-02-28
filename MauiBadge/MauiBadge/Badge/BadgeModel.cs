using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiBadge;

partial class BadgeModel : ObservableObject, IRecipient<BadgeMessage>
{
    [ObservableProperty]
    Color badgeColor;

    [ObservableProperty]
    Color textColor;

    [ObservableProperty]
    Color titleBadgeColor;

    [ObservableProperty]
    string badgeText;

    [ObservableProperty]
    string badgeTitleText;

    public BadgeModel()
    {
        WeakReferenceMessenger.Default.Register<BadgeMessage>(this);

        TextColor = Colors.White;
        BadgeColor = Colors.Red;
        TitleBadgeColor = Colors.Green;
        BadgeText = "0";
        BadgeTitleText = (Convert.ToInt32(BadgeText) * 50).ToString("N2") + " TL";
    }

    public void Receive(BadgeMessage message)
    {
        BadgeText = message.Value;
        BadgeTitleText = (Convert.ToInt32(message.Value) * 50).ToString("N2") + " TL";
    }

    
}
