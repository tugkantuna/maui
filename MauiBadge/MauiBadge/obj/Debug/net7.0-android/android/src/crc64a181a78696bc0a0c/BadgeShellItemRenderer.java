package crc64a181a78696bc0a0c;


public class BadgeShellItemRenderer
	extends crc640ec207abc449b2ca.ShellItemRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("MauiBadge.Platforms.Android.BadgeShellItemRenderer, MauiBadge", BadgeShellItemRenderer.class, __md_methods);
	}


	public BadgeShellItemRenderer ()
	{
		super ();
		if (getClass () == BadgeShellItemRenderer.class) {
			mono.android.TypeManager.Activate ("MauiBadge.Platforms.Android.BadgeShellItemRenderer, MauiBadge", "", this, new java.lang.Object[] {  });
		}
	}


	public BadgeShellItemRenderer (int p0)
	{
		super (p0);
		if (getClass () == BadgeShellItemRenderer.class) {
			mono.android.TypeManager.Activate ("MauiBadge.Platforms.Android.BadgeShellItemRenderer, MauiBadge", "System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0 });
		}
	}


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
