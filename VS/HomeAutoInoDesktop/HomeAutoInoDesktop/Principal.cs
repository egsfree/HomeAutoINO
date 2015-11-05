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

 
    }
}
