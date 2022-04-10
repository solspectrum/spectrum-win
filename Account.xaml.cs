using QRCoder;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spectrum
{

    public partial class Account : Window
    {
        StringBuilder transactions = new StringBuilder();
        public Account()
        {
            InitializeComponent();
            TxBox.Document.Blocks.Clear();
            balance();            
        }

        private void OnTabSelected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                txhistory();
            }
        }

        private void txhistory()
        {
            
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"\\1.9.11\\solana-release\\bin\\solana.exe",
                    Arguments = "transaction-history tTfbP9xMQ3FXyuyFsK13eeTZp6ebX1FAY1YUNCzRvFr",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true

                }
            };

            process.Start();


            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();

                if (line is not null)
                {
                    transactions.AppendLine(line);
                }

            }

            TxBox.AppendText(transactions.ToString());
        }

        private void balance()
        {
            StringBuilder balance = new StringBuilder();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"\\1.9.11\\solana-release\\bin\\solana.exe",
                    Arguments = "balance tTfbP9xMQ3FXyuyFsK13eeTZp6ebX1FAY1YUNCzRvFr",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true

                }
            };

            process.Start();


            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();

                if (line is not null)
                {
                    balance.AppendLine(line);
                }

            }

            balance_label.Content= balance.ToString();
        }

        private void qrcode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(AddressList.SelectedItem.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);           

            //TODO: Print QR Code for each address when selected in listbox
        }

        private void AddressList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            qrcode();
        }
    }


}
