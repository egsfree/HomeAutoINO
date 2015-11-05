package egs.homeautoino.com.homeautoinodroid;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.StrictMode;
import android.preference.PreferenceManager;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ToggleButton;


import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.sql.Struct;
import java.util.ArrayList;
import java.util.Iterator;

public class MainActivity extends AppCompatActivity {

    String IPAddr = "com.homeautonio.android.ipaddr";
    String R1Str = "com.homeautonio.android.r1str";
    String R2Str = "com.homeautonio.android.r2str";
    String R3Str = "com.homeautonio.android.r3str";
    String R4Str = "com.homeautonio.android.r4str";
    String R5Str = "com.homeautonio.android.r5str";
    String R6Str = "com.homeautonio.android.r6str";
    String R7Str = "com.homeautonio.android.r7str";
    String R8Str = "com.homeautonio.android.r8str";

    String IPAddrStr = "com.homeautonio.android.ipaddr";

    boolean ButtonEnabled = false;

    volatile int iSemaphore = 0;

    Button ButtonEditLabels ;

    Button ButtonConnect;

    ToggleButton   ButtonR1   ;
    ToggleButton   ButtonR2   ;
    ToggleButton   ButtonR3   ;
    ToggleButton   ButtonR4   ;
    ToggleButton   ButtonR5   ;
    ToggleButton   ButtonR6   ;
    ToggleButton   ButtonR7   ;
    ToggleButton   ButtonR8   ;

    EditText EditR1Label;
    EditText EditR2Label;
    EditText EditR3Label;
    EditText EditR4Label;
    EditText EditR5Label;
    EditText EditR6Label;
    EditText EditR7Label;
    EditText EditR8Label;

    EditText EditIPAddr;

    private static final String host = null;
    private int port;
    String str=null;

    byte[] send_data = new byte[1024];
    byte[] receiveData = new byte[1024];

    DatagramSocket client_socket;

    int socketInit = 0;


    SharedPreferences app_preferences;
    SharedPreferences.Editor editor  ;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);


        // SendCommand(null);


        startGUI();
        FeedEditLabels();
        FeedIPAddr();
        SetEditLabelsEditable(false);
        SetButtonsEnabled(false);

        RefreshButtonsState();


/*
        ButtonR1.setChecked(true);

        if( ButtonR1.getText().equals("ON"))
        {
            ButtonR2.setChecked(true);
        }

*/
        ButtonConnect.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {


                RefreshButtonsState();
                CommitIPAddr();
            }
        });

        ButtonEditLabels.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {


                if (!ButtonEnabled) {
                    ButtonEnabled = true;
                    ClearButtons();
                    FeedEditLabels();
                    SetEditLabelsEditable(true);
                    ButtonEditLabels.setText("Pronto!");
                } else {
                    ButtonEnabled = false;
                    CommitNewValues();
                    SetEditLabelsEditable(false);
                    ButtonEditLabels.setText("Editar");
                }
            }
        });

        ButtonR1.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {

                if( ButtonR1.getText().equals("OFF")) // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(1, 0);

                }
                else // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(1, 1);
                }
                RefreshButtonsState();

            }
        });

        ButtonR2.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {

                if( ButtonR2.getText().equals("OFF")) // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(2, 0);

                }
                else // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(2, 1);
                }
                RefreshButtonsState();

            }
        });

        ButtonR3.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {

                if( ButtonR3.getText().equals("OFF")) // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(3, 0);

                }
                else // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(3, 1);
                }
                RefreshButtonsState();

            }
        });

        ButtonR4.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {

                if( ButtonR4.getText().equals("OFF")) // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(4, 0);

                }
                else // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(4, 1);
                }
                RefreshButtonsState();

            }
        });

        ButtonR5.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {

                if( ButtonR5.getText().equals("OFF")) // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(5, 0);

                }
                else // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(5, 1);
                }
                RefreshButtonsState();

            }
        });

        ButtonR6.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {

                if( ButtonR6.getText().equals("OFF")) // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(6, 0);

                }
                else // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(6, 1);
                }
                RefreshButtonsState();

            }
        });

        ButtonR7.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {

                if( ButtonR7.getText().equals("OFF")) // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(7, 0);

                }
                else // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(7, 1);
                }
                RefreshButtonsState();

            }
        });

        ButtonR8.setOnClickListener(new View.OnClickListener() {
            public void onClick(View arg0) {

                if( ButtonR8.getText().equals("OFF")) // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(8, 0);

                }
                else // Se o estado que o bot�o vai � OFF
                {
                    SendRelayCommand(8, 1);
                }
                RefreshButtonsState();
            }
        });

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    public void startGUI()
    {
        ButtonR1   = (ToggleButton)findViewById(R.id.buttonR1);
        ButtonR2   = (ToggleButton)findViewById(R.id.buttonR2);
        ButtonR3   = (ToggleButton)findViewById(R.id.buttonR3);
        ButtonR4   = (ToggleButton)findViewById(R.id.buttonR4);
        ButtonR5   = (ToggleButton)findViewById(R.id.buttonR5);
        ButtonR6   = (ToggleButton)findViewById(R.id.buttonR6);
        ButtonR7   = (ToggleButton)findViewById(R.id.buttonR7);
        ButtonR8   = (ToggleButton)findViewById(R.id.buttonR8);

        ButtonConnect = (Button)findViewById(R.id.ConnectButton);

        ButtonEditLabels = (Button) findViewById(R.id.buttonChange);

        EditR1Label = (EditText) findViewById(R.id.editTextR1Label);
        EditR2Label = (EditText) findViewById(R.id.editTextR2Label);
        EditR3Label = (EditText) findViewById(R.id.editTextR3Label);
        EditR4Label = (EditText) findViewById(R.id.editTextR4Label);
        EditR5Label = (EditText) findViewById(R.id.editTextR5Label);
        EditR6Label = (EditText) findViewById(R.id.editTextR6Label);
        EditR7Label = (EditText) findViewById(R.id.editTextR7Label);
        EditR8Label = (EditText) findViewById(R.id.editTextR8Label);

        EditIPAddr = (EditText) findViewById(R.id.ipEdit);





        app_preferences = PreferenceManager.getDefaultSharedPreferences(this);
        editor = app_preferences.edit();
    }

    public void CommitIPAddr()
    {
        editor.putString(IPAddrStr, EditIPAddr.getText().toString());
        editor.commit();
    }

    public void FeedIPAddr()
    {
        EditIPAddr.setText(app_preferences.getString(IPAddrStr, "192.168.0.1"));
    }

    public void FeedEditLabels (  )
    {
        EditR1Label.setText(app_preferences.getString(R1Str, "Rele 1"));
        EditR2Label.setText(app_preferences.getString(R2Str, "Rele 2"));
        EditR3Label.setText(app_preferences.getString(R3Str, "Rele 3"));
        EditR4Label.setText(app_preferences.getString(R4Str, "Rele 4"));
        EditR5Label.setText(app_preferences.getString(R5Str, "Rele 5"));
        EditR6Label.setText(app_preferences.getString(R6Str, "Rele 6"));
        EditR7Label.setText(app_preferences.getString(R7Str, "Rele 7"));
        EditR8Label.setText(app_preferences.getString(R7Str, "Rele 7"));
    }

    public void SetEditLabelsEditable ( boolean Editable )
    {
        EditR1Label.setEnabled(Editable);
        EditR2Label.setEnabled(Editable);
        EditR3Label.setEnabled(Editable);
        EditR4Label.setEnabled(Editable);
        EditR5Label.setEnabled(Editable);
        EditR6Label.setEnabled(Editable);
        EditR7Label.setEnabled(Editable);
        EditR8Label.setEnabled(Editable);
    }

    public void ClearButtons (  )
    {
        EditR1Label.setText("");
        EditR2Label.setText("");
        EditR3Label.setText("");
        EditR4Label.setText("");
        EditR5Label.setText("");
        EditR6Label.setText("");
        EditR7Label.setText("");
        EditR8Label.setText("");
    }

    public void SetButtonsEnabled ( boolean Enabled  )
    {
        ButtonR1.setEnabled(Enabled);
        ButtonR2.setEnabled(Enabled);
        ButtonR3.setEnabled(Enabled);
        ButtonR4.setEnabled(Enabled);
        ButtonR5.setEnabled(Enabled);
        ButtonR6.setEnabled(Enabled);
        ButtonR7.setEnabled(Enabled);
        ButtonR8.setEnabled(Enabled);
        ButtonR1.setClickable(Enabled);
        ButtonR2.setClickable(Enabled);
        ButtonR3.setClickable(Enabled);
        ButtonR4.setClickable(Enabled);
        ButtonR5.setClickable(Enabled);
        ButtonR6.setClickable(Enabled);
        ButtonR7.setClickable(Enabled);
        ButtonR8.setClickable(Enabled);
    }

    public void RefreshButtonsState (  )
    {
        byte[] relaysState = new byte[8];
        if( ReadRelayCommand(relaysState) != 0xFF) {

            SetButtonsEnabled(true);

            for (int i = 1; i < 9; i++) {
                boolean Activate = false;
                if (relaysState[i - 1] == 0) {
                    Activate = false;
                } else {
                    Activate = true;
                }

                switch (i) {
                    case 1:
                        ButtonR1.setChecked(Activate);
                        break;
                    case 2:
                        ButtonR2.setChecked(Activate);
                        break;
                    case 3:
                        ButtonR3.setChecked(Activate);
                        break;
                    case 4:
                        ButtonR4.setChecked(Activate);
                        break;
                    case 5:
                        ButtonR5.setChecked(Activate);
                        break;
                    case 6:
                        ButtonR6.setChecked(Activate);
                        break;
                    case 7:
                        ButtonR7.setChecked(Activate);
                        break;
                    case 8:
                        ButtonR8.setChecked(Activate);
                        break;
                }

            }
        }
        else
        {
            SetButtonsEnabled(false);
        }
    }

    public void CommitNewValues (  )
    {
        editor.putString(R1Str, EditR1Label.getText().toString());
        editor.putString(R2Str, EditR2Label.getText().toString());
        editor.putString(R3Str, EditR3Label.getText().toString());
        editor.putString(R4Str, EditR4Label.getText().toString());
        editor.putString(R5Str, EditR5Label.getText().toString());
        editor.putString(R6Str, EditR6Label.getText().toString());
        editor.putString(R7Str, EditR7Label.getText().toString());
        editor.putString(R8Str, EditR8Label.getText().toString());
        editor.commit();


    }

    public void client( CommandDefs Command) throws IOException{



        byte bDataToSend[] = new byte[128];
        int iTryTimes = 0;

        if( socketInit == 0 ) {
            socketInit = 1;
            client_socket = new DatagramSocket(5000);
        }

        InetAddress IPAddress =  InetAddress.getByName(EditIPAddr.getText().toString());


        int DataLen = Command.MakePackage(Command, bDataToSend);

        DatagramPacket send_packet = new DatagramPacket(bDataToSend,DataLen, IPAddress, 5000);
        DatagramPacket receivePacket = new DatagramPacket(receiveData, receiveData.length);
        client_socket.setSoTimeout(350);

        client_socket.send(send_packet);

        client_socket.receive(receivePacket);






        if (receivePacket.getLength() > 7)
        {

            if ((receivePacket.getData()[0] == CommandList.HEADER) && (receivePacket.getData()[1] == CommandList.HEADER) && (receivePacket.getData()[receivePacket.getLength()- 1] == CommandList.ENDTRANS))
            {

                Command.bCmd      = receivePacket.getData()[3];
                Command.bDest     = receivePacket.getData()[4];
                Command.bNumData  = receivePacket.getData()[6];
                Command.bStatus   = receivePacket.getData()[5];
                Command.bSession  = receivePacket.getData()[2];

                for (int i = 0; i < Command.bNumData; i++ )
                {
                    Command.bRXData[i] = receivePacket.getData()[i + 7];
                }

            }
        }
        else
        {
            Command.bNumData = 0;
        }

       // client_socket.close();



    }

    public int SendRelayCommand(int bRelay, int bState)
    {
        byte[] bDataToSend = new byte[150];
        int bRet = 0xff;

        CommandDefs CmdPackage = new CommandDefs();
        int iRet = 0;

        CmdPackage.bCmd     = CommandList.CMD_RELAY_WRITE;
        CmdPackage.bDest    = 0x74;
        CmdPackage.bNumData = 2;
        CmdPackage.bStatus  = 0;
        CmdPackage.bSession = 0;
        CmdPackage.bRXData[0] = (byte)bRelay;
        CmdPackage.bRXData[1] = (byte)bState;

        if( SendCommand(CmdPackage) != 0)
        {
            bRet = 0;
        }




        // iRet = ProcessPackageToSend(cmdPackage);

        return 0;
    }
    public int ReadRelayCommand( byte[] bRelayState)
    {
        char bRet = 0xFF;


        byte[] bDataToSend = new byte[150];
        CommandDefs cmdPackage = new CommandDefs();


        cmdPackage.bCmd = CommandList.CMD_RELAY_READ;
        cmdPackage.bDest = 0x74;
        cmdPackage.bNumData = 1;
        cmdPackage.bStatus = 0;
        cmdPackage.bSession = 0;

        if( SendCommand(cmdPackage) != 0)
        {
            bRet = 0;
        }

        if (cmdPackage.bCmd != 0xFF)
        {
            for (int i = 0; i < 8; i++ )
            {
                bRelayState[i] = (byte)((cmdPackage.bRXData[0] >> i) & 0x01);
            }


        }



        return bRet;
    }

    public int ReceiveCommand( byte Command[] )
    {
        Command[0] = 1;
        return 0;
    }

    public void GiveSemaphore()
    {
        iSemaphore = 0;
    }

    public void TakeSemaphore()
    {
        while(iSemaphore == 1);

        iSemaphore = 1;
    }
    public int SendCommand(CommandDefs Command)
    {
        int iTryTimes = 0;
        int iCmdOk = 0;

        do {

            try {
                client(Command);
                iCmdOk = 1;
                //txt1.setText(modifiedSentence);
            } catch (IOException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }

        }while ((iCmdOk == 0) && ((iTryTimes++) < 3));

         return  iCmdOk;
    }

}
