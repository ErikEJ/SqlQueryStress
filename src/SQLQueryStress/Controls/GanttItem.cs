using System;
using System.Drawing;

namespace SQLQueryStress.Controls;

public class GanttItem
{
    public int Row { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public Color Color { get; set; }
    internal LoadEngine.QueryOutput QueryOutput { get; set; }
}