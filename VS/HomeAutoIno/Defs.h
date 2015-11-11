#ifndef DEFS_H
#define DEFS_H

#define RELAY_CHANNELS_8

#define BYTE unsigned char
#define WORD unsigned short
#define STARTHEADING 0x01U
#define ENDTRANSMISSION 0x04U

#define SEND_ERROR 0xFAU

#define NULL 0

#if RELAY_CHANNELS_4

#define RELAY1_OUT 2
#define RELAY2_OUT 3
#define RELAY3_OUT 4
#define RELAY4_OUT 5

#endif

#ifdef RELAY_CHANNELS_8

#define RELAY1_OUT 2
#define RELAY2_OUT 3
#define RELAY3_OUT 4
#define RELAY4_OUT 5
#define RELAY5_OUT 6
#define RELAY6_OUT 7
#define RELAY7_OUT 8
#define RELAY8_OUT 9

#endif

#ifdef RELAY_CHANNELS_16

#define RELAY1_OUT 2
#define RELAY2_OUT 3
#define RELAY3_OUT 4
#define RELAY4_OUT 5
#define RELAY5_OUT 6
#define RELAY6_OUT 7
#define RELAY7_OUT 8
#define RELAY8_OUT 9

#define RELAY9_OUT 31
#define RELAY10_OUT 33
#define RELAY11_OUT 35
#define RELAY12_OUT 37
#define RELAY13_OUT 39
#define RELAY14_OUT 41
#define RELAY15_OUT 43
#define RELAY16_OUT 45

#endif





enum STATES
{
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
}eSTATES;

enum TYPECOM
{
   TSERIAL,
   TETHERNET
}eTYPECOM;

typedef struct
{
	BYTE bData;
	BYTE bPckgReady;
	BYTE bNumData;
	BYTE bRcvData;
	BYTE bSession;
	BYTE bCmd;
	BYTE bDest;
	BYTE bStatus;
	BYTE bRXData[60];
	BYTE bCRC;
	TYPECOM FromCom;
}sPackage;

#endif
