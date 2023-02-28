using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Internal;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using System.ComponentModel;
using TextAlignment = Android.Views.TextAlignment;

namespace MauiBadge.Platforms.Android;
public class BadgeShellItemRenderer : ShellItemRenderer
{
    private IShellContext _context { get; set; }
    bool _init;

    BottomNavigationView _bottomNavigationView;

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

    public BadgeShellItemRenderer(IShellContext shellContext) : base(shellContext)
    {
        _context = shellContext;
    }
    public override global::Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        LinearLayout ly = (LinearLayout)view;
        _bottomNavigationView = (BottomNavigationView)ly.GetChildAt(1);

        return view;
    }

    public void Init()
    {
        if (_init)
            return;

        _tabRealIndexByItemId.Clear();
        
        SetBadges();
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

        _init = true;
    }

    private void UpdateBadge(ShellSection pShellSection, int pIndex)
    {
        if (pIndex < 0)
            return;

        string xText = Badge.GetText(pShellSection);
        Color xTextColor = Badge.GetTextColor(pShellSection);
        Color xBackColor = Badge.GetBackgroundColor(pShellSection);
        bool xAnimation = Badge.GetAnimation(pShellSection);

        if (Badge.GetVisible(pShellSection))
            ApplyBadge(pIndex, xText, xBackColor, xTextColor, xAnimation, pShellSection);
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
        TextView lblBadge = null;

        BottomNavigationView.LayoutParams lp = new BottomNavigationView.LayoutParams(_bottomNavigationView.LayoutParameters);

        DisplayMetrics xMetrics = global::Android.Content.Res.Resources.System.DisplayMetrics;
        
        //Badge Width/Height/Margin
        var xWH = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 18, xMetrics);
        lp.Width = xWH;
        lp.Height = xWH;
        lp.SetMargins(lp.Width, 0, 0, lp.Height / 2);

        //Menu
        var xMenu = _bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;
        if (xMenu.ChildCount == 0)
            return;

        //ItemView
        var xItemView = xMenu.GetChildAt(pIndex) as BottomNavigationItemView;

        var imageLayout = xItemView.GetChildAt(0);
        var ly = ((FrameLayout)imageLayout);

        for (int i = 0; i < ly.ChildCount; i++)
        {
            if (ly.GetChildAt(i) is TextView lbl)
            {
                if (lbl.Id == 500 + pIndex)
                {
                    xAdd = false;
                    lblBadge = lbl;
                    break;
                }
            }
        }

        if (lblBadge == null)
        {
            lblBadge = new TextView(_bottomNavigationView.Context);
            lblBadge.Id = 500 + pIndex;
        }

        lblBadge.SetTextColor(pTextColor.ToAndroid());
        lblBadge.SetTextSize(ComplexUnitType.Pt, 5);
        lblBadge.SetBackground(CornerLbl(pBackColor, (float)(lp.Width / 2)));
        lblBadge.TextAlignment = TextAlignment.Gravity;
        lblBadge.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
        lblBadge.LayoutParameters = lp;

        if (xAdd)
            ly.AddView(lblBadge);

        if (pAnimation)
        {
            AnimationSet animationSet = new AnimationSet(true);

            //RotateAnimation rotateAnimation = new RotateAnimation(-360,0);
            //rotateAnimation.Duration = 1000;
            //animationSet.AddAnimation(rotateAnimation);

            AlphaAnimation alphaAnimation = new AlphaAnimation(1f, 0.5f);
            alphaAnimation.Duration = 300;
            animationSet.AddAnimation(alphaAnimation);

            ScaleAnimation scaleAnimation = new ScaleAnimation(0.8f, 1f, 0.8f ,1f);
            scaleAnimation.Duration = 300;
            animationSet.AddAnimation(scaleAnimation);

            alphaAnimation = new AlphaAnimation(0.5f, 1f);
            alphaAnimation.Duration = 300;
            animationSet.AddAnimation(alphaAnimation);

            lblBadge.StartAnimation(animationSet);

            App.Current.MainPage.Dispatcher.DispatchAsync(async() =>
            {
                await Task.Delay(100);
                lblBadge.Text = pText;
                await Task.Delay(100);
                UpdateBadgeTitle(pShellSection, pIndex);
            });
        }
        
    }
    private void ApplyBadgeTitle(int pIndex, string pText, Color pBackColor, Color pTextColor,  bool pChecked)
    {
        bool xAddSmall = true;
        bool xAddLarge = true;

        TextView lblTitleSmall = null;
        TextView lblTitleLarge = null;

        BottomNavigationView.LayoutParams lpSmall = new BottomNavigationView.LayoutParams(_bottomNavigationView.LayoutParameters);
        BottomNavigationView.LayoutParams lpLarge = new BottomNavigationView.LayoutParams(_bottomNavigationView.LayoutParameters);

        var xMenu = _bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;
        if (xMenu.ChildCount == 0)
            return;

        var xItemView = xMenu.GetChildAt(pIndex) as BottomNavigationItemView;

        var titleLayout = xItemView.GetChildAt(1);

        //Default Titles Visibility
        TextView smallTextView = (TextView)((BaselineLayout)titleLayout).GetChildAt(0);
        TextView largeTextView = (TextView)((BaselineLayout)titleLayout).GetChildAt(1);
        smallTextView.Visibility = ViewStates.Invisible;
        largeTextView.Visibility = ViewStates.Invisible;

        //
        var ly = ((BaselineLayout)titleLayout);

        //Small Title
        for (int i = 0; i < ly.ChildCount; i++)
        {
            if (ly.GetChildAt(i) is TextView lblSmall)
            {
                if (lblSmall.Id == 1000 + pIndex)
                {
                    xAddSmall = false;
                    lblTitleSmall = lblSmall;
                    break;
                }
            }
        }

        if (lblTitleSmall == null)
        {
            lblTitleSmall = new TextView(_bottomNavigationView.Context);
            lblTitleSmall.Id = 1000 + pIndex;
        }

        lblTitleSmall.Visibility = pChecked ? ViewStates.Invisible : ViewStates.Visible;
        lblTitleSmall.SetTextSize(ComplexUnitType.Px, smallTextView.TextSize);
        lblTitleSmall.Text = pText;
        lblTitleSmall.SetTextColor(pTextColor.ToAndroid());
        lblTitleSmall.TextAlignment = TextAlignment.Gravity;
        lblTitleSmall.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;

        lblTitleSmall.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);

        var xSmallW = lblTitleSmall.MeasuredWidth;
        var xSmallH = lblTitleSmall.MeasuredHeight;

        lpSmall.Height = xSmallH;
        lpSmall.Width = xSmallW + xSmallH;
        lpSmall.SetMargins(xSmallH / 2, 0, xSmallH / 2, 0);
        lblTitleSmall.SetBackground(CornerLbl(pBackColor, xSmallH / 2));
        lblTitleSmall.LayoutParameters = lpSmall;

        //Large Title
        for (int i = 0; i < ly.ChildCount; i++)
        {
            if (ly.GetChildAt(i) is TextView lblLarge)
            {
                if (lblLarge.Id == 2000 + pIndex)
                {
                    xAddLarge = false;
                    lblTitleLarge = lblLarge;
                    break;
                }
            }
        }

        if (lblTitleLarge == null)
        {
            lblTitleLarge = new TextView(_bottomNavigationView.Context);
            lblTitleLarge.Id = 2000 + pIndex;
        }

        lblTitleLarge.Visibility = pChecked ? ViewStates.Visible : ViewStates.Invisible;
        lblTitleLarge.SetTextSize(ComplexUnitType.Px, largeTextView.TextSize);
        lblTitleLarge.Text = pText;
        lblTitleLarge.SetTextColor(pTextColor.ToAndroid());
        lblTitleLarge.TextAlignment = TextAlignment.Gravity;
        lblTitleLarge.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;

        lblTitleLarge.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);

        var xLargeW = lblTitleLarge.MeasuredWidth;
        var xLargeH = lblTitleLarge.MeasuredHeight;

        lpLarge.Height = xLargeH;
        lpLarge.Width = xLargeW + xLargeH;
        lpLarge.SetMargins(xLargeH / 2, 0, xLargeH / 2, 0);
        lblTitleLarge.SetBackground(CornerLbl(pBackColor, xLargeH / 2));
        lblTitleLarge.LayoutParameters = lpLarge;

        if (xAddSmall)
            ly.AddView(lblTitleSmall);

        if (xAddLarge)
            ly.AddView(lblTitleLarge);
    }

    protected override void OnShellSectionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnShellSectionPropertyChanged(sender, e);

        bool isChecked = false;

        if (_applyPropertyNames.All(x => x != e.PropertyName))
        {
            if (e.PropertyName != "IsChecked")
                return;
            else 
                isChecked = true;
        }

        var xShellSection = (ShellSection)sender;
        if (xShellSection.IsVisible)
        {
            var index = _tabRealIndexByItemId.GetValueOrDefault(xShellSection.Id, -1);

            if (isChecked)
                UpdateBadgeTitle(xShellSection, index);
            else
                UpdateBadge(xShellSection, index);
                
        }
    }
    private GradientDrawable CornerLbl(Microsoft.Maui.Graphics.Color pColor, float pSize)
    {
        var backgroundGradient = new GradientDrawable(GradientDrawable.Orientation.LeftRight, new int[] { pColor.ToAndroid(), pColor.ToAndroid() });
        backgroundGradient.SetCornerRadius(pSize);
        return backgroundGradient;
    }

}
