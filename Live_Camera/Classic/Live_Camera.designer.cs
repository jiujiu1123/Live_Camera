// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;
using MonoTouch.UIKit;

namespace Live_Camera
{
	[Register ("Live_Camera")]
	partial class Live_Camera
	{
		[Outlet]
		UIButton cameraToggleButton { get; set; }

		[Outlet]
		UIButton cancelButton { get; set; }

		[Outlet]
		internal UIImageView captureImage { get; set; }

		[Outlet]
		UIButton flashToggleButton { get; set; }

		[Outlet]
		UIView imagePreview { get; set; }

		[Outlet]
		UIImageView ImgViewGrid { get; set; }

		[Outlet]
		UIButton libraryToggleButton { get; set; }

		[Outlet]
		internal UIView photoBar { get; set; }

		[Outlet]
		internal UIButton photoCaptureButton { get; set; }

		[Outlet]
		UIView topBar { get; set; }

		[Action ("cancel:")]
		partial void cancel (UIButton sender);

		[Action ("donePhotoCapture:")]
		partial void donePhotoCapture (UIButton sender);

		[Action ("gridToogle:")]
		partial void gridToogle (UIButton sender);

		[Action ("retakePhoto:")]
		partial void retakePhoto (UIButton sender);

		[Action ("skipped:")]
		partial void skipped (UIButton sender);

		[Action ("snapImage:")]
		partial void snapImage (UIButton sender);

		[Action ("switchCamera:")]
		partial void switchCamera (UIButton sender);

		[Action ("switchToLibrary:")]
		partial void switchToLibrary (UIButton sender);

		[Action ("toogleFlash:")]
		partial void toogleFlash (UIButton sender);
		
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
