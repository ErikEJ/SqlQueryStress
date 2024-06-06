using System.IO;
using System.Windows;

namespace SQLQueryStress
{
    /// <summary>
    /// Interaction logic for SqlControl.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Windows only")]
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

        public string SelectedText
        {
            get { return AvalonEdit.SelectedText; }
        }

        
        private void SqlControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            Stream stream = null;
            try
            {
                stream =
                    System.Reflection.Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SQLQueryStress.Resources.SQL.xshd");
                if (stream == null) return;
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    AvalonEdit.SyntaxHighlighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                            ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }
            finally
            {
                stream?.Dispose();    
            }
        }
    }
}
