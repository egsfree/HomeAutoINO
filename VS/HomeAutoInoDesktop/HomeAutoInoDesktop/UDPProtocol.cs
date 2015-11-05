using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace HomeAutoInoDesktop
{


    public unsafe struct CmdData
    {
        public byte bData;
        public byte bPckgReady;
        public byte bNumData;
        public byte bRcvData;
        public byte bSession;
        public byte bCmd;
        public byte bDest;
        public byte bStatus;
        public fixed byte bRXData[128];
        public byte bCRC;
    };

    

    class UDPProtocol
    {

       /*
           HEADER1,
   HEADER2,
   SESSION,
   DEST,
   CMD1,
   STATUS,
   NUMDATA,
   DATA,
   CHECKSUM,
   END
        */
       byte[] bTestPkg = {  0x01,  // HEADER1
                            0x01, // HEADER2
                            0x00, // SESSION
                            0x0A, // DEST
                            0x00, // CMD1
                            0x00, // STATUS
                            0x05, // NUMDATA
                            0x01, // DATA1
                            0x02, // DATA2
                            0x03, // DATA3
                            0x04, // DATA4
                            0x05, // DATAN
                            0x00, // CHKSUM
                            0x04  // END
                          };

        UdpClient udpClient;


       

        public UDPProtocol()
        {

            udpClient = new UdpClient();



            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);

            /*
            int sockopt = (int)server.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
            
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
            sockopt = (int)server.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
           

            server.Close();
             * */
        }

        public void UDPSetAddressPort( string stIP, int iPort)
        {
            udpClient.Connect(stIP, iPort);
        }

        public void ShowMessageA( )
        {
           MessageBox.Show("Hello, World");
        }

        public unsafe byte SendRelayCommand(byte bRelay, byte bState)
        {
            byte[] bDataToSend = new byte[150];
            CmdData cmdPackage = new CmdData();
            int iRet = 0;

            cmdPackage.bCmd     = CommandPackgeDefs.CMD_RELAY_WRITE;
            cmdPackage.bDest    = 0x74;
            cmdPackage.bNumData = 2;
            cmdPackage.bStatus  = 0;
            cmdPackage.bSession = 0;
            cmdPackage.bRXData[0] = bRelay;
            cmdPackage.bRXData[1] = bState;

            iRet = ProcessPackageToSend(cmdPackage);   

            return 0;
        }


        public unsafe byte ReadRelayCommand(ref byte[] bRelayState)
        {
            byte bRet = 0xFF;

            byte[] bDataToSend = new byte[150];
            CmdData cmdPackage = new CmdData();
           

            cmdPackage.bCmd = CommandPackgeDefs.CMD_RELAY_READ;
            cmdPackage.bDest = 0x74;
            cmdPackage.bNumData = 1;
            cmdPackage.bStatus = 0;
            cmdPackage.bSession = 0;

            cmdPackage = ProcessPackageToRead(cmdPackage);

            if (cmdPackage.bCmd != 0xFF)
            {
                for (int i = 0; i < 8; i++ )
                {
                    bRelayState[i] = (byte)((cmdPackage.bRXData[0] >> i) & 0x01);
                }

                bRet = 0;
            }



            return bRet;
        }

       // public unsafe byte SendConfigEventCommand(byte bDia, byte bMes, byte bAno, byte bStartHora, byte bStartMinuto, byte bStartSegundo, byte bStopHora, byte bStopMinuto, byte bStopSegundo)
        public unsafe byte SendConfigEventCommand(DateTime StartDate, DateTime StopDate, int Type, int Relay, int Slot, byte WeekDays)
        {
            byte[] bDataToSend = new byte[150];
            CmdData cmdPackage = new CmdData();
            int iRet = 0;
                     
            cmdPackage.bCmd = CommandPackgeDefs.CMD_SET_EVENT;
            cmdPackage.bDest = 0x74;
            cmdPackage.bNumData = 13;
            cmdPackage.bStatus = 0;
            cmdPackage.bSession = 0;
            cmdPackage.bRXData[0] = (byte)Slot;
            cmdPackage.bRXData[1] = (byte)Type; //Type
            cmdPackage.bRXData[2] = (byte)Relay; //Relay
            cmdPackage.bRXData[3] = (byte)StartDate.Day;
            cmdPackage.bRXData[4] = (byte)StartDate.Month;
            cmdPackage.bRXData[5] = (byte)(StartDate.Year - 2000);
            cmdPackage.bRXData[6] = WeekDays;
            cmdPackage.bRXData[7] = (byte)StartDate.Hour;
            cmdPackage.bRXData[8] = (byte)StartDate.Minute;
            cmdPackage.bRXData[9] = (byte)StartDate.Second;
            cmdPackage.bRXData[10] = (byte)StopDate.Hour;
            cmdPackage.bRXData[11] = (byte)StopDate.Minute;
            cmdPackage.bRXData[12] = (byte)StopDate.Second;

            iRet = ProcessPackageToSend(cmdPackage);

            return 0;
        }

        unsafe int MakePackage ( CmdData CmdPackage, ref byte[] bData )
        {
            /*byte [] bData = new byte[128];*/
            int DataCount = 0;

            bData[0] = CommandPackgeDefs.HEADER;
            bData[1] = CommandPackgeDefs.HEADER;
            bData[2] = CmdPackage.bSession;
            bData[3] = CmdPackage.bCmd;
            bData[4] = CmdPackage.bDest;
            bData[5] = CmdPackage.bStatus;
            bData[6] = CmdPackage.bNumData;


            for (int i = 0; i < CmdPackage.bNumData; i++ )
            {
                DataCount = i+7;
                bData[DataCount] = CmdPackage.bRXData[i];
            }

            DataCount++;

            bData[DataCount++] = 0;

            bData[DataCount++] = CommandPackgeDefs.ENDTRANS;

            return DataCount;
                
        }

        int ProcessPackageToSend ( CmdData CmdPackage )
        {
            byte[] bData = new byte[256];
            int DataLen = 0;
            int DataSent = 0;
            int PackageOk = -1;

            DataLen = MakePackage(CmdPackage, ref bData);

            try 
	        {
               	DataSent = udpClient.Send(bData, DataLen);
	        }
	        catch (Exception)
	        {
	        	
	        	throw;
	        }

            

            if( DataSent == DataLen )
            {
                try 
	            {	        
	            	var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receivedResults = udpClient.Receive(ref remoteEndPoint);

                    if(receivedResults.Length > 7)
                    {
                        if( ( receivedResults[0] == CommandPackgeDefs.HEADER) && ( receivedResults[1] == CommandPackgeDefs.HEADER)  && ( receivedResults[receivedResults.Length-1] == CommandPackgeDefs.ENDTRANS) )
                        {
                            PackageOk = 0;
                        }
                    }

	            }
	            catch (Exception)
	            {
	            	
	            	
	            }
              
            }

            return PackageOk;         


        }

        unsafe CmdData ProcessPackageToRead(CmdData CmdPackage)
        {
            byte[] bData = new byte[256];
            int DataLen = 0;
            int DataSent = 0;
            int PackageOk = -1;

            DataLen = MakePackage(CmdPackage, ref bData);

            try
            {
                DataSent = udpClient.Send(bData, DataLen);
            }
            catch (Exception)
            {

                throw;
            }

            CmdPackage.bCmd = 0xFF;

            if (DataSent == DataLen)
            {
                try
                {
                    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receivedResults = udpClient.Receive(ref remoteEndPoint);

                    if (receivedResults.Length > 7)
                    {
                        if ((receivedResults[0] == CommandPackgeDefs.HEADER) && (receivedResults[1] == CommandPackgeDefs.HEADER) && (receivedResults[receivedResults.Length - 1] == CommandPackgeDefs.ENDTRANS))
                        {
                                                        
                            CmdPackage.bCmd      = receivedResults[3];
                            CmdPackage.bDest     = receivedResults[4];
                            CmdPackage.bNumData  = receivedResults[6];
                            CmdPackage.bStatus   = receivedResults[5];
                            CmdPackage.bSession  = receivedResults[2];

                            for (int i = 0; i < CmdPackage.bNumData; i++ )
                            {
                                CmdPackage.bRXData[i] = receivedResults[i + 7];
                            }  

                            PackageOk = 0;
                        }
                    }

                }
                catch (Exception)
                {


                }

            }

            return CmdPackage;


        }

    }
   

}
