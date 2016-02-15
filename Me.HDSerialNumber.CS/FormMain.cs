using System;
using System.Windows.Forms;
using Me.Utils;
using Me.Utils.MySystem;
using System.Diagnostics;

namespace Me.HDSerialNumber.CS
{
    public partial class FormMain : Form
    {
        private int _indexSelected = 0;
        private Stopwatch _stopWatch;

        public FormMain()
        {
            InitializeComponent();
            _stopWatch = new Stopwatch();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            System.IO.DriveInfo[] allDrives = System.IO.DriveInfo.GetDrives();
            comboBoxDriveLetter.DataSource = allDrives;           
        }

        private void GetSerialNumberDriveInfo(int index = 0)
        {

            TextBoxDisque.Clear();

            this._stopWatch.Start();
            DriveInfo info = new DriveInfo(index);
            LogMethodExcecutionTime("GetSerialNumberDriveInfo");

            if ((info.SerialNumber != null))
            {
                TextBoxDisque.AppendText(info.SerialNumber.ToString() + "\r\n");
                TextBoxDisque.AppendText(info.Model.ToString() + "\r\n");
                TextBoxDisque.AppendText(info.DriveType.ToString() + "\r\n");
                TextBoxDisque.AppendText(info.Firmware.ToString() + "\r\n");
                return;
            }
            TextBoxDisque.AppendText("No serial number");

        }

        private void ButtonDriveInfo_Click(object sender, EventArgs e)
        {
            TextBoxDisque.Clear();
            GetSerialNumberDriveInfo(this._indexSelected);
        }


        private void ButtonWmi_Click(object sender, EventArgs e)
        {
            TextBoxDisque.Clear();

            this._stopWatch.Start();
            MyWmi.WmiDonneesDisque dd = MyWmi.GetSerialNumberWmi(this._indexSelected);
            LogMethodExcecutionTime("GetSerialNumberWmi");

            if (dd.NumeroSerie != "")
            {
                TextBoxDisque.AppendText(dd.NumeroSerie + "\r\n");
                TextBoxDisque.AppendText(dd.Modele + "\r\n");
                TextBoxDisque.AppendText(dd.Type + "\r\n");
                TextBoxDisque.AppendText(dd.DeviceID + "\r\n");
                return;
            }
            TextBoxDisque.AppendText("No serial number");

        }

        private void comboBoxDriveLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxDisque.Clear();
            ComboBox myCombo = (ComboBox)sender;
            _indexSelected = myCombo.SelectedIndex;
        }

         private void LogMethodExcecutionTime(string methodName)
        {
            TextBoxPerf.Clear();
            this._stopWatch.Stop();
            TextBoxPerf.Text = string.Format("Method name: {0} time elapsed  = {1} ms ticks = {2}", methodName, this._stopWatch.ElapsedMilliseconds.ToString(), this._stopWatch.ElapsedTicks.ToString());
            this._stopWatch.Reset();
        }
   }
}
