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
        public class knowledge
        {
            public string signatures;
            public bool produce = false;
            public string comment;
        }

        public Form1()
        {
            InitializeComponent();

        }
        //public string signatures = "424f4c5a41";
        //BOLZANO
        // public bool produce = false;
        // public string[] signatures = { "424f4c5a41", "4e0044656c65746546696c654100", "47657446696c655479706500", "536574456e644f6646696c6500", "4578697450726f63657373", "526567436c6f73654b657900", "4765744d6f64756c6548616e646c654100", "e800000000", "55524c446f776e6c6f6164546f46696c654100", "574e65744f70656e456e756d4100", "ff15b8104000", "ff152c104000" };
        public string[]  signatures = { "6c6f676765722e6269" // 0-записывает в logger.bin
                , "706f736572762e657865" // 1-копирует себя в корневой каталог Windows с именем poserv.exe
                , "ff15fc614000" // 2-GetWindowTextA следит за появлением окон, в заголовке которых содержатся следующие строки:ftp,mail,password,telnet
                , "ff1538604000" // 3-OpenSCManager устанавливает связь с диспетчером управления службами
                , "4578697450726f63657373" // 4-Завершает все потоки
                , "526567436c6f73654b657900" // 5-Завершает все потоки
                , "4765744d6f64756c6548616e646c654100" // 6-Извлекает дескриптор указанного модуля
                , "ff1518624000" // 7-FindWindowA ищет окна с подходящим названием
                , "ff15f8604000" // 8-CreateFileA
                , "ff15d8604000" // 9-DeleteFileA
                , "ff15b8104000" // 10 -GetLocalTime
                , "ff152c104000" };
        public bool[] produce = { false, false, false, false, false, false, false, false, false, false, false, false };
        public ArrayList produces = new ArrayList();
        public string[] comments = { };

        double p_1 = 1;
        string[] files = { };
        int y = 0;

        // провверка, исполняемый файл или нет
        private bool PEzag(string h)
        {
            bool e = false;
            long g = 0;
            int b = 0;
            bool ep = false;
            bool pe = false;
            int smesh = 0;
            using (FileStream fs = new FileStream(h, FileMode.Open, FileAccess.Read))
            {

                do
                {
                    b = fs.ReadByte();
                    if (g == 60)
                    {
                        smesh = b;
                    }
                    if (g == 61)
                    {
                        smesh += b * 16 * 16;
                    }

                    g++;
                }
                while (b != -1);
            }
            g = 0;
            using (FileStream fs = new FileStream(h, FileMode.Open, FileAccess.Read))
            {

                do
                {
                    b = fs.ReadByte();
                    if ((g == smesh) && (b.ToString("x2") == "50")) //X2 prints the string as two uppercase hexadecimal characters
                    {
                        ep = true;
                    }
                    if ((g == smesh + 1) && (b.ToString("x2") == "45") && ep)
                    {
                        pe = true;
                    }
                    g++;
                }
                while (b != -1);
                if (pe)
                    e = true;
                else
                    e = false;
            }
            return e;
        }

        // поиск сигнатуры
        private bool Find(string sign, string fileName)
        {
            bool glflag = false;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int counter = 0;
                int b = 0;
                string signatures1 = "";
                bool flag = false;
                string buf = "";
                do
                {
                    b = fs.ReadByte();
                    buf = b.ToString("x2");
                    string buf1 = sign.Substring(0, 2);
                    if (buf1 == buf)
                    {
                        signatures1 = buf;
                        counter = sign.Length / 2;
                        for (int i = 0; i < counter - 1; i++)
                        {
                            b = fs.ReadByte();
                            buf = b.ToString("x2");
                            signatures1 += buf;

                        }
                        if (sign == signatures1)
                        {
                            flag = true;
                            b = -1;
                        }
                    }
                }
                while (b != -1);
                if ((flag))
                    glflag = true;
                else
                    glflag = false;
            }

            return glflag;
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

            for (int i = dgvStatistic.Rows.Count - 1; i >= 0; i--)
            {
                dgvStatistic.Rows.Remove(dgvStatistic.Rows[i]);
            }

            int idx = 0;
            // ArrayList probability = new ArrayList();
            // ArrayList comments = new ArrayList();
            double probability = 0;
            String comments = "";

            for (int i = 0; i < files.Length; i++)
            {
                dgvStatistic.Rows.Add();
                string[] buf = new string[10];
                int pp = 0;
                double f = 1;
                double[] p = new double[10];

                //probability.Clear();
                //comments.Clear();
                probability = 0;
                comments = "";
                if (PEzag(files[i]) == true)
                {
                    produces.Clear();
                    ArrayList prArr = new ArrayList();
                    ArrayList comment = new ArrayList();
                    foreach (var sign in signatures)
                    {
                        produces.Add(Find(sign, files[i]));
                    }
                    idx = prArr.Add(0.0);
                    comment.Add("");

                    if ((bool)produces[7] && (bool)produces[2] && (bool)produces[1])
                    {
                        
                        if (0.9 < p_1)
                        {
                            prArr[idx] = 0.9;
                           comment[idx] = "Ищет различные окна\n "
                                +"Ищет окна с подходящим названием\n "
                                +"Копирует себя в корневой каталог Windows с именем poserv.exe\n";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");

                    if ((bool)produces[7] && (bool)produces[2] && (bool)produces[4] && (bool)produces[5])
                    {

                        if (0.7 < p_1)
                        {
                            prArr[idx] = 0.7;
                           comment[idx] = "Ищет различные окна\n "
                                + "Ищет окна с подходящим названием\n "
                                + "Завершает все потоки";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");

                    if ((bool)produces[1])
                    {

                        if (0.5 < p_1)
                        {
                            prArr[idx] = 0.5;
                           comment[idx] = "Копирует себя в корневой каталог Windows с именем poserv.exe\n";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");

                    if ((bool)produces[4] && (bool)produces[5])
                    {

                        if (0.5 < p_1)
                        {
                            prArr[idx] = 0.5;
                           comment[idx] = "Завершает все потоки";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");
                    if ((bool)produces[7] && (bool)produces[8] )
                    {

                        if (0.4 < p_1)
                        {
                            prArr[idx] = 0.4;
                           comment[idx] = "Создает и удаляет файлы\n";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");
                    if ((bool)produces[10])
                    {
                        if (0.05 < p_1)
                        {
                            prArr[idx] = 0.05;
                           comment[idx] = "GetLocalTime\n";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }
                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");
                    if ((bool)produces[7])
                    {

                        if (0.1 < p_1)
                        {
                            prArr[idx] = 0.1;
                            comment[idx] = "Создает файлы\n";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");
                    if ((bool)produces[7] && (bool)produces[2] && (bool)produces[3] && (bool)produces[4] && (bool)produces[5])
                    {

                        if (0.8 < p_1)
                        {
                            prArr[idx] = 0.8;
                            comment[idx] = "Ищет различные окна\n "
                                 + "Ищет окна с подходящим названием\n "
                                 + "Завершает все потоки\n "
                                 + "OpenSCManager устанавливает связь с диспетчером управления службами\n";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");
                    if ( (bool)produces[3] )
                    {

                        if (0.2 < p_1)
                        {
                            prArr[idx] = 0.2;
                            comment[idx] = "OpenSCManager устанавливает связь с диспетчером управления службами\n";
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    idx = prArr.Add(0.0);
                    comment.Add("");
                    if ((bool)produces[7])
                    {

                        if (0.1 < p_1)
                        {
                            prArr[idx] = 0.1;
                        }
                        else
                        {
                            prArr[idx] = p_1;
                        }

                    }

                    double o = 0;
                    int y = 0;
                    int u = 0;


                    for (; u < prArr.Count; u++)
                    {
                        double vsp_p = (double)prArr[u];
                        if (vsp_p > probability)
                        {
                            probability = (double)prArr[u];
                            comments = (String)comment[u];
                        }

                    }

                    if (probability < 0.5) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    if ((probability >= 0.5) && (probability < 0.85)) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Yellow; if (probability > 1) probability = 1;
                    if (probability >= 0.85) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                    dgvStatistic.Rows[i].Cells[0].Value = files[i];
                    if (probability == 0) dgvStatistic.Rows[i].Cells[2].Value = "Просто файл";
                    else dgvStatistic.Rows[i].Cells[2].Value = comments;

                    dgvStatistic.Rows[i].Cells[1].Value = probability;


                }
                else
                {
                    dgvStatistic.Rows[i].Cells[0].Value = files[i];
                    dgvStatistic.Rows[i].Cells[2].Value = "Не исполняемый файл";

                }
            }



            /*    for (int i = 0; i < files.Length; i++)
            {
                dgvStatistic.Rows.Add();
                string[] buf = new string[10];
                int pp = 0;
                double[] p = new double[10];

                //probability.Clear();
                //comments.Clear();
                probability = 0;
                comments = "";
                if (PEzag(files[i]) == true)
                {
                    produces.Clear();
                    foreach (var sign in signatures)
                    {
                        produces.Add(Find(sign, files[i]));
                    }

                    if ((bool)produces[7])
                    {
                        probability += 0.1;
                        comments +=  "Ищет различные окна\n";
                        if ((bool)produces[2])
                        {
                            probability = probability + 0.2;
                            comments = comments + "Ищет окна с подходящим названием\n";
                            if ((bool)produces[4] && (bool)produces[5])
                            {
                                probability = probability + 0.2;
                                comments = comments + "Завершает все потоки\n";
                            }
                            if ((bool)produces[1])
                            {
                                probability = probability + 0.4;
                                comments = comments + "Копирует себя в корневой каталог Windows с именем poserv.exe\n";
                            }
                            if ((bool)produces[8])
                            {
                                probability = probability + 0.05;
                                comments = comments + "Создает файлы\n";
                                if ((bool)produces[9])
                                {
                                    probability = probability + 0.2;
                                    comments = comments + "Удаляет файлы\n";
                                }
                            }
                            if ((bool)produces[10])
                            {
                                probability = probability + 0.05;
                                comments = comments + "GetLocalTime\n";
                            }
                            if ((bool)produces[3])
                            {
                                probability = probability + 0.1;
                                comments = comments + "OpenSCManager устанавливает связь с диспетчером управления службами\n";
                            }

                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if ((bool)produces[4] && (bool)produces[5])
                        {
                            probability = probability + 0.2;
                            comments = comments + "Завершает все потоки\n";
                        }
                        if ((bool)produces[1])
                        {
                            probability = probability + 0.4;
                            comments = comments + "Копирует себя в корневой каталог Windows с именем poserv.exe\n";
                        }
                        if ((bool)produces[8])
                        {
                            probability = probability + 0.05;
                            comments = comments + "Создает файлы\n";
                            if ((bool)produces[9])
                            {
                                probability = probability + 0.2;
                                comments = comments + "Удаляет файлы\n";
                            }
                        }
                        if ((bool)produces[10])
                        {
                            probability = probability + 0.05;
                            comments = comments + "GetLocalTime\n";
                        }
                        if ((bool)produces[3])
                        {
                            probability = probability + 0.1;
                            comments = comments + "OpenSCManager устанавливает связь с диспетчером управления службами\n";
                        }

                    }

                    if (probability < 0.5) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    if ((probability >= 0.5) && (probability < 0.85)) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Yellow; if (probability > 1) probability = 1;
                    if (probability >= 0.85) dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                    dgvStatistic.Rows[i].Cells[0].Value = files[i];
                    if (probability == 0) dgvStatistic.Rows[i].Cells[2].Value = "Просто файл";
                    else dgvStatistic.Rows[i].Cells[2].Value = comments;

                    dgvStatistic.Rows[i].Cells[1].Value = probability;
                    

                }
                else
                {
                    dgvStatistic.Rows[i].Cells[0].Value = files[i];
                    dgvStatistic.Rows[i].Cells[2].Value = "Не исполняемый файл";

                }

            }*/
        }

        private void bSignature_Click(object sender, EventArgs e)
        {
            if (files.Length == 0)
            {
                selectDir();
            }

            for (int i = dgvStatistic.Rows.Count - 1; i >= 0; i--)
            {
                dgvStatistic.Rows.Remove(dgvStatistic.Rows[i]);
            }

            for (int i = 0; i < files.Length; i++)
            {
                
                dgvStatistic.Rows.Add();
                string buf = "Вообще не PE EXE!";
                if (PEzag(files[i]) == true)
                {

                    bool flagSign = Find(signatures[1], files[i]);
                    if (flagSign)
                    {
                        buf = "Danger! Virus.Win32.Porex";
                        dgvStatistic.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else buf = "Исполняемый файл";

                }

                dgvStatistic.Rows[i].Cells[0].Value = files[i];
                dgvStatistic.Rows[i].Cells[2].Value = buf;
                //dgvStatistic.Rows[i].Cells[1].Value = p[y];
            }
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            for (int i = dgvStatistic.Rows.Count-1; i >= 0; i--)
            {
                dgvStatistic.Rows.Remove(dgvStatistic.Rows[i]);
            }

        }
    }

}