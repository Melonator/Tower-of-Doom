using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Console = SadConsole.Console;

namespace TowerOfDoom.UI
{
    class DrawImageComponent : SadConsole.Components.DrawConsoleComponent, System.IDisposable
    {
        public PositionModes PositionMode { get; set; } = PositionModes.Cells;

        public Point PositionOffset { get; set; } = new Point(0, 0);

        private Texture2D _image;
        private bool _isDisposed;

        public DrawImageComponent(string filePath)
        {
            using (var stream = System.IO.File.OpenRead(filePath))
                _image = Texture2D.FromStream(SadConsole.Global.GraphicsDevice, stream);
        }

        ~DrawImageComponent()
        {
            Dispose();
        }

        public override void Draw(Console console, System.TimeSpan delta)
        {
            if (PositionMode == PositionModes.Cells)
                SadConsole.Global.DrawCalls.Add(new SadConsole.DrawCalls.DrawCallTexture(_image, (console.CalculatedPosition + console.Font.GetWorldPosition(PositionOffset)).ToVector2()));
            else
                SadConsole.Global.DrawCalls.Add(new SadConsole.DrawCalls.DrawCallTexture(_image, (console.CalculatedPosition + PositionOffset).ToVector2()));
        }

        public void Dispose()
        {
            if (!_isDisposed)
                _image.Dispose();

            _isDisposed = true;
        }

        public enum PositionModes
        {
            Pixels,
            Cells
        }
    }
}
