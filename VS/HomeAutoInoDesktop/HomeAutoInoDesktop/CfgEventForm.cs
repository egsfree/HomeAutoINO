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
    public partial class CfgEventForm : Form
    {
        public int RelayFrom = 0;

        public string IPAddr;
        public string Port;

        public CfgEventForm()
        {
            InitializeComponent();
            comboBoxEventSlot.SelectedIndex = 0;

        }

        private void ConfigEventsButton(object sender, EventArgs e)
        {
   

            
        }

        private void ChangeStartHour(object sender, EventArgs e)
        {
               StartHour.Maximum = StopHour.Value;
        }

        private void SetEventType(object sender, EventArgs e)
        {
            if(radioButtonDaily.Checked)
            {
                groupBoxDate.Enabled = false;
                groupBoxWeek.Enabled = false;
            }
            else
            if(radioButtonOneTime.Checked)
            {
                groupBoxDate.Enabled = true;
                groupBoxWeek.Enabled = false;
            }
            else
            {
                groupBoxDate.Enabled = false;
                groupBoxWeek.Enabled = true;
            }
        }

        private void ConfigEvent(object sender, EventArgs e)
        {
            int EventType = 0;

            byte WeekDays = 0;

            UDPProtocol Protocol = new UDPProtocol();
            Principal PrincipalLink = new Principal();         

            Protocol.UDPSetAddressPort(IPAddr, Convert.ToInt32(Port));

            DateTime Start = new DateTime(EventDate.Value.Year, EventDate.Value.Month, EventDate.Value.Day, (int)StartHour.Value, (int)StartMinute.Value, (int)StartSecond.Value);
            DateTime Stop = new DateTime (EventDate.Value.Year, EventDate.Value.Month, EventDate.Value.Day, (int)StopHour.Value, (int)StopMinute.Value, (int)StopSecond.Value);   

            if(radioButtonOneTime.Checked)
            {
                EventType = 1;
            }
            else
            if(radioButtonDaily.Checked)
            {
                EventType = 2;
            }
            else
            {
                EventType = 3;
            }


            switch(EventType)
            {
                case 1:
                    Protocol.SendConfigEventCommand(Start, Stop, EventType, comboBoxRelay.SelectedIndex + 1, comboBoxEventSlot.SelectedIndex + 1, WeekDays);
                    break;
                case 2:
                    Protocol.SendConfigEventCommand(Start, Stop, EventType, comboBoxRelay.SelectedIndex + 1, comboBoxEventSlot.SelectedIndex + 1, WeekDays);
                    break;
                case 3:
                    WeekDays |= checkBoxDomingo.Checked ? (byte)0x01 : (byte)0x00; /*Domingo*/
                    WeekDays |= checkBoxSegunda.Checked ? (byte)0x02 : (byte)0x00;
                    WeekDays |= checkBoxTerca.Checked   ? (byte)0x04 : (byte)0x00;
                    WeekDays |= checkBoxQuarta.Checked  ? (byte)0x08 : (byte)0x00;
                    WeekDays |= checkBoxQuinta.Checked  ? (byte)0x10 : (byte)0x00;
                    WeekDays |= checkBoxSexta.Checked   ? (byte)0x20 : (byte)0x00;
                    WeekDays |= checkBoxSabado.Checked  ? (byte)0x40 : (byte)0x00;

                    Protocol.SendConfigEventCommand(Start, Stop, EventType, comboBoxRelay.SelectedIndex + 1, comboBoxEventSlot.SelectedIndex + 1, WeekDays);

                    break;
                default:
                    break;
            }
        }

        private void CfgEventForm_Activated(object sender, EventArgs e)
        {
            comboBoxRelay.SelectedIndex = RelayFrom;
            
        }

        private void comboBoxRelay_SelectedIndexChanged(object sender, EventArgs e)
        {
            RelayFrom = comboBoxRelay.SelectedIndex;
        }
    }
}
