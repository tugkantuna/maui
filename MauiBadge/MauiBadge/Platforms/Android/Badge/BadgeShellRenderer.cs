using Android.Graphics.Drawables;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using System.ComponentModel;

namespace MauiBadge.Platforms.Android;
public class BadgeShellRenderer : ShellRenderer
{
    BadgeShellItemRenderer _badgeShellItemRenderer;
    protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
    {
        _badgeShellItemRenderer = new BadgeShellItemRenderer(this);
         
        return _badgeShellItemRenderer;
    }

    protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
    {
        var view = new BottomNavViewAppearance(this, shellItem);
        view.evShellItemInit += View_evShellItemInit;

        return view;
    }

    private void View_evShellItemInit(object sender, EventArgs e)
    {
        _badgeShellItemRenderer.Init();
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

    }
}

internal class BottomNavViewAppearance : ShellBottomNavViewAppearanceTracker
{
    public event EventHandler evShellItemInit;
    public BottomNavViewAppearance(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
    {
    }
    public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
    {
        base.SetAppearance(bottomView, appearance);

        if (bottomView.LayoutParameters is LinearLayout.LayoutParams layoutParams1)
        {
            layoutParams1.SetMargins(20, 0, 20, 20);
            bottomView.LayoutParameters = layoutParams1;
        }
        bottomView.Background = Corner(appearance.EffectiveTabBarBackgroundColor);

        evShellItemInit?.Invoke(this, EventArgs.Empty);
    }
    private GradientDrawable Corner(Microsoft.Maui.Graphics.Color pColor, int pOpacity = 230)
    {
        var backgroundGradient = new GradientDrawable(GradientDrawable.Orientation.LeftRight, new int[] { pColor.ToAndroid(), pColor.ToAndroid() });
        backgroundGradient.SetCornerRadius(50);
        backgroundGradient.SetAlpha(pOpacity);
        return backgroundGradient;
    }
}