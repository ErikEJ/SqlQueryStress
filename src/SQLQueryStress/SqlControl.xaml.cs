using System.Windows;

namespace SQLQueryStress
{
    /// <summary>
    /// Interaction logic for SqlControl.xaml
    /// </summary>
    public partial class SqlControl
    {
        public SqlControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return AvalonEdit.Text; }
            set { AvalonEdit.Text = value; }
        }

        private void SqlControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("SQLQueryStress.Resources.SQL.xshd"))
            {
                if (stream == null) return;
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    AvalonEdit.SyntaxHighlighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                            ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }
        }
    }
}
