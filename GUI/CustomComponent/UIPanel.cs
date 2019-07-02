using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI.GUI.CustomComponent
{
	//TODO : Replace with the Advanced UI ui system
	internal class UIPanel : UIElement
	{

		private Vector2 offset;
		public bool dragging;

		private readonly int CORNER_SIZE = 10;
		private readonly int BAR_SIZE = 4;
		private readonly Texture2D _backgroundTexture;

		public bool isVisible = true;

		public List<UIElement> Children
		{
			get => this.Elements;
		}

		public UIPanel()
		{

			_backgroundTexture = ModContent.GetTexture("Terraria/UI/PanelBackground");

			base.SetPadding((float)CORNER_SIZE);
		}

		public UIPanel(Texture2D texture)
		{
			if (_backgroundTexture == null)
			{
				_backgroundTexture = texture;
			}
			base.SetPadding((float)CORNER_SIZE);
		}

		public UIPanel(Texture2D texture, int cornerSize, int barSize)
		{
			if (_backgroundTexture == null)
			{
				_backgroundTexture = texture;
			}
			base.SetPadding((float)CORNER_SIZE);
		}


		public override void MouseDown(UIMouseEvent evt)
		{
			base.MouseDown(evt);
			DragStart(evt);
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			base.MouseUp(evt);
			DragEnd(evt);
		}

		private void DragStart(UIMouseEvent evt)
		{
			offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
			dragging = true;
		}

		private void DragEnd(UIMouseEvent evt)
		{
			Vector2 end = evt.MousePosition;
			dragging = false;

			Left.Set(end.X - offset.X, 0f);
			Top.Set(end.Y - offset.Y, 0f);

			Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime); // don't remove.

			// Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			if (dragging)
			{
				Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same.
				Top.Set(Main.mouseY - offset.Y, 0f);
				Recalculate();
			}

			// Here we check if the DragableUIPanel is outside the Parent UIElement rectangle. 
			// (In our example, the parent would be ExampleUI, a UIState. This means that we are checking that the DragableUIPanel is outside the whole screen)
			// By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution.
			var parentSpace = Parent.GetDimensions().ToRectangle();
			if (!GetDimensions().ToRectangle().Intersects(parentSpace))
			{
				Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
				Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
				// Recalculate forces the UI system to do the positioning math again.
				Recalculate();
			}
		}

		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{

			CalculatedStyle dimensions = base.GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y); //opposite corner
			Point point2 = new Point(point.X + (int)dimensions.Width - CORNER_SIZE, point.Y + (int)dimensions.Height - CORNER_SIZE);
			int width = point2.X - point.X - CORNER_SIZE;
			int height = point2.Y - point.Y - CORNER_SIZE;

			//Top part drawing
			spriteBatch.Draw(_backgroundTexture, new Vector2(point.X, point.Y), new Rectangle(0, 0, 10, 10), Color.White);
			spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X + CORNER_SIZE, point.Y, width, 10), new Rectangle(CORNER_SIZE + 3, 0, 2, 10), Color.White);
			spriteBatch.Draw(_backgroundTexture, new Vector2(point.X + CORNER_SIZE + width, point.Y), new Rectangle(18, 0, 10, 10), Color.White);

			//Middle part drawing
			spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X, point.Y + CORNER_SIZE, 10, height), new Rectangle(0, CORNER_SIZE + 3, 10, 2), Color.White);
			spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X + CORNER_SIZE, point.Y + CORNER_SIZE, width + 1, height), new Rectangle(13, 13, 2, 2), Color.White);
			spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X + CORNER_SIZE + width + 1, point.Y + CORNER_SIZE, 10, height), new Rectangle(19, CORNER_SIZE + 3, 10, 2), Color.White);

			//Bottom part drawing
			spriteBatch.Draw(_backgroundTexture, new Vector2(point.X, point.Y + height + CORNER_SIZE), new Rectangle(0, 18, 10, 10), Color.White);
			spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X + CORNER_SIZE, point.Y + height + CORNER_SIZE, width, 10), new Rectangle(CORNER_SIZE + 3, 18, 2, 10), Color.White);
			spriteBatch.Draw(_backgroundTexture, new Vector2(point.X + CORNER_SIZE + width, point.Y + height + CORNER_SIZE), new Rectangle(18, 18, 10, 10), Color.White);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (isVisible)
			{
				this.DrawPanel(spriteBatch, _backgroundTexture, Color.Gray);
			}
		}
	}
}
