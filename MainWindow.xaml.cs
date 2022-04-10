using System.Windows;
using System.Diagnostics;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls;

namespace Spectrum
{
    public partial class MainWindow : Window
    {
        StringBuilder addresses = new StringBuilder();

        public MainWindow()
        {
            
            InitializeComponent();
            seed_words.Document.Blocks.Clear();
        }

        private void DeriveAddresses_Click(object sender, RoutedEventArgs e)
        {

            Account Addresses = new Account();
            Addresses.Show();
            this.Close();

            solanakeygen(1);            

            Addresses.AddressList.Items.Clear();
            Addresses.AddressList.Items.Add(addresses.ToString());

            for (int i = 2;i< 9;i++)
            {
                addresses.Clear();
                solanakeygen(i);
                Addresses.AddressList.Items.Add(addresses.ToString());

            }

        }

        string Seedbox(RichTextBox words)
        {
            TextRange textRange = new TextRange(
                words.Document.ContentStart,

                words.Document.ContentEnd
            );
            return textRange.Text;
        }

        private void solanakeygen(int index)
        {
            string words = Seedbox(seed_words);
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"\\1.9.11\\solana-release\\bin\\solana-keygen.exe",
                    Arguments = "pubkey prompt://?key=0/"+index,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true

                }
            };

            process.Start();

            process.StandardInput.Write(words);
            process.StandardInput.Write("\n");
            process.StandardInput.Write("\n");            


            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();

                if (line is not null)
                {
                    addresses.AppendLine(line);
                }

            }
        }
    }
}
