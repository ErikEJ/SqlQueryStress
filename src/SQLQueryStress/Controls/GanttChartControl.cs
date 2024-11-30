using System;
using System.Collections.Generic;
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

        public GanttChartControl()
        {
            InitializeComponents();
            InitializeGanttChart();
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

            // Add controls
            Controls.Add(_chartPanel);
            Controls.Add(_verticalScrollBar);
            Controls.Add(_horizontalScrollBar);
        }

        private void InitializeGanttChart()
        {
            // Create dummy data
            _ganttStartTime = DateTime.Now;
            var random = new Random(42);
            
            for (int i = 0; i < 50; i++)
            {
                var row = random.Next(0, 10);
                var startOffset = random.Next(0, 60);
                var duration = random.Next(1, 20);
                
                _ganttItems.Add(new GanttItem
                {
                    Row = row,
                    StartTime = _ganttStartTime.AddSeconds(startOffset),
                    Duration = TimeSpan.FromSeconds(duration),
                    Color = Color.FromArgb(random.Next(64, 255), random.Next(64, 255), random.Next(64, 255))
                });
            }

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

            // Draw items
            foreach (var item in _ganttItems)
            {
                var offsetSeconds = (item.StartTime - _ganttStartTime).TotalSeconds;
                var x = (float)(offsetSeconds * _timeScale);
                var y = item.Row * (_rowHeight + _rowSpacing);
                var width = (float)(item.Duration.TotalSeconds * _timeScale);
                
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
    }

    public class GanttItem
    {
        public int Row { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Color Color { get; set; }
    }
} 