using CoreGraphics;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Platform;
using System.ComponentModel;
using UIKit;

namespace MauiBadge.Platforms.iOS;

public class BadgeShellItemRenderer : ShellItemRenderer
{
    private readonly string[] _applyPropertyNames =
        new string[]
        {
            Badge.VisibleProperty.PropertyName,
            Badge.TextProperty.PropertyName,
            Badge.TextColorProperty.PropertyName,
            Badge.BackgroundColorProperty.PropertyName,
            Badge.AnimationProperty.PropertyName,
            Badge.TitleTextProperty.PropertyName,
            Badge.TitleTextColorProperty.PropertyName,
            Badge.TitleBackgroundColorProperty.PropertyName,
            Badge.TitleVisibleProperty.PropertyName
        };

    private readonly Dictionary<Guid, int> _tabRealIndexByItemId =
        new Dictionary<Guid, int>();
    public BadgeShellItemRenderer(IShellContext context) : base(context)
    {
    }

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);
        InitBadges();
    }
    public override void ViewLayoutMarginsDidChange()
    {
        base.ViewLayoutMarginsDidChange();
        try
        {
            SetBadges();
        }
        catch (Exception ex) { }
    }

    private void InitBadges()
    {
        _tabRealIndexByItemId.Clear();
    }

    private void SetBadges()
    {
        if (ShellItem?.Items == null)
            return;

        for (int index = 0, filteredIndex = 0; index < ShellItem.Items.Count; index++)
        {
            var item = ShellItem.Items.ElementAtOrDefault(index);
            if (item == null || !item.IsVisible)
                continue;

            _tabRealIndexByItemId[item.Id] = filteredIndex;

            UpdateBadge(item, filteredIndex);

            filteredIndex++;
        }
    }

    private void UpdateBadge(ShellSection pShellSection, int pIndex)
    {
        if (pIndex < 0)
            return;

        var text = Badge.GetText(pShellSection);
        var textColor = Badge.GetTextColor(pShellSection);
        var bg = Badge.GetBackgroundColor(pShellSection);
        var anim = Badge.GetAnimation(pShellSection);

        if (Badge.GetVisible(pShellSection))
            ApplyBadge(pIndex, text, bg, textColor, anim, pShellSection);

    }

    private void UpdateBadgeTitle(ShellSection pShellSection, int pIndex)
    {
        if (pIndex < 0)
            return;

        string xText = Badge.GetTitleText(pShellSection);
        Color xTextColor = Badge.GetTitleTextColor(pShellSection);
        Color xBackColor = Badge.GetTitleBackgroundColor(pShellSection);

        if (Badge.GetTitleVisible(pShellSection))
            ApplyBadgeTitle(pIndex, xText, xBackColor, xTextColor, pShellSection.IsChecked);
    }

    private void ApplyBadge(int pIndex, string pText, Color pBackColor, Color pTextColor, bool pAnimation, ShellSection pShellSection)
    {
        bool xAdd = true;
        bool xFind = false;
        int xTabIndex = -1;
        UILabel lblBadge = null;

        foreach (UIView xView in TabBar.Subviews)
        {
            var c = new ObjCRuntime.Class("UITabBarButton");
            var s = new ObjCRuntime.Selector("frame");

            if (xView.IsKindOfClass(c) && xView.RespondsToSelector(s))
            {
                xAdd = true;
                xFind = false;
                xTabIndex += 1;

                if (xTabIndex != pIndex)
                    continue;

                foreach (var xV in xView.Subviews)
                {
                    if (xV is UIImageView img)
                    {

                        foreach (var xBadgeLabel in img.Subviews)
                        {
                            if (xBadgeLabel is UILabel lbl)
                            {
                                if (lbl.Tag == 500 + pIndex)
                                {
                                    xFind = true;
                                    xAdd = false;
                                    lblBadge = lbl;
                                    break;
                                }
                            }
                        }

                        if (lblBadge == null)
                            lblBadge = new UILabel(new CGRect(10, -4, 20, 20));

                        lblBadge.Tag = 500 + pIndex;
                        lblBadge.TextColor = pTextColor.ToUIColor();
                        lblBadge.TextAlignment = UITextAlignment.Center;
                        lblBadge.Layer.BackgroundColor = pBackColor.ToCGColor();
                        lblBadge.Layer.CornerRadius = 9;
                        lblBadge.Layer.MasksToBounds = true;
                        lblBadge.Font = UIFont.BoldSystemFontOfSize(8.0f);

                        if (xAdd)
                            img.AddSubview(lblBadge);

                        if (pAnimation)
                        {
                            CGAffineTransform startTransform = lblBadge.Transform;
                            CGAffineTransform transform1 = CGAffineTransform.MakeScale(0.8f, 0.8f);
                            lblBadge.Transform = transform1;

                            UIView.Animate(0.5, animation: new Action(() =>
                            {
                                lblBadge.Transform = startTransform;

                                UIView.Animate(0.1, animation: new Action(() =>
                                {
                                    lblBadge.Alpha = 0.5f;
                                }));

                            }), completion: new Action(() =>
                            {
                                try
                                {
                                    lblBadge.Alpha = 1;
                                    lblBadge.Text = pText;
                                    lblBadge = null;

                                    UpdateBadgeTitle(pShellSection, pIndex);
                                }
                                catch (Exception ex)
                                {
                                }
                            }));
                        }
                        else
                        {
                            lblBadge.Text = pText;
                            UpdateBadgeTitle(pShellSection, pIndex);
                        }
                    }
                }
            }

        }

    }
    private void ApplyBadgeTitle(int pIndex, string pText, Color pBackColor, Color pTextColor, bool pChecked)
    {
        // Add SubView
        bool xAdd = true;

        //Tab Index
        int xTabIndex = -1;

        //Fake Title
        UILabel lblTitle = null;

        foreach (UIView xTabView in TabBar.Subviews)
        {
            var c = new ObjCRuntime.Class("UITabBarButton");
            var s = new ObjCRuntime.Selector("frame");

            //UITabBarButton And frame
            if (xTabView.IsKindOfClass(c) && xTabView.RespondsToSelector(s))
            {
                //Index Control
                xTabIndex += 1;
                if (xTabIndex != pIndex)
                    continue;

                //Subview Loop
                foreach (var xSubView1 in xTabView.Subviews)
                {

                    if (xSubView1 is UILabel xOrginalTitle)
                    {
                        //Badge Label
                        if (xOrginalTitle.Tag == 500 + pIndex)
                            continue;

                        //Orginal Title
                        if (xOrginalTitle.Tag != 1000 + pIndex)
                        {

                            //Orginal InVisible
                            xOrginalTitle.Hidden = true;
                            xOrginalTitle.Text = pText;

                            foreach (var xSubView2 in xTabView.Subviews)
                            {
                                if (xSubView2 is UILabel xFakeTitleOrBadge)
                                {
                                    //Fake Badge
                                    if (xFakeTitleOrBadge.Tag == 500 + pIndex)
                                        continue;

                                    //Fake Title
                                    if (xFakeTitleOrBadge.Tag == 1000 + pIndex)
                                    {
                                        xAdd = false;
                                        lblTitle = xFakeTitleOrBadge;
                                        break;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (lblTitle == null)
                                lblTitle = new UILabel();

                            lblTitle.Tag = 1000 + pIndex;
                            lblTitle.Font = xOrginalTitle.Font;
                            lblTitle.Text = pText;
                                                        
                            if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
                            {
                                lblTitle.Frame = new CGRect(10 + xOrginalTitle.Frame.Left, xOrginalTitle.Frame.Top, xTabView.Frame.Width - 20 - xOrginalTitle.Frame.Left, xOrginalTitle.Frame.Height);
                            }
                            else
                            {
                                lblTitle.Frame = new CGRect(10, xOrginalTitle.Frame.Top, xTabView.Frame.Width - 20, xOrginalTitle.Frame.Height);
                            }

                            //lblTitle.Frame = xOrginalTitle.Frame;
                            lblTitle.TextColor = pTextColor.ToUIColor();
                            lblTitle.Layer.BackgroundColor = pBackColor.ToCGColor();
                            lblTitle.Layer.CornerRadius = xOrginalTitle.Frame.Height / 2;
                            
                            lblTitle.TextAlignment = UITextAlignment.Center;

                            if (xAdd)
                                xTabView.AddSubview(lblTitle);

                        }

                    }
                }
            }
        }
    }

    protected override void OnShellSectionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnShellSectionPropertyChanged(sender, e);

        var xShellSection = (ShellSection)sender;

        if (xShellSection.IsVisible)
        {
            bool isChecked = false;
            var index = _tabRealIndexByItemId.GetValueOrDefault(xShellSection.Id, -1);

            if (_applyPropertyNames.All(x => x != e.PropertyName))
            {
                if (e.PropertyName != "IsChecked")
                    return;
                else
                    isChecked = true;
            }

            if (isChecked)
            {
                App.Current.MainPage.Dispatcher.Dispatch(() =>
                {
                    UpdateBadgeTitle(xShellSection, index);
                });
            }
            else
                UpdateBadge(xShellSection, index);
        }

    }

}
