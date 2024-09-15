using System.Diagnostics;

namespace Lab5
{
    public partial class Form1 : Form
    {
        private readonly List<string> _wordsList = new List<string>();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private Button? _buttonLoadFile;
        private Button? _buttonSearch;
        
        private TextBox? _textBoxLoadTime;
        private TextBox? _textBoxSearch;
        private TextBox? _textBoxSearchTime;
        private TextBox? _textBoxMaxDistance;
        
        private ListBox? _listBoxResults;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }
        

        private void InitializeCustomComponents()
        {
            
            _buttonLoadFile = new Button();
            _buttonSearch = new Button();
            
            _textBoxLoadTime = new TextBox();
            _textBoxSearch = new TextBox();
            _textBoxSearchTime = new TextBox();
            _textBoxMaxDistance = new TextBox();
            
            _listBoxResults = new ListBox();

            
            _buttonLoadFile.Location = new Point(30, 20);
            _buttonLoadFile.Size = new Size(120, 30);
            _buttonLoadFile.Text = "Загрузить файл";
            _buttonLoadFile.Click += ButtonLoadFile_Click;

            _buttonSearch.Location = new Point(30, 90);
            _buttonSearch.Size = new Size(120, 30);
            _buttonSearch.Text = "Найти слово";
            _buttonSearch.Click += ButtonSearch_Click;

            
            _textBoxLoadTime.Location = new Point(180, 25);
            _textBoxLoadTime.ReadOnly = true;
            _textBoxLoadTime.Size = new Size(200, 20);

            _textBoxSearch.Location = new Point(180, 95);
            _textBoxSearch.Size = new Size(200, 20);

            _textBoxSearchTime.Location = new Point(180, 130);
            _textBoxSearchTime.ReadOnly = true;
            _textBoxSearchTime.Size = new Size(200, 20);

            _textBoxMaxDistance.Location = new Point(400, 95);
            _textBoxMaxDistance.Size = new Size(200, 20);

            
            _listBoxResults.Location = new Point(30, 170);
            _listBoxResults.Size = new Size(350, 150);
            
            
            Controls.Add(_buttonLoadFile);
            Controls.Add(_buttonSearch);
            
            Controls.Add(_textBoxLoadTime);
            Controls.Add(_textBoxSearch);
            Controls.Add(_textBoxSearchTime);
            Controls.Add(_textBoxMaxDistance);
            
            Controls.Add(_listBoxResults);
            
        }

        private void ButtonLoadFile_Click(object? sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _stopwatch.Start();

                string content = File.ReadAllText(openFileDialog.FileName);
                string[] words = content.Split();

                foreach (var word in words)
                {
                    if (!_wordsList.Contains(word))
                    {
                        _wordsList.Add(word);
                    }
                }

                _stopwatch.Stop();
                if (_textBoxLoadTime != null)
                    _textBoxLoadTime.Text = $"Время загрузки: {_stopwatch.ElapsedMilliseconds} мс";
            }
        }

        private void ButtonSearch_Click(object? sender, EventArgs e)
        {
            var searchTerm = _textBoxSearch?.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Введите слово для поиска!");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(_textBoxMaxDistance?.Text))
            {
                MessageBox.Show("Введите слово для поиска!");
                return;
            }
            
            _stopwatch.Start();

            _listBoxResults?.BeginUpdate();
            _listBoxResults?.Items.Clear();
            
            for(int l = 0; l<_wordsList.Count; l++)
            {
                var m = _wordsList[l].Length;
                var n = searchTerm!.Length;

                var maxDistance = Convert.ToInt32(_textBoxMaxDistance?.Text);
                

                int[,] matrix = new int[m + 1, n + 1];

                for (var i = 0; i <= m; i++) matrix[i, 0] = i;
                for (var i = 0; i <= n; i++) matrix[0, i] = i;

                for (int i = 1; i <= m; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        int cost = (_wordsList[l][i - 1] == searchTerm[j - 1]) ? 1 : 0;

                        matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                            matrix[i - 1, j - 1] + cost);
                    }
                }

                if (matrix[m, n] <= maxDistance)
                {
                    _listBoxResults?.Items.Add(_wordsList[l]);
                }
            }
            
            _listBoxResults?.EndUpdate();

            _stopwatch.Stop();
            if (_textBoxSearchTime != null)
                _textBoxSearchTime.Text = $"Время поиска: {_stopwatch.ElapsedMilliseconds} мс";
        }
    }
}
