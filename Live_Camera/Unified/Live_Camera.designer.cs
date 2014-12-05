// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Live_Camera
{
	[Register ("Live_Camera")]
	partial class Live_Camera
	{
		[Outlet]
		UIKit.UIButton cameraToggleButton { get; set; }

		[Outlet]
		UIKit.UIButton cancelButton { get; set; }

		[Outlet]
		internal UIKit.UIImageView captureImage { get; set; }

		[Outlet]
		UIKit.UIButton flashToggleButton { get; set; }

		[Outlet]
		UIKit.UIView imagePreview { get; set; }

		[Outlet]
		UIKit.UIImageView ImgViewGrid { get; set; }

		[Outlet]
		UIKit.UIButton libraryToggleButton { get; set; }

		[Outlet]
		internal UIKit.UIView photoBar { get; set; }

		[Outlet]
		internal UIKit.UIButton photoCaptureButton { get; set; }

		[Outlet]
		UIKit.UIView topBar { get; set; }

		[Action ("cancel:")]
		partial void cancel (UIKit.UIButton sender);

		[Action ("donePhotoCapture:")]
		partial void donePhotoCapture (UIKit.UIButton sender);

		[Action ("gridToogle:")]
		partial void gridToogle (UIKit.UIButton sender);

		[Action ("retakePhoto:")]
		partial void retakePhoto (UIKit.UIButton sender);

		[Action ("skipped:")]
		partial void skipped (UIKit.UIButton sender);

		[Action ("snapImage:")]
		partial void snapImage (UIKit.UIButton sender);

		[Action ("switchCamera:")]
		partial void switchCamera (UIKit.UIButton sender);

		[Action ("switchToLibrary:")]
		partial void switchToLibrary (UIKit.UIButton sender);

		[Action ("toogleFlash:")]
		partial void toogleFlash (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (cameraToggleButton != null) {
				cameraToggleButton.Dispose ();
				cameraToggleButton = null;
			}

			if (cancelButton != null) {
				cancelButton.Dispose ();
				cancelButton = null;
			}

			if (captureImage != null) {
				captureImage.Dispose ();
				captureImage = null;
			}

			if (flashToggleButton != null) {
				flashToggleButton.Dispose ();
				flashToggleButton = null;
			}

			if (imagePreview != null) {
				imagePreview.Dispose ();
				imagePreview = null;
			}

			if (ImgViewGrid != null) {
				ImgViewGrid.Dispose ();
				ImgViewGrid = null;
			}

			if (libraryToggleButton != null) {
				libraryToggleButton.Dispose ();
				libraryToggleButton = null;
			}

			if (photoBar != null) {
				photoBar.Dispose ();
				photoBar = null;
			}

			if (photoCaptureButton != null) {
				photoCaptureButton.Dispose ();
				photoCaptureButton = null;
			}

			if (topBar != null) {
				topBar.Dispose ();
				topBar = null;
			}
		}
	}
}
