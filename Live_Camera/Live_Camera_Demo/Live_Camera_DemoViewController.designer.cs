// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;

namespace Live_Camera_Demo
{
	[Register ("Live_Camera_DemoViewController")]
	partial class Live_Camera_DemoViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Btn_Show { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView ImageView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Btn_Show != null) {
				Btn_Show.Dispose ();
				Btn_Show = null;
			}
			if (ImageView != null) {
				ImageView.Dispose ();
				ImageView = null;
			}
		}
	}
}
