package egs.homeautoino.com.homeautoinodroid;

/**
 * Created by Eduardo on 05/10/2015.
 */
public class CommandDefs {
    public byte bData;
    public byte bPckgReady;
    public byte bNumData;
    public byte bRcvData;
    public byte bSession;
    public byte bCmd;
    public byte bDest;
    public byte bStatus;
    public byte bRXData[] = new byte[128];
    public byte bCRC;

    public int MakePackage ( CommandDefs CmdPackage, byte bData[] )
    {
        CommandList CmdList = new CommandList();    /*byte [] bData = new byte[128];*/
        int DataCount = 0;

        bData[0] = CmdList.HEADER;
        bData[1] = CmdList.HEADER;
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

        bData[DataCount++] = CmdList.ENDTRANS;

        return DataCount;

    }




}