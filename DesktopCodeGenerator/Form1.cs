namespace DesktopCodeGenerator
{
    public partial class Form1 : Form
    {
        private ConfigInfo oConfig;
        string fileName = nameof(ConfigInfo) + ".json";

        public Form1()
        {
            InitializeComponent();

            LoadConfig();
        }

        private void CustomDispose()
        {
          
            SaveConfig();
        }

        private void LoadConfig()
        {
            if (!System.IO.File.Exists(fileName))
            {
                oConfig = new ConfigInfo();
                System.IO.File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(oConfig));
            }

            string jsonContent = System.IO.File.ReadAllText(fileName);
            oConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigInfo>(jsonContent);
        }

        private void SaveConfig()
        {
            System.IO.File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(oConfig));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = oConfig.DefaultResultPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DesktopCodeGenerator src = new DesktopCodeGenerator(oConfig.DefaultResultPath);
            src.AddFiles();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            oConfig.DefaultResultPath = this.textBox1.Text;
        }

        public class ConfigInfo
        {          
            

            public string DefaultResultPath { get; set; }

            public ConfigInfo()
            {
                DefaultResultPath = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = this.oConfig.DefaultResultPath;
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            this.textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}