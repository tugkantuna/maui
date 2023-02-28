namespace MauiBadge;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        BindingContext = new BadgeModel();
    }
}
