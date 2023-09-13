using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace InterfaceDesignTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> allStringsAtFile = new List<string>(); // все строки текстового файла
        private string relativePath = @"..\..\Files\Data.txt"; // относительный путь к файлу
        public MainWindow()
        {
            InitializeComponent();
            string[] allSex = { "Мужской", "Женский", "Другой" };
            foreach (string s in allSex)
            {
                listSexUpload.Items.Add(s);
            }
            allStringsAtFile = GetDataAtFile(relativePath);
        }
        private List<string> GetDataAtFile(string path) // метод получения данных из файла
        {
            List<string> allData = new List<string>();
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        allData.Add(line);
                    }
                }
                return allData;
            }
        }
        private void WriteLineAtFile(string path, string line) // метод записи данных в файл
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(line);
            }
            allStringsAtFile.Add(line);
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CheckBox> checkedItems = new List<CheckBox>(); //лист для хранения выбранных пользователем элементов
                foreach (var item in listInformationUpload.Items.OfType<CheckBox>())
                {
                    if (item.IsChecked == true)
                    {
                        checkedItems.Add(item);
                    }
                }
                if (txtNameUpload.Text != "" && dataUpload.Text != "" && listSexUpload.SelectedItem != null && checkedItems.Count > 0)
                {
                    string name = txtNameUpload.Text;
                    if (name.Contains(" "))
                    {
                        name = name.Replace(' ', '_');
                        MessageBox.Show("Пробелы во введенном имени были заменены на нижнее подчеркивание");
                    }
                    string newData = name + " " + dataUpload.Text + " " + listSexUpload.SelectedItem.ToString() + " ";
                    foreach (var item in checkedItems)
                    {
                        newData += item.Content.ToString() + " ";
                    }
                    WriteLineAtFile(relativePath, newData);
                    MessageBox.Show("Данные успешно записаны");
                }
                else MessageBox.Show("Вы заполнили не все поля. Заполните поля и повторите попытку");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошибка, перезапустите программу и повторите попытку");
            }
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allStringsAtFile.Count > 0)
                {
                    GetData();
                    btnRead.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Данные в файле закончились. Добавьте новые данные или перезапустите программу");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошибка, перезапустите программу и повторите попытку");
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allStringsAtFile.Count > 0) GetData();
                else MessageBox.Show("Данные в файле закончились. Добавьте новые данные или перезапустите программу");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошибка, перезапустите программу и повторите попытку");
            }
        }

        private void GetData() //метод получения данных и их вывода для пользователя
        {
            string strForOutput = allStringsAtFile.First();
            string[] allWordsAtStr = strForOutput.Split(' ');
            txtNameOutput.Text = allWordsAtStr[0];
            txtDataOutput.Text = allWordsAtStr[1];
            txtSexOutput.Text = allWordsAtStr[2];
            string info = "";
            for (int i = 3; i < allWordsAtStr.Length; i++)
            {
                info += allWordsAtStr[i] + " ";
            }
            txtInformationOutput.Text = info;
            allStringsAtFile.Remove(strForOutput);
        }
    }
}
