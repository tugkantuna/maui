using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiBadge;

public class BadgeMessage : ValueChangedMessage<string>
{
    public BadgeMessage(string value) : base(value)
    {
    }
}
