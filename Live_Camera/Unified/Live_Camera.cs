using System;
using System.Drawing;
using Foundation;
using UIKit;
using AVFoundation;
using CoreMotion;
using CoreAnimation;
using CoreGraphics;

namespace Camera
{
	public partial class Live_Camera : UIViewController
	{
		UIImagePickerController imgPicker;
		bool FrontCamera;
		bool  haveImage;
		internal static bool initializeCamera, photoFromCam;
		AVCaptureSession session= null;
		AVCaptureVideoPreviewLayer captureVideoPreviewLayer=null;
		AVCaptureStillImageOutput stillImageOutput=null;
		public delegate void DidFinishPickingImage_delegate(UIImage image);
		public delegate void CameraControllerDidCancel_delegate();
		public delegate void CameraControllerdidSkipped_delegate();
		public delegate void LibarydidCalled_delegate();
		public delegate void LibaryWillDisappear_delegate();
		public DidFinishPickingImage_delegate didFinishPickingImage;
		public CameraControllerDidCancel_delegate CameraControllerDidCancel;
		public CameraControllerdidSkipped_delegate CameraControllerdidSkipped;
		public LibarydidCalled_delegate LibarydidCalled;
		public LibaryWillDisappear_delegate LibaryWillDisappear;
		internal CGRect Rect;

		public Live_Camera () : base ("Live_Camera", null)
		{

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			FrontCamera = false;
			captureImage.Hidden = true;
			imgPicker = new UIImagePickerController ();
			imgPicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imgPicker.ImagePickerControllerDelegate = new picker(this) ;
			imgPicker.AllowsEditing = true;
			initializeCamera = true;
			photoFromCam = true;  
			Rect = new CGRect (View.Frame.Location,View.Frame.Size);
			// Perform any additional setup after loading the view, typically from a nib.
		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			UIApplication.SharedApplication.SetStatusBarHidden (true, true);
		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			if (initializeCamera) {
				initializeCamera = false;
				finitializeCamera ();
			}
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}
		private void finitializeCamera()
		{
			session = new AVCaptureSession ();
			session.SessionPreset = AVCaptureSession.PresetPhoto;
			captureVideoPreviewLayer = new AVCaptureVideoPreviewLayer (session);
			captureVideoPreviewLayer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
			captureVideoPreviewLayer.Frame = imagePreview.Bounds;
			imagePreview.Layer.AddSublayer (captureVideoPreviewLayer);
			UIView view = imagePreview;
			CALayer viewlayer = view.Layer;
			viewlayer.MasksToBounds = true;
			CGRect bounds = view.Bounds;
			captureVideoPreviewLayer.Frame = bounds;
			AVCaptureDevice[] devices = AVCaptureDevice.Devices;
			AVCaptureDevice frontCamera=null;
			AVCaptureDevice backCamera = null;
			if (devices.Length==0) {
				Console.WriteLine ("No Camera Available");
				disableCameraDeviceControls ();
				Console.WriteLine (photoCaptureButton.Enabled);
				return;
			}
			foreach (var device in devices) {
				Console.WriteLine ("Device name: " + device.LocalizedName);
				if(device.HasMediaType(AVMediaType.Video))
				{
					if (device.Position == AVCaptureDevicePosition.Back) {
						Console.WriteLine ("Device position : back");
						backCamera = device;
					} else {
						Console.WriteLine("Device position : front");
						frontCamera = device;
					}
				}
			}
			if (!FrontCamera) {
				if (backCamera.HasFlash) {
					var error_temp = new NSError ();
					backCamera.LockForConfiguration (out error_temp);
					if (flashToggleButton.Selected) {
						backCamera.FlashMode = AVCaptureFlashMode.On;
					} else {
						backCamera.FlashMode = AVCaptureFlashMode.Off;
					}
					backCamera.UnlockForConfiguration ();
					flashToggleButton.Enabled = true;
				} else {
					if (backCamera.IsFlashModeSupported (AVCaptureFlashMode.Off)) {
						var error_temp = new NSError ();
						backCamera.LockForConfiguration (out error_temp);
						backCamera.FlashMode = AVCaptureFlashMode.Off;
						backCamera.UnlockForConfiguration ();
					}
					flashToggleButton.Enabled = true;
				}
				NSError error = null;
				AVCaptureDeviceInput input = new AVCaptureDeviceInput (backCamera,out  error);
				if (input == null) {
					Console.WriteLine ("ERROR: trying to open camera:" + error.ToString ());
				}
				session.AddInput (input);
			}
			if (FrontCamera) {
				flashToggleButton.Enabled = false;
				NSError error = null;
				AVCaptureDeviceInput input = new AVCaptureDeviceInput (frontCamera, out error);
				if (input == null) {
					Console.WriteLine ("ERROR: trying to open camera:" + error.ToString ());
				}
				session.AddInput (input);
			}
			stillImageOutput = new AVCaptureStillImageOutput ();
			NSDictionary outputSettings = NSDictionary.FromObjectAndKey(new
				NSString("AVVideoCodecKey"), new NSString("AVVideoCodecJPEG"));
			stillImageOutput.OutputSettings = outputSettings;
			session.AddOutput (stillImageOutput);
			session.StartRunning ();
		}
		private void capImage()
		{
			AVCaptureConnection videoConnection = null;
			foreach (var connection in stillImageOutput.Connections) {

				foreach (var port in connection.InputPorts) {

					if (port.MediaType == AVMediaType.Video) {
						videoConnection = connection;
						break;
					}
				}

				if (videoConnection!=null) {
					break;
				}
			}
			Console.WriteLine("about to request a capture from: " + stillImageOutput.ToString());
			stillImageOutput.CaptureStillImageAsynchronously (videoConnection, new AVCaptureCompletionHandler (
				(imageSampleBuffer, error) => {
					if (imageSampleBuffer != null) {
						NSData imageData = AVCaptureStillImageOutput.JpegStillToNSData(imageSampleBuffer);
						processImage(new UIImage(imageData));
					}
				})
			);
		}
		internal void processImage(UIImage image)
		{
			haveImage = true;
			photoFromCam = true;
			UIImage smallImage = imageWithImage (image, 640.0f);
			CGRect cropRect = new CGRect(0, 105, 640, 640);
			CGImage imageRef = smallImage.CGImage;
			imageRef = imageRef.WithImageInRect (cropRect);
			//CGImageRef imageRef = CGImageCreateWithImageInRect([smallImage CGImage], cropRect);
			this.captureImage.Image = new UIImage (imageRef);
			setCapturedImage ();
		}
		private void setCapturedImage()
		{
			session.StopRunning ();
			hideControllers ();
		}
		internal void hideControllers()
		{
			UIView.Animate (
				duration: 0.2,
				animation: () => {
					//photoBar.Center = new CGPoint (photoBar.Center.X, photoBar.Center.Y + 116);
					topBar.Center = new CGPoint (topBar.Center.X, topBar.Center.Y - 44);
				});
		}
		internal void showControllers()
		{
			UIView.Animate (
				duration: 0.2,
				animation: () => {
					//	photoBar.Center = new CGPoint (photoBar.Center.X, photoBar.Center.Y - 116);
					topBar.Center = new CGPoint (topBar.Center.X, topBar.Center.Y + 44);
				});
		}
		private UIImage imageWithImage(UIImage sourceImage, nfloat i_width)
		{
			nfloat oldWidth = sourceImage.Size.Width;
			nfloat scaleFactor = i_width / oldWidth;
			nfloat newHeight = sourceImage.Size.Height * scaleFactor;
			nfloat newWidth = oldWidth * scaleFactor;
			UIGraphics.BeginImageContext (new CGSize (newWidth, newHeight));
			sourceImage.Draw (new CGRect (0, 0, newWidth, newHeight));
			UIImage newImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return newImage;
		}
		private void disableCameraDeviceControls()
		{
			cameraToggleButton.Enabled = false;
			flashToggleButton.Enabled = false;
			photoCaptureButton.Enabled = false;
		}
		partial void snapImage (UIKit.UIButton sender)
		{
			photoCaptureButton.Enabled = false;
			if(!haveImage)
			{
				captureImage.Image=null;
				captureImage.Hidden= false;
				imagePreview.Hidden=true;
				capImage();
			}
			else
			{
				captureImage.Hidden= true;
				imagePreview.Hidden=false;
				haveImage=false;
			}
		}
		partial void cancel (UIKit.UIButton sender)
		{
			cancel_trigger();
			//DismissViewController(true,null);
		}
		private void cancel_trigger()
		{
			if (CameraControllerDidCancel != null) {
				CameraControllerDidCancel ();
			} else {
				CameraControllerDidCancel = new CameraControllerDidCancel_delegate(()=>{});
				CameraControllerDidCancel();
			}
		}
		partial void donePhotoCapture (UIKit.UIButton sender)
		{
			donePhotoCapture_trigger(captureImage.Image);
		}
		private void donePhotoCapture_trigger(UIImage image)
		{
			if (didFinishPickingImage != null) {
				didFinishPickingImage (image);
			} else {
				didFinishPickingImage = new DidFinishPickingImage_delegate((img)=>{});
				didFinishPickingImage (image);
			}
		}
		partial void gridToogle (UIKit.UIButton sender)
		{
			if (sender.Selected) {
				sender.Selected = false;
				UIView.Animate(0.2,()=>
					{
						ImgViewGrid.Alpha = 1.0f;
					}
				);
			}
			else{
				sender.Selected=true;
				UIView.Animate(0.2,()=>
					{
						ImgViewGrid.Alpha = 0.0f;
					}
				);
			}

		}
		partial void retakePhoto (UIKit.UIButton sender)
		{
			photoCaptureButton.Enabled = true;
			captureImage.Image=null;
			imagePreview.Hidden=false;
			showControllers();
			haveImage=false;
			FrontCamera=false;
			session.StartRunning();
		}
		partial void skipped (UIKit.UIButton sender)
		{
			skipped_trigger();
		}
		private void skipped_trigger()
		{
			if (CameraControllerdidSkipped != null) {
				CameraControllerdidSkipped ();
			} else {
				CameraControllerdidSkipped = new CameraControllerdidSkipped_delegate(()=>{});
				CameraControllerdidSkipped();
			}
		}
		partial void switchCamera (UIKit.UIButton sender)
		{
			session.StopRunning();
			if (sender.Selected) {  // Switch to Back camera
				sender.Selected = false;
				FrontCamera = false;
				finitializeCamera();
			}
			else {                  // Switch to Front camera
				sender.Selected = true;
				FrontCamera = true;
				finitializeCamera();
			}
		}
		private void switchToLibrary_trigger()
		{
			if (LibarydidCalled != null) {
				LibarydidCalled ();
			} else {
				LibarydidCalled = new LibarydidCalled_delegate(()=>{});
				LibarydidCalled();
			}
		}
		partial void switchToLibrary (UIKit.UIButton sender)
		{
			if(session!=null)
			{
				session.StopRunning();
			}
			switchToLibrary_trigger();
			PresentViewController(imgPicker,true,null);
		}
		partial void toogleFlash (UIKit.UIButton sender)
		{
			NSError error= new NSError();
			if(!FrontCamera)
			{
				if(sender.Selected)
				{
					sender.Selected= false;
					var devices=AVCaptureDevice.Devices;
					foreach(var device in devices)
					{
						Console.WriteLine("Device name: "+device.LocalizedName);
						if(device.HasMediaType(AVMediaType.Video))
						{
							if (device.Position == AVCaptureDevicePosition.Back) {
								Console.WriteLine("Device position : back");
								if (device.HasFlash){
									device.LockForConfiguration(out error);
									device.FlashMode= AVCaptureFlashMode.Off;
									device.UnlockForConfiguration();
									break;
								}
							}
						}
					}
				}
				else
				{
					sender.Selected=true;
					var devices=AVCaptureDevice.Devices;
					foreach(var device in devices)
					{
						Console.WriteLine("Device name: "+device.LocalizedName);
						if(device.HasMediaType(AVMediaType.Video))
						{
							if (device.Position == AVCaptureDevicePosition.Back) {
								Console.WriteLine("Device position : back");
								if (device.HasFlash){
									device.LockForConfiguration(out error);
									device.FlashMode= AVCaptureFlashMode.On;
									device.UnlockForConfiguration();
									break;
								}
							}
						}
					}
				}
			}
		}
	}
	internal class picker:UIImagePickerControllerDelegate
	{
		public Live_Camera Controller; 
		public override void FinishedPickingMedia (UIImagePickerController picker, NSDictionary info)
		{
			if (info != null) {
				Live_Camera.photoFromCam = false;
				UIImage outputImage = (info.ObjectForKey (UIImagePickerController.EditedImage))as UIImage;
				if (outputImage == null) {
					outputImage =  (info.ObjectForKey (UIImagePickerController.OriginalImage)) as UIImage; 
				}
				if (outputImage != null) {
					Controller.captureImage.Hidden = false;
					//	Controller.processImage (outputImage);
					Controller.captureImage.Image = outputImage;
					picker.DismissViewController (true, null);
					Controller.hideControllers ();
					Controller.photoCaptureButton.Enabled = false;
					Controller.View.Frame = Controller.Rect;
					if (Controller.LibaryWillDisappear != null) {
						Controller.LibaryWillDisappear ();
					} else {
						Controller.LibaryWillDisappear = new Live_Camera.LibaryWillDisappear_delegate(()=>{});
						Controller.LibaryWillDisappear();
					}
				}
			}
		}
		public override void Canceled (UIImagePickerController picker)
		{
			Live_Camera.initializeCamera = true;
			picker.DismissViewController (true,null);
			Controller.View.Frame = Controller.Rect;
			if (Controller.LibaryWillDisappear != null) {
				Controller.LibaryWillDisappear ();
			} else {
				Controller.LibaryWillDisappear = new Live_Camera.LibaryWillDisappear_delegate(()=>{});
				Controller.LibaryWillDisappear();
			}
		}

		public picker(Live_Camera controller)
		{
			this.Controller = controller;
		}
	}
}

