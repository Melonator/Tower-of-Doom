using System.Collections.Generic;
using SadConsole;
using System;
using Microsoft.Xna.Framework;
using SadConsole.Input;

namespace TowerOfDoom.UI
{
    //A scrollable window that displays messages
    //using a FIFO (first-in-first-out) Queue data structure
    public class MessageLogWindow : Window
    {

        //max number of lines to store in message log
        private static readonly int _maxLines = 100;

        // a Queue works using a FIFO structure, where the first line added
        // is the first line removed when we exceed the max number of lines
        private readonly Queue<string> _lines;

        // the messageConsole displays the active messages
        private SadConsole.ScrollingConsole _messageConsole;

        //scrollbar for message console
        private SadConsole.Controls.ScrollBar _messageScrollBar;

        //Track the current position of the scrollbar
        private int _scrollBarCurrentPosition;

        // account for the thickness of the window border to prevent UI element spillover
        private  int _windowBorderThickness = 2;

        // Create a new window with the title centered
        // the window is draggable by default
        public MessageLogWindow(int width, int height, string title) : base(width, height)

        {
            // Ensure that the window background is the correct colour
            Theme.FillStyle.Background = Color.Black;
            _lines = new Queue<string>();
            CanDrag = true;
            Title = title.Align(HorizontalAlignment.Center, Width);



            // add the message console, reposition, enable the viewport, and add it to the window
            _messageConsole = new SadConsole.ScrollingConsole(width - _windowBorderThickness, _maxLines, SadConsole.Global.FontEmbedded);
            _messageConsole.Position = new Point(1, 1);
            _messageConsole.ViewPort = new Rectangle(0, 0, width - 1, height - _windowBorderThickness);

            // create a scrollbar and attach it to an event handler, then add it to the Window
            _messageScrollBar = new SadConsole.Controls.ScrollBar(SadConsole.Orientation.Vertical, height - _windowBorderThickness);
            _messageScrollBar.Position = new Point(_messageConsole.Width + 1, _messageConsole.Position.X);
            _messageScrollBar.IsEnabled = false;
            _messageScrollBar.ValueChanged += MessageScrollBar_ValueChanged;
            //_messageScrollBar.Position = new Point(_messageConsole.Width, 1);
            Add(_messageScrollBar);

            // enable mouse input
            UseMouse = true;

            // add all child consoles to the window
            Children.Add(_messageConsole);
        }

        // Controls the position of the messagelog viewport
        // based on the scrollbar position using an event handler
        void MessageScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _messageConsole.ViewPort = new Rectangle(0, _messageScrollBar.Value + _windowBorderThickness, _messageConsole.Width, _messageConsole.ViewPort.Height);
        }

        //add a line to the queue of messages
        public void Add(string message)
        {
            _lines.Enqueue(message);
            // when exceeding the max number of lines remove the oldest one
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
            _messageConsole.Cursor.Position = new Point(1, _lines.Count);
            _messageConsole.Cursor.Print(message + '\n');
        }

        //print directly to the queue without adding a new line
        public void Print(string text)
        {
            string[] lines = _lines.ToArray();
            lines[lines.Length] += text;
        }

        //Remember to draw the window!
        public override void Draw(TimeSpan drawTime)
        {
            base.Draw(drawTime);
        }

        // copied directly from http://sadconsole.com/docs/make-a-scrolling-console.html
        // and modified to suit this class variable names
        public override void Update(TimeSpan time)
        {
            base.Update(time);

            // Ensure that the scrollbar tracks the current position of the _messageConsole.
            if (_messageConsole.TimesShiftedUp != 0 | _messageConsole.Cursor.Position.Y >= _messageConsole.ViewPort.Height + _scrollBarCurrentPosition)
            {
                //enable the scrollbar once the messagelog has filled up with enough text to warrant scrolling
                _messageScrollBar.IsEnabled = true;

                // Make sure we've never scrolled the entire size of the buffer
                if (_scrollBarCurrentPosition < _messageConsole.Height - _messageConsole.ViewPort.Height)
                    // Record how much we've scrolled to enable how far back the bar can see
                    _scrollBarCurrentPosition += _messageConsole.TimesShiftedUp != 0 ? _messageConsole.TimesShiftedUp : 1;

                _messageScrollBar.Maximum = (_messageConsole.Height + _scrollBarCurrentPosition) - _messageConsole.Height - _windowBorderThickness;

                // This will follow the cursor since we move the render area in the event.
                _messageScrollBar.Value = _scrollBarCurrentPosition;

                // Reset the shift amount.
                _messageConsole.TimesShiftedUp = 0;
            }
        }
    }
}