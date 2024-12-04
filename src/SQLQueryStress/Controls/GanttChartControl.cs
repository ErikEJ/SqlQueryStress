using Microsoft.SqlServer.XEvent.XELite;
using SQLQueryStress.Forms;
using System;
using System.Collections.Concurrent;
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
        private float _timeScale = 2000.0f; // pixels per second (start at 2 pixels per millisecond)
        private readonly int _rowHeight = 20;
        private readonly int _rowSpacing = 4;
        private DateTime _ganttStartTime;
        private readonly List<GanttItem> _ganttItems = new List<GanttItem>();
        private readonly Random _random = new Random(42);
        private ToolTip _tooltip = new ToolTip();
        private Point _lastMousePosition;
        private const float MIN_SCALE = 100.0f; // 0.1 pixel per millisecond
        private const float MAX_SCALE = 10000.0f; // 10 pixels per millisecond
        private const float ZOOM_FACTOR = 1.2f;

        private readonly ConcurrentDictionary<Guid, List<IXEvent>> _events;

        public GanttChartControl(ConcurrentDictionary<Guid, List<IXEvent>> events)
        {
            InitializeComponents();
            InitializeGanttChart();
        
            _events = events;
        }

        public void ZoomIn()
        {
            _timeScale = Math.Min(_timeScale * ZOOM_FACTOR, MAX_SCALE);
            UpdateScrollBars();
            _chartPanel.Invalidate();
        }

        public void ZoomOut()
        {
            _timeScale = Math.Max(_timeScale / ZOOM_FACTOR, MIN_SCALE);
            UpdateScrollBars();
            _chartPanel.Invalidate();
        }

        public void FitToData()
        {
            if (_ganttItems.Count == 0) return;

            // Find the time range of the data
            var startTime = _ganttItems.Min(x => x.StartTime);
            var endTime = _ganttItems.Max(x => x.StartTime.Add(x.Duration));
            var totalSeconds = (endTime - startTime).TotalSeconds;

            // Calculate scale needed to fit the width
            var availableWidth = _chartPanel.Width - 50; // Leave some margin
            _timeScale = (float)(availableWidth / totalSeconds);

            // Ensure minimum scale shows 5ms as 10 pixels
            _timeScale = Math.Max(MIN_SCALE, Math.Min(_timeScale, MAX_SCALE));
            
            // Update start time to include some padding
            _ganttStartTime = startTime.AddMilliseconds(-100); // 100ms padding

            UpdateScrollBars();
            _chartPanel.Invalidate();
        }

        internal void AddGanttItem(int row, DateTime startTime, int durationMS, LoadEngine.QueryOutput queryOutput)
        {
            _ganttItems.Add(new GanttItem
            {
                Row = row,
                StartTime = startTime,
                Duration = TimeSpan.FromMilliseconds(durationMS),
                Color = Color.FromArgb(_random.Next(64, 255), _random.Next(64, 255), _random.Next(64, 255)),
                QueryOutput = queryOutput
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

            // Add mouse wheel handler for zooming
            _chartPanel.MouseWheel += ChartPanel_MouseWheel;

            // Add mouse click handler
            _chartPanel.MouseClick += ChartPanel_MouseClick;

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
            // Draw items with millisecond precision
            foreach (var item in _ganttItems.OrderBy(x => x.Row))
            {
                var offsetSeconds = (item.StartTime - _ganttStartTime).TotalSeconds;
                var x = (float)(offsetSeconds * _timeScale);
                var y = item.Row * (_rowHeight + _rowSpacing);
                var width = Math.Max(2.0f, (float)(item.Duration.TotalSeconds * _timeScale)); // Minimum 2px width
                
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

            // Draw time scale with millisecond precision
            using (var pen = new Pen(Color.Black))
            using (var font = new Font("Arial", 8))
            using (var brush = new SolidBrush(Color.Black))
            {
                // Calculate appropriate time interval based on zoom level
                float pixelsPerInterval = 100; // Desired pixels between markers
                float millisecondsPerInterval = (pixelsPerInterval / _timeScale) * 1000;
                
                // Round to a nice interval
                float intervalMs;
                if (millisecondsPerInterval >= 1000) intervalMs = 1000; // 1 second
                else if (millisecondsPerInterval >= 500) intervalMs = 500;
                else if (millisecondsPerInterval >= 250) intervalMs = 250;
                else if (millisecondsPerInterval >= 100) intervalMs = 100;
                else if (millisecondsPerInterval >= 50) intervalMs = 50;
                else if (millisecondsPerInterval >= 25) intervalMs = 25;
                else if (millisecondsPerInterval >= 10) intervalMs = 10;
                else intervalMs = 5;

                float intervalSeconds = intervalMs / 1000f;

                // Draw time markers
                float visibleStartSeconds = _horizontalScrollBar.Value / _timeScale;
                float visibleEndSeconds = (_horizontalScrollBar.Value + _chartPanel.Width) / _timeScale;

                for (float i = (float)Math.Floor(visibleStartSeconds / intervalSeconds) * intervalSeconds; 
                     i <= visibleEndSeconds; 
                     i += intervalSeconds)
                {
                    var x = i * _timeScale;
                    e.Graphics.DrawLine(pen, x, 0, x, 10 * (_rowHeight + _rowSpacing));
                    
                    string timeText;
                    if (intervalMs >= 1000)
                        timeText = $"{Math.Floor(i)}s";
                    else
                        timeText = $"{Math.Floor(i * 1000)}ms";
                    
                    e.Graphics.DrawString(timeText, font, brush, x, -15);
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
            
            // Convert X position to time with millisecond precision
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
                                 $"Duration: {item.Duration.TotalMilliseconds:F3}ms{Environment.NewLine}" +
                                 $"Context:{item.QueryOutput.context.ToString()}";

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

        private void ChartPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.Delta > 0)
                    ZoomIn();
                else
                    ZoomOut();
            }
        }

        private void ChartPanel_MouseClick(object sender, MouseEventArgs e)
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

            if (item != null && item.QueryOutput != null)
            {
                var detailForm = new QueryDetailForm(item.QueryOutput,_events);
                detailForm.Show();
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case GanttMessages.WM_FIT_TO_DATA:
                    FitToData();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
} 