using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace MauiBadge.Platforms.iOS;

public class BadgeShellRenderer : ShellRenderer
{
    protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
    {
        return new TabBarShellAppearanceTracker();
    }
    
    protected override IShellItemRenderer CreateShellItemRenderer(ShellItem item)
    {
        return new BadgeShellItemRenderer(this) { ShellItem = item };
    }

}

public class TabBarShellAppearanceTracker : ShellTabBarAppearanceTracker, IShellTabBarAppearanceTracker
{
    private IShellAppearanceElement _shellAppearance;
    void IShellTabBarAppearanceTracker.UpdateLayout(UIKit.UITabBarController controller)
    {
        try
        {
            var backgroundColor = _shellAppearance.EffectiveTabBarBackgroundColor.ToUIColor();
            controller.TabBar.MakeCornerTabBar(UIColor.Black, backgroundColor, 0.9F, UIRectCorner.AllCorners, new CGSize(0, 0), 50);
        }
        catch (Exception ex)
        {
        }
    }
    void IShellTabBarAppearanceTracker.SetAppearance(UITabBarController controller, ShellAppearance appearance)
    {
        base.SetAppearance(controller, appearance);

        try
        {
            _shellAppearance = appearance as IShellAppearanceElement;
        }
        catch (Exception ex)
        {
        }
    }
}
internal static class UIViewExtensions
{
    public static UIView MakeCornerTabBar(this UIView uIView, UIColor shadowColor, UIColor fillColor, float opacity, UIRectCorner corners, CGSize offset, float radious)
    {
        var shadowLayer = new CAShapeLayer();
        var xRect = new CGRect(10, 0, uIView.Bounds.Right  - 20, uIView.Bounds.Height - 20);

        shadowLayer.Path = UIBezierPath.FromRoundedRect(xRect, corners, new CGSize(radious, radious)).CGPath;
        shadowLayer.FillColor = fillColor.CGColor;
        shadowLayer.Opacity = opacity;
       
        shadowLayer.ShadowColor = shadowColor.CGColor;
        shadowLayer.ShadowPath = shadowLayer.Path;
        shadowLayer.ShadowOffset = offset;
        shadowLayer.ShadowOpacity = opacity;
        shadowLayer.ShadowRadius = 5;

        shadowLayer.BackgroundColor = UIColor.Clear.CGColor;
        shadowLayer.BorderColor = UIColor.Clear.CGColor;
        shadowLayer.StrokeColor = UIColor.Clear.CGColor;
        shadowLayer.FillRule = CAShapeLayer.FillRuleEvenOdd;

        uIView.Layer.Mask = shadowLayer;

        return uIView;
    }
}