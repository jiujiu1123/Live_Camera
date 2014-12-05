using System;
using Foundation;
using UIKit;

namespace Live_Camera_Demo
{
	public partial class Live_Camera_DemoViewController : UIViewController
	{
		public Live_Camera_DemoViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{	
			base.ViewDidAppear (animated);

			Btn_Show.TouchUpInside+= (object sender, EventArgs e) => 
			{
				Add_Cam();
			};
		}

		private void Add_Cam()
		{
			var live = new Live_Camera.Live_Camera();
			live.View.Frame= new CoreGraphics.CGRect(new CoreGraphics.CGPoint(live.View.Frame.X+640,live.View.Frame.Y),live.View.Frame.Size);
			UIView.Animate (1, () => {
				live.View.Frame= new CoreGraphics.CGRect(new CoreGraphics.CGPoint(live.View.Frame.X-640,live.View.Frame.Y),live.View.Frame.Size);
				Add (live.View);
			});
			live.CameraControllerDidCancel += () => {
				UIView.Animate(0.5,()=>{
					live.View.Frame = new CoreGraphics.CGRect(new CoreGraphics.CGPoint(live.View.Frame.X,live.View.Frame.Y+640),live.View.Frame.Size);
				},()=>
					{
						live.View.RemoveFromSuperview();
					});
				//
			};
			live.didFinishPickingImage += (image) => {
				UIView.Animate(0.5,()=>{
					live.View.Frame = new CoreGraphics.CGRect(new CoreGraphics.CGPoint(live.View.Frame.X,live.View.Frame.Y+640),live.View.Frame.Size);
				},()=>
					{
						live.View.RemoveFromSuperview();
					}); 
				ImageView.Image=image;
			};
			live.CameraControllerdidSkipped+= () => {
				UIView.Animate(0.5,()=>{
					live.View.Frame = new CoreGraphics.CGRect(new CoreGraphics.CGPoint(live.View.Frame.X,live.View.Frame.Y+640),live.View.Frame.Size);
				},()=>
					{
						live.View.RemoveFromSuperview();
					});
				//
			};
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

