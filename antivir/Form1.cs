using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace antivir
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //BOLZANO
        public string[] signatures = 
        {
            "424f4c5a41",  // 0-записывает в logger.bin
            "4e0044656c65746546696c654100",  // 1-копирует себя в корневой каталог Windows с именем poserv.exe
            "47657446696c655479706500",     // 2-GetWindowTextA следит за появлением окон, в заголовке которых содержатся следующие строки:ftp,mail,password,telnet
            "536574456e644f6646696c6500",  // 3-OpenSCManager устанавливает связь с диспетчером управления службами
            "4578697450726f63657373",     // 4-Завершает все потоки
            "526567436c6f73654b657900",  // 5-Завершает все потоки
            "4765744d6f64756c6548616e646c654100", // 6-Извлекает дескриптор указанного модуля
            "e800000000", // 7-FindWindowA ищет окна с подходящим названием
            "55524c446f776e6c6f6164546f46696c654100",  // 8-CreateFileA
            "574e65744f70656e456e756d4100",   // 9-DeleteFileA
            "ff15b8104000",   // 10-GetLocalTime
            "ff152c104000"
        };

        public List<bool> checkResults = new List<bool>();

        string[] files = { };

        // провверка, исполняемый файл или нет
        private bool CheckIsPE(string filePath)
        {
            var offset = FindOffset(filePath);

            return CheckIsPeByOffset(filePath, offset);
        }

        private static bool CheckIsPeByOffset(string filePath, int offset)
        {
            long byteNumber=0;
            var ep = false;
            var pe = false;
            int readByte;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                do
                {
                    readByte = fs.ReadByte();
                    if ((byteNumber == offset) && (readByte.ToString("x2") == "50")
                    ) //X2 prints the string as two uppercase hexadecimal characters
                    {
                        ep = true;
                    }

                    if ((byteNumber == offset + 1) && (readByte.ToString("x2") == "45") && ep)
                    {
                        pe = true;
                    }

                    byteNumber++;
                } while (readByte != -1);
            }

            return pe;
        }

        private static int FindOffset(string filePath)
        {
            long byteNumber = 0;
            var readByte = 0;
            var offset = 0;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                do
                {
                    readByte = fs.ReadByte();
                    if (byteNumber == 60)
                    {
                        offset = readByte;
                    }

                    if (byteNumber == 61)
                    {
                        offset += readByte * 16 * 16;
                    }

                    byteNumber++;
                } while (readByte != -1);
            }

            return offset;
        }

        // поиск сигнатуры
        private bool Find(string signature, string fileName)
        {
            var isFound = false;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int counter = 0;
                int b = 0;
                string signatures1 = "";
                string buf = "";
                do
                {
                    b = fs.ReadByte();
                    buf = b.ToString("x2");
                    string buf1 = signature.Substring(0, 2);
                    if (buf1 == buf)
                    {
                        signatures1 = buf;
                        counter = signature.Length / 2;
                        for (int i = 0; i < counter - 1; i++)
                        {
                            b = fs.ReadByte();
                            buf = b.ToString("x2");
                            signatures1 += buf;

                        }
                        if (signature == signatures1)
                        {
                            isFound = true;
                            b = -1;
                        }
                    }
                }
                while (b != -1);
            }

            return isFound;
        }

        private void bSelectDir_Click(object sender, EventArgs e)
        {
            selectDir();
        }

        // выбор папки для проверки
        private void selectDir()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tbDirectory.Text = dialog.SelectedPath;
                }
                files = Directory.GetFiles(tbDirectory.Text);
            }
        }

        private void bHeuristic_Click(object sender, EventArgs e)
        {
            if (files.Length == 0)
            {
                selectDir();
            }

            dgvStatistic.Rows.Clear();

            int idx = 0;

            for (int index = 0; index < files.Length; index++)
            {
                dgvStatistic.Rows.Add();
                string[] buf = new string[10];
                double[] p = new double[10];

                if (CheckIsPE(files[index]) == true)
                {
                    checkResults.Clear();
                    var probabilities = new ArrayList();
                    //var prArr = new List<double>();
                    ArrayList comment = new ArrayList();
                    foreach (var sign in signatures)
                    {
                        checkResults.Add(Find(sign, files[index]));
                    }
                    idx = probabilities.Add(0.0);
                    comment.Add("");

                    if (checkResults[7] && checkResults[2] && checkResults[1])
                    {

                            probabilities[idx] = 0.9;
                           comment[idx] = "Ищет различные окна\n "
                                +"Ищет окна с подходящим названием\n "
                                +"Копирует себя в корневой каталог Windows с именем poserv.exe\n";


                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");

                    if (checkResults[7] && checkResults[2] && checkResults[4] && checkResults[5])
                    {
 
                            probabilities[idx] = 0.7;
                           comment[idx] = "Ищет различные окна\n "
                                + "Ищет окна с подходящим названием\n "
                                + "Завершает все потоки";


                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");

                    if (checkResults[1])
                    {

                            probabilities[idx] = 0.8;
                           comment[idx] = "Копирует себя в корневой каталог Windows с именем poserv.exe\n";


                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");

                    if (checkResults[4] && checkResults[5])
                    {

                            probabilities[idx] = 0.5;
                           comment[idx] = "Завершает все потоки";

                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");
                    if (checkResults[7] && checkResults[8])
                    {
                        probabilities[idx] = 0.6;
                        comment[idx] = "Создает и удаляет файлы\n";
                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");
                    if (checkResults[10])
                    {

                            probabilities[idx] = 0.05;
                           comment[idx] = "GetLocalTime\n";

                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");
                    if (checkResults[7])
                    {

                            probabilities[idx] = 0.4;
                            comment[idx] = "Извлекает дескриптор указанного модуля\n";
                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");
                    if (checkResults[7] && checkResults[2] && checkResults[3] && checkResults[4] && checkResults[5])
                    {

                            probabilities[idx] = 0.8;
                            comment[idx] = "Ищет различные окна\n "
                                 + "Ищет окна с подходящим названием\n "
                                 + "Завершает все потоки\n "
                                 + "OpenSCManager устанавливает связь с диспетчером управления службами\n";

                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");
                    if (checkResults[3])
                    {

                            probabilities[idx] = 0.2;
                            comment[idx] = "OpenSCManager устанавливает связь с диспетчером управления службами\n";
      

                    }

                    idx = probabilities.Add(0.0);
                    comment.Add("");
                    if (checkResults[7])
                    {

                            probabilities[idx] = 0.1;


                    }

                    FillDataGrid(probabilities, comment, index);
                }
                else
                {
                    dgvStatistic.Rows[index].Cells[0].Value = files[index];
                    dgvStatistic.Rows[index].Cells[2].Value = "Не исполняемый файл";
                }
            }
        }

        private void FillDataGrid(ArrayList prArr, ArrayList comments, int i)
        {
            var comment = "";
            double probability = 0;
            for (var u=0; u < prArr.Count; u++)
            {
                double vsp_p = (double)prArr[u];
                if (vsp_p > probability)
                {
                    probability = (double)prArr[u];
                    comment = (string) comments[u];
                }
            }

            probability = SetBackgroundColor(probability, i);

            dgvStatistic.Rows[i].Cells[0].Value = files[i];
            if (probability == 0) dgvStatistic.Rows[i].Cells[2].Value = "Просто файл";
            else dgvStatistic.Rows[i].Cells[2].Value = comment;

            dgvStatistic.Rows[i].Cells[1].Value = probability;
        }

        private double SetBackgroundColor(double probability, int i)
        {
            //if (probability < 0.5) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Green;
            if ((probability >= 0.4) && (probability < 0.85)) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
            if (probability > 1) probability = 1;
            if (probability >= 0.8) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            return probability;
        }

        private void bSignature_Click(object sender, EventArgs e)
        {
            if (files.Length == 0)
            {
                selectDir();
            }

            dgvStatistic.Rows.Clear();

            for (int i = 0; i < files.Length; i++)
            {
                
                dgvStatistic.Rows.Add();
                string buf = "Не исполняемый файл!";
                if (CheckIsPE(files[i]) == true)
                {

                    bool flagSign = Find(signatures[1], files[i]);
                    if (flagSign)
                    {
                        buf = "Danger! Virus.Win32.Bolzano";
                        dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else buf = "Исполняемый файл";

                }

                dgvStatistic.Rows[i].Cells[0].Value = files[i];
                dgvStatistic.Rows[i].Cells[2].Value = buf;
            }
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            dgvStatistic.Rows.Clear();
        }
    }

}