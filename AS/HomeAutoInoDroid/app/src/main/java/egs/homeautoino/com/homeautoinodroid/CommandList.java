package egs.homeautoino.com.homeautoinodroid;

/**
 * Created by Eduardo on 05/10/2015.
 */
public class CommandList {

    protected static byte HEADER   = 0x01;
    protected static byte ENDTRANS = 0x04;

    protected static byte CMD_RELAY_WRITE    = 0x01;
    protected static byte CMD_RELAY_READ     = 0x02;
    protected static byte CMD_TIME_WRITE     = 0x03;
    protected static byte CMD_TIME_READ      = 0x04;
    protected static byte CMD_RLY_TMP_WRITE  = 0x05;
    protected static byte CMD_RLY_TMP_READ   = 0x06;

    protected static byte CMD_ANLG_WRITE     = 0x07;
    protected static byte CMD_ANLG_READ      = 0x08;

    protected static byte CMD_DIG_WRITE      = 0x09;
    protected static byte CMD_DIG_READ       = 0x0A;

    protected static byte CMD_SET_EVENT      = 0x0B;
    protected static byte CMD_GET_EVENT      = 0x0C;;



}
