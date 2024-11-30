using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SQLQueryStress.Controls
{
    public class GanttChartControl : UserControl
    {
        private VScrollBar _verticalScrollBar;
        private HScrollBar _horizontalScrollBar;
        private Panel _chartPanel;
        private readonly float _timeScale = 20.0f; // pixels per second
        private readonly int _rowHeight = 20;
        private readonly int _rowSpacing = 4;
        private DateTime _ganttStartTime;
        private readonly List<GanttItem> _ganttItems = new List<GanttItem>();
        private readonly Random _random = new Random(42);
        private ToolTip _tooltip = new ToolTip();
        private Point _lastMousePosition;

        public GanttChartControl()
        {
            InitializeComponents();
            InitializeGanttChart();
        }

        public void AddGanttItem(GanttItem item)
        {
            _ganttItems.Add(item);
            _chartPanel.Invalidate();
        }

        public void AddGanttItem(int row, DateTime startTime, int durationMS)
        {
            _ganttItems.Add(new GanttItem
            {
                Row = row,
                StartTime = startTime,
                Duration = TimeSpan.FromMilliseconds(durationMS),
                Color = Color.FromArgb(_random.Next(64, 255), _random.Next(64, 255), _random.Next(64, 255))
            });
        }

        public void ClearItems(){
            _ganttStartTime = DateTime.Now;
            _ganttItems.Clear();
            _chartPanel.Invalidate();
        }

        private void InitializeComponents()
        {
            // Initialize panel
            _chartPanel = new Panel();
            _chartPanel.Dock = DockStyle.Fill;
            _chartPanel.BackColor = Color.White;
            _chartPanel.Paint += GanttChartPanel_Paint;
            _chartPanel.Resize += (s, e) => UpdateScrollBars();

            // Initialize scroll bars
            _verticalScrollBar = new VScrollBar();
            _horizontalScrollBar = new HScrollBar();
            
            _verticalScrollBar.Dock = DockStyle.Right;
            _horizontalScrollBar.Dock = DockStyle.Bottom;
            
            _verticalScrollBar.Scroll += (s, e) => _chartPanel.Invalidate();
            _horizontalScrollBar.Scroll += (s, e) => _chartPanel.Invalidate();

            // Add mouse move handler to chart panel
            _chartPanel.MouseMove += ChartPanel_MouseMove;

            // Add controls
            Controls.Add(_chartPanel);
            Controls.Add(_verticalScrollBar);
            Controls.Add(_horizontalScrollBar);
        }

        private void InitializeGanttChart()
        {
            // Create dummy data
            _ganttStartTime = DateTime.Now;
           
            UpdateScrollBars();
        }

        private void UpdateScrollBars()
        {
            if (_ganttItems.Count == 0) return;

            // Calculate content size
            var lastEndTime = _ganttItems.Max(x => x.StartTime.Add(x.Duration));
            var totalSeconds = (lastEndTime - _ganttStartTime).TotalSeconds;
            var contentWidth = (int)(totalSeconds * _timeScale);
            var contentHeight = 10 * (_rowHeight + _rowSpacing);

            // Update horizontal scrollbar
            _horizontalScrollBar.Maximum = Math.Max(0, contentWidth - _chartPanel.Width + _horizontalScrollBar.LargeChange);
            _horizontalScrollBar.LargeChange = _chartPanel.Width / 4;
            _horizontalScrollBar.SmallChange = _chartPanel.Width / 10;

            // Update vertical scrollbar
            _verticalScrollBar.Maximum = Math.Max(0, contentHeight - _chartPanel.Height + _verticalScrollBar.LargeChange);
            _verticalScrollBar.LargeChange = _chartPanel.Height / 4;
            _verticalScrollBar.SmallChange = _rowHeight;
        }

        private void GanttChartPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            
            // Apply scrolling offset
            e.Graphics.TranslateTransform(-_horizontalScrollBar.Value, -_verticalScrollBar.Value);

            // Draw row backgrounds
            using (var brush = new SolidBrush(Color.FromArgb(245, 245, 245)))
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i % 2 == 0) continue;
                    e.Graphics.FillRectangle(brush, 0, i * (_rowHeight + _rowSpacing), 
                        _chartPanel.Width + _horizontalScrollBar.Value, _rowHeight);
                }
            }
            Debug.WriteLine($"Drawing {_ganttItems.Count} items");
            int c = 0;
            // Draw items
            foreach (var item in _ganttItems.OrderBy(x=>x.Row))
            {
                var offsetSeconds = (item.StartTime - _ganttStartTime).TotalSeconds;
                var x = (float)(offsetSeconds * _timeScale);
                var y = item.Row * (_rowHeight + _rowSpacing);
                var width = (float)(item.Duration.TotalSeconds * _timeScale);

                if (item.Row == 1)
                {
                    Debug.WriteLine($"Drawing {c++},{item.StartTime.Millisecond},{item.Duration.TotalMilliseconds}{x},{y},{width}");
                }
                using (var brush = new SolidBrush(item.Color))
                {
                    var rect = new RectangleF(x, y, width, _rowHeight);
                    e.Graphics.FillRectangle(brush, rect);
                    
                    using (var pen = new Pen(Color.FromArgb(100, 100, 100)))
                    {
                        e.Graphics.DrawRectangle(pen, x, y, width, _rowHeight);
                    }
                }
            }

            // Draw time scale
            using (var pen = new Pen(Color.Black))
            using (var font = new Font("Arial", 8))
            using (var brush = new SolidBrush(Color.Black))
            {
                for (int i = 0; i <= 60; i += 5)
                {
                    var x = i * _timeScale;
                    e.Graphics.DrawLine(pen, x, 0, x, 10 * (_rowHeight + _rowSpacing));
                    e.Graphics.DrawString($"{i}s", font, brush, x, -15);
                }
            }
        }

        private void ChartPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // Convert mouse coordinates to chart coordinates by adding scroll offset
            var chartX = e.X + _horizontalScrollBar.Value;
            var chartY = e.Y + _verticalScrollBar.Value;

            // Find the row based on Y position
            var row = chartY / (_rowHeight + _rowSpacing);
            
            // Convert X position to time
            var secondsFromStart = chartX / _timeScale;
            var timeAtCursor = _ganttStartTime.AddSeconds(secondsFromStart);

            // Find matching GanttItem
            var item = _ganttItems.FirstOrDefault(x => 
                x.Row == row && 
                timeAtCursor >= x.StartTime && 
                timeAtCursor <= x.StartTime.Add(x.Duration));

            if (item != null)
            {
                var tooltipText = $"Start: {item.StartTime:HH:mm:ss.fff}{Environment.NewLine}" +
                                 $"Duration: {item.Duration.TotalMilliseconds:F0}ms";
                
                // Only show tooltip if mouse position changed
                if (_lastMousePosition != e.Location)
                {
                    _tooltip.Show(tooltipText, _chartPanel, e.X, e.Y + 20);
                    _lastMousePosition = e.Location;
                }
            }
            else
            {
                _tooltip.Hide(_chartPanel);
                _lastMousePosition = Point.Empty;
            }
        }
    }

    public class GanttItem
    {
        public int Row { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Color Color { get; set; }
    }
} 