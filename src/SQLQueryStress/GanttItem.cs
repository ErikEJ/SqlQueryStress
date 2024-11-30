using System;
using System.Drawing;

public class GanttItem
{
    public int Row { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public Color Color { get; set; }
} 