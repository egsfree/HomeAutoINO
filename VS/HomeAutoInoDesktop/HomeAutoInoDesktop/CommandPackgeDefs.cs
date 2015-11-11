using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAutoInoDesktop
{
    static class CommandPackgeDefs
    {
        public const byte HEADER   = 0x01;
        public const byte ENDTRANS = 0x04;

        public const byte CMD_RELAY_WRITE    = 0x01;
        public const byte CMD_RELAY_READ     = 0x02;
        public const byte CMD_TIME_WRITE     = 0x03;
        public const byte CMD_TIME_READ      = 0x04;
        public const byte CMD_RLY_TMP_WRITE  = 0x05;
        public const byte CMD_RLY_TMP_READ   = 0x06;

        public const byte CMD_ANLG_WRITE     = 0x07;
        public const byte CMD_ANLG_READ      = 0x08;

        public const byte CMD_DIG_WRITE      = 0x09;
        public const byte CMD_DIG_READ       = 0x0A;

        public const byte CMD_SET_EVENT      = 0x0B;
        public const byte CMD_GET_EVENT      = 0x0C;

        public const byte CMD_GET_IP        = 0x0D;
        public const byte CMD_SET_IP        = 0x0E;



    }
}
