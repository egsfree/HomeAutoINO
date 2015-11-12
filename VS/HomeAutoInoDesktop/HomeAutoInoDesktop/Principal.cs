using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HomeAutoInoDesktop
{
    public partial class Principal : Form
    {

        UDPProtocol UDPProt;

        public string IPAddr;
        public string Port;


        public Principal()
        {
            InitializeComponent();
            UDPProt = new UDPProtocol();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            panelRelay1.Enabled = false;
            panelRelay2.Enabled = false;
            panelRelay3.Enabled = false;
            panelRelay4.Enabled = false;
            panelRelay5.Enabled = false;
            panelRelay6.Enabled = false;
            panelRelay7.Enabled = false;
            panelRelay8.Enabled = false;  
            
        }

       
 

        private void buttonConnOk_Click(object sender, EventArgs e)
        {
            UDPProt.UDPSetAddressPort(textBoxIP.Text, Convert.ToInt32(textBoxPort.Text));
            IPAddr = textBoxIP.Text;
            Port = textBoxPort.Text;
            
            RefreshRelayState();

            panelRelay1.Enabled = true;
            panelRelay2.Enabled = true;
            panelRelay3.Enabled = true;
            panelRelay4.Enabled = true;
            panelRelay5.Enabled = true;
            panelRelay6.Enabled = true;
            panelRelay7.Enabled = true;
            panelRelay8.Enabled = true;  

        }


        private void RelayONClick(object sender, EventArgs e)
        {
            int iTag = Convert.ToInt32(((Button)sender).Tag);            

            UDPProt.SendRelayCommand((byte)iTag, 1);
            RefreshRelayState();
        }

        private void RelayOFFClick(object sender, EventArgs e)
        {
            int iTag = Convert.ToInt32(((Button)sender).Tag);  
            UDPProt.SendRelayCommand((byte)iTag, 0);
            RefreshRelayState();
        }

        private void CfgEvent(object sender, EventArgs e)
        {
            CfgEventForm FormCfg = new CfgEventForm();

            FormCfg.RelayFrom = Convert.ToInt32(((Button)sender).Tag) - 1;

            FormCfg.IPAddr = textBoxIP.Text;
            FormCfg.Port = textBoxPort.Text;

            FormCfg.ShowDialog();

          //  DateTime Start = new DateTime(2015, 9, 15, 23, 05, 0);
           // DateTime Stop  = new DateTime(2015, 9, 15, 23, 06, 0);

          //  UDPProt.SendConfigEventCommand(Start, Stop);
        }

        private void RefreshRelayState ()
        {
            byte[] bRelayState = new byte[8];
            byte UdpRet = UDPProt.ReadRelayCommand(ref bRelayState);

            if (UdpRet == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    bool Codition = Convert.ToBoolean(bRelayState[i]);
                    switch (i + 1)
                    {
                        case 1:
                            buttonR1On.Enabled = !Codition;
                            buttonR1Off.Enabled = Codition;
                            break;
                        case 2:
                            buttonR2On.Enabled = !Codition;
                            buttonR2Off.Enabled = Codition;
                            break;
                        case 3:
                            buttonR3On.Enabled = !Codition;
                            buttonR3Off.Enabled = Codition;
                            break;
                        case 4:
                            buttonR4On.Enabled = !Codition;
                            buttonR4Off.Enabled = Codition;
                            break;
                        case 5:
                            buttonR5On.Enabled = !Codition;
                            buttonR5Off.Enabled = Codition;
                            break;
                        case 6:
                            buttonR6On.Enabled = !Codition;
                            buttonR6Off.Enabled = Codition;
                            break;
                        case 7:
                            buttonR7On.Enabled = !Codition;
                            buttonR7Off.Enabled = Codition;
                            break;
                        case 8:
                            buttonR8On.Enabled = !Codition;
                            buttonR8Off.Enabled = Codition;
                            break;
                        default:

                            break;
                    }
                }

            }
        }

        private void buttonSysInfo_Click(object sender, EventArgs e)
        {
            CmdData SerialData = new CmdData();
            byte[] bData = new byte[128];
            byte[] bRData = new byte[128];

            SerialData.bCmd = CommandPackgeDefs.CMD_GET_IP;
            SerialData.bNumData = 1;

            int numdata = UDPProt.MakePackage(SerialData, ref bData);

            try
            {
                Serial.PortName = comboBoxPorts.Text;
                Serial.Open();               

                Serial.Write(bData, 0, numdata);
                System.Threading.Thread.Sleep(100);
                Serial.Read(bRData, 0, 13);
                Serial.Close();

                textBoxIP.Text = Convert.ToString(bRData[7]) + "." + Convert.ToString(bRData[8]) + "." + Convert.ToString(bRData[9]) + "." + Convert.ToString(bRData[10]);
                
            }
            catch (Exception)
            {
                Serial.Close();
                textBoxIP.Text = "Erro.";
            }


            

        }

        private void buttonListPorts_Click(object sender, EventArgs e)
        {
            var ports = System.IO.Ports.SerialPort.GetPortNames();
            comboBoxPorts.DataSource = ports;
        }

        private void buttonGetDateTime_Click(object sender, EventArgs e)
        {
            int iDia = 0;
            int iMes = 0;
            int iAno = 0;

            int iHora = 0;
            int iMin = 0;
            int iSec = 0;

            byte bRet = UDPProt.GetDateTime(ref iDia, ref iMes, ref iAno, ref iHora, ref iMin, ref iSec);

            if(bRet == 0)
            {
                textBoxDateTime.Text = Convert.ToString(iDia) + "/" + Convert.ToString(iMes) + "/" + Convert.ToString(iAno) + "  -  " +
                    Convert.ToString(iHora) + ":" + Convert.ToString(iMin) + ":" + Convert.ToString(iSec);
            }
            else
            {
                textBoxDateTime.Text = "Erro.";
            }



        }

        private void buttonSyncDateTime_Click(object sender, EventArgs e)
        {
            int iDia = DateTime.Now.Day;
            int iMes = DateTime.Now.Month; 
            int iAno = DateTime.Now.Year;

            int iHora = DateTime.Now.Hour; 
            int iMin  = DateTime.Now.Minute;
            int iSec  = DateTime.Now.Second;

            UDPProt.SetDateTime(iDia, iMes, iAno, iHora, iMin, iSec);

        }
    }
}
