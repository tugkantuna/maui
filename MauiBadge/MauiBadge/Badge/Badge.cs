namespace MauiBadge;

public class Badge
{
    #region "Badge -- BindableProperty"
    public static readonly BindableProperty AnimationProperty =
        BindableProperty.CreateAttached("Animation", typeof(bool), typeof(Badge),
            true);

    public static readonly BindableProperty VisibleProperty =
        BindableProperty.CreateAttached("Visible", typeof(bool), typeof(Badge),
            false);

    public static readonly BindableProperty TextProperty =
        BindableProperty.CreateAttached("Text", typeof(string), typeof(Badge),
            string.Empty);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.CreateAttached("TextColor", typeof(Color), typeof(Badge),
            Colors.White);

    public static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.CreateAttached("BackgroundColor", typeof(Color), typeof(Badge),
            Colors.Red);

    #endregion

    #region "Badge -- Get/Set"
    public static bool GetAnimation(BindableObject target) =>
        (bool)target.GetValue(AnimationProperty);
    public static void SetAnimation(BindableObject view, bool value) =>
        view.SetValue(AnimationProperty, value);
    public static bool GetVisible(BindableObject target) =>
        (bool)target.GetValue(VisibleProperty);
    public static void SetVisible(BindableObject view, bool value) =>
        view.SetValue(VisibleProperty, value);
    public static string GetText(BindableObject target) =>
        (string)target.GetValue(TextProperty);

    public static void SetText(BindableObject view, string value) =>
        view.SetValue(TextProperty, value);

    public static Color GetTextColor(BindableObject target) =>
        (Color)target.GetValue(TextColorProperty);

    public static void SetTextColor(BindableObject view, Color value) =>
        view.SetValue(TextColorProperty, value);

    public static Color GetBackgroundColor(BindableObject target) =>
       (Color)target.GetValue(BackgroundColorProperty);
    public static void SetBackgroundColor(BindableObject view, Color value) =>
        view.SetValue(BackgroundColorProperty, value);

    #endregion

    #region "Title -- BindableProperty"
    public static readonly BindableProperty TitleVisibleProperty =
            BindableProperty.CreateAttached("TitleVisible", typeof(bool), typeof(Badge),
                false);

    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.CreateAttached("TitleText", typeof(string), typeof(Badge),
            string.Empty);

    public static readonly BindableProperty TitleTextColorProperty =
        BindableProperty.CreateAttached("TitleTextColor", typeof(Color), typeof(Badge),
            Colors.White);

    public static readonly BindableProperty TitleBackgroundColorProperty =
        BindableProperty.CreateAttached("TitleBackgroundColor", typeof(Color), typeof(Badge),
            Colors.Transparent);
    #endregion

    #region "Title -- Get/Set"
    public static bool GetTitleVisible(BindableObject target) =>
       (bool)target.GetValue(TitleVisibleProperty);
    public static void SetTitleVisible(BindableObject view, bool value) =>
        view.SetValue(TitleVisibleProperty, value);
    public static string GetTitleText(BindableObject target) =>
        (string)target.GetValue(TitleTextProperty);

    public static void SetTitleText(BindableObject view, string value) =>
        view.SetValue(TitleTextProperty, value);

    public static Color GetTitleTextColor(BindableObject target) =>
        (Color)target.GetValue(TitleTextColorProperty);

    public static void SetTitleTextColor(BindableObject view, Color value) =>
        view.SetValue(TitleTextColorProperty, value);

    public static Color GetTitleBackgroundColor(BindableObject target) =>
        (Color)target.GetValue(TitleBackgroundColorProperty);

    public static void SetTitleBackgroundColor(BindableObject view, Color value) =>
        view.SetValue(TitleBackgroundColorProperty, value);
    #endregion
}