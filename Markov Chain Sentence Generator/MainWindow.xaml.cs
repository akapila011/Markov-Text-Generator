using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace Markov_Chain_Sentence_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Markov mk = null;
        String text = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDailog = new OpenFileDialog();
            openFileDailog.FileName = ""; // Default file name
            openFileDailog.DefaultExt = ".txt"; // Default file extension
            openFileDailog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            Nullable<bool> result = openFileDailog.ShowDialog();
            if (result == true)
            {
                // Open document
                string filepath = openFileDailog.FileName;
                try
                {
                    text = File.ReadAllText(filepath);
                    filepath_textBox.Text = filepath;
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("There was a problem reading the slected file. Error: " + ioEx.Message);
                    text = null;
                    filepath_textBox.Text = "";
                }
            }
        }   // end SelectFile()

        private void TrainModel(object sender, RoutedEventArgs e)
        {
            if (text == null)
            {
                MessageBox.Show("There is no text available for training.");
                return;
            }
            mk = new Markov("Test", text);
            try
            {
                mk.Train();
                MessageBox.Show("Successfully Trained Markov model. You can now generate sentences.");
                trainingStatus_label.Content = "Trained";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                trainingStatus_label.Content = "Not Trained";
            }
        }

        private void GenerateText(object sender, RoutedEventArgs e)
        {
            if (mk == null)
            {
                MessageBox.Show("You must first select a text file and click train before generating text.");
                return;
            }
            if ((String)trainingStatus_label.Content != "Trained")
            {
                MessageBox.Show("There was a problem during loading and training the model. Please try again.");
                return;
            }
            try
            {
                generatedText_textBlock.Text = mk.GenerateSentence();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with generating text. Error: " + ex.Message);
                generatedText_textBlock.Text = "";
            }
        }
    }
}
