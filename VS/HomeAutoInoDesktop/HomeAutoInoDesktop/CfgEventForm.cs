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
        public bool boReady = false;

        UDPProtocol Protocol = new UDPProtocol();

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
            if (radioButtonDaily.Checked)
            {
                groupBoxDate.Enabled = false;
                groupBoxWeek.Enabled = false;
                groupBoxStartTime.Enabled = true;
                groupBoxStopTime.Enabled = true;
            }
            else
            if (radioButtonOneTime.Checked)
            {
                groupBoxDate.Enabled = true;
                groupBoxWeek.Enabled = false;
                groupBoxStartTime.Enabled = true;
                groupBoxStopTime.Enabled = true;
            }
            else
            if (radioButtonWeekly.Checked)
            {
                groupBoxDate.Enabled = false;
                groupBoxWeek.Enabled = true;
                groupBoxStartTime.Enabled = true;
                groupBoxStopTime.Enabled = true;
            }
            else
            {
                groupBoxDate.Enabled = false;
                groupBoxWeek.Enabled = false;
                groupBoxStartTime.Enabled = false;
                groupBoxStopTime.Enabled = false;

            }
        }

        private void ConfigEvent(object sender, EventArgs e)
        {
            int EventType = 0;

            byte WeekDays = 0;

           // UDPProtocol Protocol = new UDPProtocol();
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
            if(radioButtonWeekly.Checked)            
            {
                EventType = 3;
            }
            else
            {
                EventType = 0;
            }


            switch(EventType)
            {
                case 0:
                    Protocol.SendConfigEventCommand(Start, Stop, EventType, comboBoxRelay.SelectedIndex + 1, comboBoxEventSlot.SelectedIndex + 1, WeekDays);
                    break;

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

        private void comboBoxEventSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boReady)
            {
                RefreshEventSlot(comboBoxEventSlot.SelectedIndex + 1);
            }
        }

        private void CfgEventForm_Load(object sender, EventArgs e)
        {

        }

        private void CfgEventForm_Shown(object sender, EventArgs e)
        {
            boReady = true;

              RefreshEventSlot(1);

        }

        public void RefreshEventSlot(int Slot )
        {
            DateTime StartTime = new DateTime();
            DateTime StopTime = new DateTime();
            int iType = 0;
            int iRelay = 0;
            byte WeekDays = 0;            
            int iStartDay = 0;
            int iStartMonth = 0;
            int iStartYear = 0;
            int iStartHour = 0;
            int iStartMinute = 0;
            int iStartSecond = 0;
            int iStopHour = 0;
            int iStopMinute = 0;
            int iStopSecond = 0;

            Protocol.UDPSetAddressPort(IPAddr, Convert.ToInt32(Port));

            byte bStatus = Protocol.SendReadEventCommand(Slot, ref iStartDay, ref iStartMonth, ref iStartYear, ref iStartHour, ref iStartMinute, ref iStartSecond, ref iStopHour, ref iStopMinute, ref iStopSecond, ref iType, ref iRelay, ref WeekDays);


            switch(iType)
            {
                case 0:
                    radioButtonDesativado.Checked = true;

                    StartHour.Value   = 0;
                    StartMinute.Value = 0;
                    StartSecond.Value = 0;

                    StopHour.Value   = 0;
                    StopMinute.Value = 0;
                    StopSecond.Value = 0;


                    break;

                case 1:
                    radioButtonOneTime.Checked = true;

                    StartHour.Value = Convert.ToDecimal(iStartHour);
                    StartMinute.Value = Convert.ToDecimal(iStartMinute);
                    StartSecond.Value = Convert.ToDecimal(iStartSecond);

                    StopHour.Value = Convert.ToDecimal  (iStopHour);
                    StopMinute.Value = Convert.ToDecimal(iStopMinute);
                    StopSecond.Value = Convert.ToDecimal(iStopSecond);


                    break;

                case 2:
                    radioButtonDaily.Checked = true;

                    StartHour.Value = Convert.ToDecimal(iStartHour);
                    StartMinute.Value = Convert.ToDecimal(iStartMinute);
                    StartSecond.Value = Convert.ToDecimal(iStartSecond);

                    StopHour.Value = Convert.ToDecimal(iStopHour);
                    StopMinute.Value = Convert.ToDecimal(iStopMinute);
                    StopSecond.Value = Convert.ToDecimal(iStopSecond);

                    break;

                case 3:
                    radioButtonWeekly.Checked = true;

                    StartHour.Value = Convert.ToDecimal(iStartHour);
                    StartMinute.Value = Convert.ToDecimal(iStartMinute);
                    StartSecond.Value = Convert.ToDecimal(iStartSecond);

                    StopHour.Value = Convert.ToDecimal(iStopHour);
                    StopMinute.Value = Convert.ToDecimal(iStopMinute);
                    StopSecond.Value = Convert.ToDecimal(iStopSecond);

                    break;

                default:
                    break;
                    
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
