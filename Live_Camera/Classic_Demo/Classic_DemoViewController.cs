using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Classic_Demo
{
	public partial class Classic_DemoViewController : UIViewController
	{
		public Classic_DemoViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidAppear (bool animated)
		{	
			base.ViewDidAppear (animated);
			Add_Cam();
		}

		private void Add_Cam()
		{
			var live = new Live_Camera.Live_Camera();
			live.View.Frame= new RectangleF(new PointF(live.View.Frame.X+640,live.View.Frame.Y),live.View.Frame.Size);
			UIView.Animate (1, () => {
				live.View.Frame= new RectangleF(new PointF(live.View.Frame.X-640,live.View.Frame.Y),live.View.Frame.Size);
				Add (live.View);
			});
			live.CameraControllerDidCancel += () => {
				UIView.Animate(0.5,()=>{
					live.View.Frame = new RectangleF(new PointF(live.View.Frame.X,live.View.Frame.Y+640),live.View.Frame.Size);
				},()=>
					{
						live.View.RemoveFromSuperview();
					});
				//
			};
			live.didFinishPickingImage += (image) => {
				UIView.Animate(0.5,()=>{
					live.View.Frame = new RectangleF(new PointF(live.View.Frame.X,live.View.Frame.Y+640),live.View.Frame.Size);
				},()=>
					{
						live.View.RemoveFromSuperview();
					}); 
				ImageView.Image=image;
			};
			live.CameraControllerdidSkipped+= () => {
				UIView.Animate(0.5,()=>{
					live.View.Frame = new RectangleF(new PointF(live.View.Frame.X,live.View.Frame.Y+640),live.View.Frame.Size);
				},()=>
					{
						live.View.RemoveFromSuperview();
					});
				//
			};
		}

		#endregion
	}
}

