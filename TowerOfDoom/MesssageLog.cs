using System.Collections.Generic;
using SadConsole;
using System;
using Microsoft.Xna.Framework;

namespace TowerOfDoom
{
    public class MessageLogWindow : Window
    {
        private static readonly int _maxLines = 15;

        private readonly Queue<string> _lines;

        private SadConsole.ScrollingConsole _messageConsole;

        private int _windowBorderThickness = 2;

        public MessageLogWindow(int width, int height, string title) : base(width, height)
        {
            Theme.FillStyle.Background = DefaultBackground;
            _lines = new Queue<string>();
            CanDrag = false;

            _messageConsole = new SadConsole.ScrollingConsole(width - _windowBorderThickness, _maxLines, SadConsole.Global.FontEmbedded);
            _messageConsole.Position = new Point(1, 1);
            _messageConsole.ViewPort = new Rectangle(0, 0, width / 2, height - _windowBorderThickness);

            Title = title.Align(HorizontalAlignment.Center, Width - 5, (char)32);

            UseMouse = true;

            Children.Add(_messageConsole);
        }
        public void Add(string message)
        {
            _lines.Enqueue(message);

            {
                _lines.Dequeue();
            }

            _messageConsole.Cursor.Position = new Point(1, _lines.Count);
            _messageConsole.Cursor.Print(message + "\n");
        }
    }
}
