namespace Breakout
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Game.Instance.ScoreChanged += () =>
            {
                labelScore.Text = "Score: " + Game.Instance.Score.ToString();
                var players = Network.Instance.Sessions
                    .Concat(new[] { new Session { Name = "Kit", Score = Game.Instance.Score } })
                    .OrderByDescending(x => x.Score)
                    .Select(x => new ListViewItem(new[] { $"{x.Name} ({x.Address})", x.Score.ToString() }))
                    .ToArray();
                listView1.Items.Clear();
                listView1.Items.AddRange(players);

            };
            Game.Instance.GameOverChanged += () => MessageBox.Show("Game Over!");
            Load += (s, e) => canvas1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}