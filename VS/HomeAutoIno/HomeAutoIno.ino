/*
* HomeAutoIno
*
* Solu��o completa para Automa��o Residencial
* 
*
* Proibida a distribui��o sem a autoriza��o do autor
*

* Copyright (C) 2015 Eduardo Gomes Silva (egsfree@gmail.com)
*/

#include <UIPEthernet.h>
#include "Defs.h"
#include <string.h>
#include <Wire.h>
#include "RTClib.h"
#include "CommandsDefinitions.h"
#include "TimeEvents.h"
#include "EEPROM.h"
#include "E2PAddrs.h"
//#include <MemoryFree.h>
//RTC - Real Time Clock
RTC_DS1307 RTC;

#include <EEPROM.h>

//#define DEBUG_SERIAL
EthernetUDP udp;
unsigned long next;
sPackage * psRcvPackage;

#define LED_PIN 13

BYTE bLEDState = 0;

BYTE RELAY_OFFSET = 1;

void ProcessEvents(BYTE EventToCheck);
void ProcessRelays( void );


void setup() {
	
	Serial.begin(57600);
	Wire.begin();//Inicializacao do protocolo wire
	RTC.begin();//Inicializacao do modulo RTC

	if (!RTC.isrunning()) {
		Serial.println("RTC is NOT running!");
	}
	else
	{		
		DateTime now = RTC.now();//Recuperando a data e hora atual
		Serial.print(now.day(), DEC);//Imprimindo o dia
		Serial.print('/');
		Serial.print(now.month(), DEC);//Recuperando o mes
		Serial.print('/');
		Serial.print(now.year(), DEC);//Recuperando o ano
		Serial.print(' ');
		Serial.print(now.hour(), DEC);//Recuperando a hora
		Serial.print(':');
		Serial.print(now.minute(), DEC);//Reci[erando os minutos
		Serial.print(':');
		Serial.print(now.second(), DEC);//Recuperando os segundos
		Serial.println();
		
		
	}
	
	
    pinMode(RELAY1_OUT, OUTPUT);
	pinMode(RELAY2_OUT, OUTPUT);
	pinMode(RELAY3_OUT, OUTPUT);
	pinMode(RELAY4_OUT, OUTPUT);
	pinMode(RELAY5_OUT, OUTPUT);
	pinMode(RELAY6_OUT, OUTPUT);
	pinMode(RELAY7_OUT, OUTPUT);
	pinMode(RELAY8_OUT, OUTPUT);

	digitalWrite(RELAY1_OUT, HIGH);
	digitalWrite(RELAY2_OUT, HIGH);
	digitalWrite(RELAY3_OUT, HIGH);
	digitalWrite(RELAY4_OUT, HIGH);
	digitalWrite(RELAY5_OUT, HIGH);
	digitalWrite(RELAY6_OUT, HIGH);
	digitalWrite(RELAY7_OUT, HIGH);
	digitalWrite(RELAY8_OUT, HIGH);
	


#ifdef DEBUG_SERIAL
	Serial.println("Initializing...");

	//Serial.print("freeMemory()=");
	//Serial.println(freeMemory());
#endif

	const uint8_t mac[6] = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };

	// Ethernet.begin(mac,IPAddress(192,168,1,190));
       Serial.println("aLIVE/!");

	Ethernet.begin(mac);

	int success = udp.begin(5000);

	psRcvPackage = NULL;
}

// TestAppforArduinoCode.cpp : Defines the entry point for the console application.
// 0x01 0x01 0x00 0x0A 0x00 0x00 0x05 0x01 0x02 0x03 0x04 0x05 0x00 0x04


BYTE ProcessPackage(sPackage * psRcvPackage, TYPECOM FromCOM);
BYTE SendPackage(sPackage * psRcvPackage, TYPECOM FromCOM);
BYTE MakePackage(sPackage * psRcvPackage, BYTE bBuffer[]);
size_t GetPkgSize(sPackage * psRcvPackage);


//BYTE bTestPkg[] = { STARTHEADING, STARTHEADING, 0x00, 0x0A, 0x00, 0x00, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, ENDTRANSMISSION };

void loop()
{
	static BYTE iCountSlots = 0;

	while (1)
	{
                 
		udp.begin(5000);

		ProcessRelays();
               
		if (iCountSlots < MAX_SLOTS)
		{
			ProcessEvents(iCountSlots++);
		}
		else
		{
			iCountSlots = 0U;
			ProcessEvents(iCountSlots++);
		}

		int size = udp.parsePacket();
		if (size > 0)
		{
#ifdef DEBUG_SERIAL
			Serial.println("UDP Data available");
#endif
			psRcvPackage = NULL;

			psRcvPackage = RecebePacote(TETHERNET);

			if (psRcvPackage == NULL)
			{
#ifdef DEBUG_SERIAL
				Serial.println("UDP Package error.");
#endif			
				udp.flush();


				continue;
			}

			ProcessPackage(psRcvPackage, TETHERNET);
			udp.flush();
			//udp.stop();
			//udp.begin(5000);
		}

		if (Serial.available() > 0)
		{
#ifdef DEBUG_SERIAL
			Serial.println("Serial Data available");
#endif
			psRcvPackage = NULL;
			psRcvPackage = NULL;

			psRcvPackage = RecebePacote(TSERIAL);

			if (psRcvPackage == NULL)
			{
#ifdef DEBUG_SERIAL
				Serial.println("Serial Package error.");
#endif          
				Serial.end();
				Serial.begin(57600);
				continue;
			}

			ProcessPackage(psRcvPackage, TSERIAL);
			Serial.flush();
		}

		udp.stop();

		

	}

}








sPackage * RecebePacote(TYPECOM FromCOM)
{
	sPackage sRcvPackage;



	BYTE bEstado = HEADER1;
	static BYTE bData = 0x00U;
	BYTE bPckgReady = 0x00U;
	BYTE bRcvData = 0x00U;


	memset((BYTE*)&sRcvPackage, 0, sizeof(sRcvPackage));

	while (1)
	{
		if (1)
		{
			switch (FromCOM)
			{
			case TSERIAL:

				//bData = Serial.read();
				Serial.readBytes((char *)&bData, (size_t)1);
				//Serial.print("Data: ");
				//Serial.println(bData, HEX);

				break;
			case TETHERNET:

				if (udp.available() > 0)
				{
					bData = udp.read();

				}
				else
				{
#ifdef DEBUG_SERIAL
					Serial.println("Read Error");
#endif
					psRcvPackage = NULL;
					return NULL;
				}

				break;
			default:
				return NULL;
				break;
			}



			switch (bEstado)
			{
			case HEADER1:
				if (bData == STARTHEADING)
				{
					bEstado = HEADER2;

				}
				else
				{
#ifdef DEBUG_SERIAL
					Serial.println("HEADER 1 Error");
#endif
					return NULL;
				}
				break;

			case HEADER2:
				if (bData == STARTHEADING)
				{
					bEstado = SESSION;
				}
				else
				{
#ifdef DEBUG_SERIAL
					Serial.println("HEADER 2 Error");
#endif
					bEstado = HEADER1;
					return NULL;
				}

				break;

			case SESSION:
				sRcvPackage.bSession = bData;
				bEstado = CMD1;
				break;

			case CMD1:
				sRcvPackage.bCmd = bData;
				bEstado = DEST;
				break;
			case DEST:
				sRcvPackage.bDest = bData;
				bEstado = STATUS;
				break;

			case STATUS:
				sRcvPackage.bStatus = bData;
				bEstado = NUMDATA;
				break;

			case NUMDATA:
				sRcvPackage.bNumData = bData;
				bEstado = DATA;
#ifdef DEBUG_SERIAL
				Serial.print("Data size: ");
				Serial.println(bData, DEC);

#endif
				break;

			case DATA:

				sRcvPackage.bRXData[bRcvData] = bData;
				bRcvData++;

				if (bRcvData == sRcvPackage.bNumData)
				{
					bEstado = CHECKSUM;
				}
				break;

			case CHECKSUM:

				sRcvPackage.bCRC = bData;

#ifdef DEBUG_SERIAL
				Serial.println("CRC Received");
#endif
				bEstado = END;
				break;

			case END:
				if (bData == ENDTRANSMISSION)
				{
					Serial.println("EOT Received");
					bPckgReady = 0x01U;
				}
				else
				{
					sRcvPackage.bData = 0x00U;
					sRcvPackage.bPckgReady = 0x00U;
					sRcvPackage.bNumData = 0x00U;
					sRcvPackage.bRcvData = 0x00U;
					sRcvPackage.bSession = 0x00U;
					sRcvPackage.bCmd = 0x00U;
					sRcvPackage.bStatus = 0x00U;

					bEstado = HEADER1;
#ifdef DEBUG_SERIAL
					Serial.print("End-Of-Data Error. End char: ");
					Serial.println(bData, HEX);

#endif
					return NULL;
				}
				break;

			default:
				bEstado = HEADER1;
				break;
			}
		}

		if (bPckgReady == 0x01)
		{
#ifdef DEBUG_SERIAL
			Serial.println("PackageReceived");
#endif
			return &sRcvPackage;

		}

	}
}

BYTE ProcessPackage(sPackage * psRcvPackage, TYPECOM FromCOM)
{
	BYTE bRet = 0;
	unsigned short usPort;

	if (psRcvPackage == NULL)
	{
		return (BYTE)-1;
	}


	switch (psRcvPackage->bCmd)
	{

	case CMD_CONFIG_ETHERNET:
#ifdef DEBUG_SERIAL
		Serial.println("Config Serial.");
#endif
		break;

	case CMD_CONFIG_DEST_IP:
#ifdef DEBUG_SERIAL
		Serial.println("Config UDP.");
#endif
		usPort = (psRcvPackage->bRXData[4] << 8) | (psRcvPackage->bRXData[5]);
		/*bRet = UDP.beginPacket(IPAddress(psRcvPackage->bRXData[0], psRcvPackage->bRXData[1], psRcvPackage->bRXData[2], psRcvPackage->bRXData[3]), usPort); */

		break;

	case CMD_RELAY_WRITE:
#ifdef DEBUG_SERIAL
		Serial.println("RELAY WRITE.");
#endif
		WriteRelay(psRcvPackage->bRXData[0], psRcvPackage->bRXData[1] & 0x01);
		//digitalWrite((BYTE)(+ RELAY_OFFSET), !());
		/*bRet = UDP.beginPacket(IPAddress(psRcvPackage->bRXData[0], psRcvPackage->bRXData[1], psRcvPackage->bRXData[2], psRcvPackage->bRXData[3]), usPort); */

		break;

	case CMD_SET_EVENT:
	{
		sEvent Event;

		Event.bEventSlot = psRcvPackage->bRXData[0];
		Event.bEventType = psRcvPackage->bRXData[1];
		Event.bRelay = psRcvPackage->bRXData[2];
		Event.bDay = psRcvPackage->bRXData[3];
		Event.bMounth = psRcvPackage->bRXData[4];
		Event.bYear = psRcvPackage->bRXData[5];
		Event.bWeekDays = psRcvPackage->bRXData[6];
		Event.bStartHour = psRcvPackage->bRXData[7];
		Event.bStartMinute = psRcvPackage->bRXData[8];
		Event.bStartSecond = psRcvPackage->bRXData[9];
		Event.bStopHour = psRcvPackage->bRXData[10];
		Event.bStopMinute = psRcvPackage->bRXData[11];
		Event.bStopSecond = psRcvPackage->bRXData[12];
		

		ProcessSetEvent(&Event);
	}
	break;

	case CMD_GET_EVENT:
	{
		sEvent Event;

		Event.bEventSlot = psRcvPackage->bRXData[0];

		psRcvPackage->bStatus = ProcessGetEvents(&Event);


		if (psRcvPackage->bStatus == 0U)
		{
			psRcvPackage->bRXData[1] = Event.bEventType;
			psRcvPackage->bRXData[2] = Event.bRelay;
			psRcvPackage->bRXData[3] = Event.bDay;
			psRcvPackage->bRXData[4] = Event.bMounth;
			psRcvPackage->bRXData[5] = Event.bYear;
			psRcvPackage->bRXData[6] = Event.bWeekDays;
			psRcvPackage->bRXData[7] = Event.bStartHour;
			psRcvPackage->bRXData[8] = Event.bStartMinute;
			psRcvPackage->bRXData[9] = Event.bStartSecond;
			psRcvPackage->bRXData[10] = Event.bStopHour;
			psRcvPackage->bRXData[11] = Event.bStopMinute;
			psRcvPackage->bRXData[12] = Event.bStopSecond;
			psRcvPackage->bRXData[13] = Event.bEventSlot;

			psRcvPackage->bNumData = 13;	

		}
		
	}
	break;

	default:

		break;


	}

#ifdef DEBUG_SERIAL
	Serial.println("Generic Data. Retransmitting...");
#endif
	SendPackage(psRcvPackage, TETHERNET);

#ifdef DEBUG_SERIAL
	Serial.println("Done Process Package.");
#endif
	return 0;

}

BYTE SendPackage(sPackage * psRcvPackage, TYPECOM FromCOM)
{
	BYTE bBuffer[68];
	BYTE bSentData = 0;
	BYTE bReceiveData = 0;
	BYTE bRet = 0;
	BYTE bTimeout = 0;

	memset(bBuffer, 0, sizeof(bBuffer));

	MakePackage(psRcvPackage, bBuffer);


	switch (FromCOM)
	{
	case TSERIAL:
		Serial.write(&bBuffer[0], GetPkgSize(psRcvPackage));
		break;

	case TETHERNET:

		udp.beginPacket(udp.remoteIP(), udp.remotePort());
		bSentData = udp.write(bBuffer, GetPkgSize(psRcvPackage));
		udp.endPacket();

		bRet = bSentData ? 0 : 1;

		udp.endPacket();

		if (bRet != 0)
		{
			return SEND_ERROR;
		}

		/*
		do
		{
		bRet = udp.parsePacket() ? 0 : 1;
		delay(10);
		} while (((bTimeout++) < 5) && (bRet != 0));
		break;
		*/
	}


	if (bRet != 0)
	{
		return bRet;
	}

	return bRet;
}

BYTE MakePackage(sPackage * psRcvPackage, BYTE bBuffer[])
{
	if ((psRcvPackage == NULL) || (bBuffer == NULL))
	{
		return (BYTE)-1;
	}

	bBuffer[0] = STARTHEADING;
	bBuffer[1] = STARTHEADING;
	bBuffer[2] = psRcvPackage->bSession;
	bBuffer[3] = psRcvPackage->bCmd;
	bBuffer[4] = psRcvPackage->bDest;
	bBuffer[5] = psRcvPackage->bStatus;
	bBuffer[6] = psRcvPackage->bNumData;

	memcpy(&bBuffer[7], psRcvPackage->bRXData, (psRcvPackage->bNumData));

	bBuffer[psRcvPackage->bNumData + 7] = psRcvPackage->bCRC;
	bBuffer[psRcvPackage->bNumData + 8] = ENDTRANSMISSION;
	return 0;

}

size_t GetPkgSize(sPackage * psRcvPackage)
{
	if ((psRcvPackage == NULL))
	{
		return (BYTE)-1;
	}
	return psRcvPackage->bNumData + 9;
}

void WriteRelay(int iRelay, BYTE bState)
{
	bState = !bState;

#ifdef RELAY_CHANNELS_8

	BYTE RelayBlockA = 0U;
	WORD wAddr = EEPROM_RLY_STATUS_ADDR + RELAY_BLOCK_A;

	RelayBlockA = EEPROM.read((int)wAddr);

	RelayBlockA = RelayBlockA & (~(1 << (iRelay-1)));

	RelayBlockA = (RelayBlockA | (bState << (iRelay-1)));

	EEPROM.write((int)wAddr, RelayBlockA);

#endif
}

BYTE ProcessSetEvent(sEvent * Event)
{
	BYTE bBuffer[16];
	int iSlotAddr = 0;
	if ((Event->bEventSlot > MAX_SLOTS) || (Event == NULL) )
	{
		return (BYTE)-1;
	}

	memset(bBuffer, 0U, sizeof(bBuffer));

	bBuffer[0] = Event->bEventType;
	bBuffer[1] = Event->bRelay;
	bBuffer[2] = Event->bDay;
	bBuffer[3] = Event->bMounth;
	bBuffer[4] = Event->bYear;
	bBuffer[5] = Event->bWeekDays;
	bBuffer[6] = Event->bStartHour;
	bBuffer[7] = Event->bStartMinute;
	bBuffer[8] = Event->bStartSecond;
	bBuffer[9] = Event->bStopHour;
	bBuffer[10] = Event->bStopMinute;
	bBuffer[11] = Event->bStopSecond;

	iSlotAddr = SLOTS_OFFSET * Event->bEventSlot;

	for (int iCount = 0; iCount < 12; iCount++)
	{
		EEPROM.write((iSlotAddr + iCount), bBuffer[iCount]);
	}	

	return 0;

}

BYTE ProcessGetEvents(sEvent * Event)
{
	if (Event == NULL)
	{
		return ((BYTE) - 1);
	}

	BYTE bBuffer[16];

	int iSlotAddr = SLOTS_OFFSET * Event->bEventSlot;

	memset(bBuffer, 0U, sizeof(bBuffer));

	for (int iCount = 0; iCount < 12; iCount++)
	{
		bBuffer[iCount] = EEPROM.read(iSlotAddr + iCount);
	}

	if ((bBuffer[0] < OnceDayEvent) || ((bBuffer[0] > WeeklyEvent)))
	{
		return (BYTE)-1;
	}


	Event->bEventType   = bBuffer[0];
	Event->bRelay       = bBuffer[1];
	Event->bDay         = bBuffer[2];
	Event->bMounth      = bBuffer[3];
	Event->bYear        = bBuffer[4];
	Event->bWeekDays    = bBuffer[5];
	Event->bStartHour   = bBuffer[6];
	Event->bStartMinute = bBuffer[7];
	Event->bStartSecond = bBuffer[8];
	Event->bStopHour    = bBuffer[9];
	Event->bStopMinute  = bBuffer[10];
	Event->bStopSecond  = bBuffer[11];

	return 0;

}

BYTE ClearEvent(sEvent * Event)
{
	int iSlotAddr = 0;
	if ((Event->bEventSlot > MAX_SLOTS) || (Event == NULL))
	{
		return (BYTE)-1;
	}


	iSlotAddr = SLOTS_OFFSET * Event->bEventSlot;

	for (int iCount = 0; iCount < 12; iCount++)
	{
		EEPROM.write((iSlotAddr + iCount), 0U);
	}

	return 0;
}

void ProcessRelays(void)
{

#ifdef RELAY_CHANNELS_8
	
	BYTE RelayBlockA = 0U;
	WORD wAddr = EEPROM_RLY_STATUS_ADDR + RELAY_BLOCK_A;

	RelayBlockA = EEPROM.read((int)wAddr);

	for (BYTE bCount = 0; bCount < 8; bCount++)
	{
		digitalWrite((BYTE)(RELAY_OFFSET + bCount + 1), (BYTE)((RelayBlockA >> (bCount)) & 0x01));
	}


#endif

}

void ProcessEvents(BYTE EventToCheck)
{	
	sEvent Event;
        DateTime Now = RTC.now();
	Event.bEventSlot = EventToCheck;

	if (ProcessGetEvents(&Event) == 0)
	{	
#ifdef DEBUG_SERIAL
		Serial.print("Event Available Slot ");
		Serial.println(EventToCheck);
#endif

		switch (Event.bEventType)
		{		
			case OnceDayEvent:
			{
#ifdef DEBUG_SERIAL
				Serial.println("Once Day Event");				
#endif
				Now = RTC.now();
				DateTime StartTime(Event.bYear+2000, Event.bMounth, Event.bDay, Event.bStartHour, Event.bStartMinute, Event.bStartSecond);
				DateTime StopTime (Event.bYear+2000, Event.bMounth, Event.bDay, Event.bStopHour, Event.bStopMinute, Event.bStopSecond);


#ifdef DEBUG_SERIAL
				Serial.println("Startting...");
				Serial.print(StartTime.day(), DEC);//Imprimindo o dia
				Serial.print('/');
				Serial.print(StartTime.month(), DEC);//Recuperando o mes
				Serial.print('/');
				Serial.print(StartTime.year(), DEC);//Recuperando o ano
				Serial.print(' ');
				Serial.print(StartTime.hour(), DEC);//Recuperando a hora
				Serial.print(':');
				Serial.print(StartTime.minute(), DEC);//Reci[erando os minutos
				Serial.print(':');
				Serial.print(StartTime.second(), DEC);//Recuperando os segundos
				Serial.println();
				Serial.println("Stopping...");
				Serial.print(StopTime.day(), DEC);//Imprimindo o dia
				Serial.print('/');
				Serial.print(StopTime.month(), DEC);//Recuperando o mes
				Serial.print('/');
				Serial.print(StopTime.year(), DEC);//Recuperando o ano
				Serial.print(' ');
				Serial.print(StopTime.hour(), DEC);//Recuperando a hora
				Serial.print(':');
				Serial.print(StopTime.minute(), DEC);//Reci[erando os minutos
				Serial.print(':');
				Serial.print(StopTime.second(), DEC);//Recuperando os segundos
				Serial.println();
#endif

				if ((Now.secondstime() >= StartTime.secondstime()) && (Now.secondstime() <= StopTime.secondstime()))
				{
					WriteRelay(Event.bRelay, HIGH);
				}
				else
				{
					WriteRelay(Event.bRelay, LOW);
				}
			}
			break;

			case DailyEvent:
			{
#ifdef DEBUG_SERIAL
				Serial.println("Daily Event");
#endif
				Now = RTC.now();
				DateTime StartTime(Now.year(), Now.month(), Now.day(), Event.bStartHour, Event.bStartMinute, Event.bStartSecond);
				DateTime StopTime (Now.year(), Now.month(), Now.day(), Event.bStopHour, Event.bStopMinute, Event.bStopSecond);
				
				if ((Now.secondstime() >= StartTime.secondstime()) && (Now.secondstime() <= StopTime.secondstime()))
				{
					WriteRelay(Event.bRelay, HIGH);
				}
				else
				{
					WriteRelay(Event.bRelay, LOW);
				}
			}
			break;

			case WeeklyEvent:
			{
#ifdef DEBUG_SERIAL
				Serial.println("Weekly Event");
#endif
			        Now = RTC.now();

				for (int iWeekDay = 1; iWeekDay < 8; iWeekDay++)
				{
					BYTE Week = (BYTE)(Event.bWeekDays >> (iWeekDay - 1)) & 0x01;

					if (Week != 0)
					{
						if (Now.dayOfWeek() == iWeekDay)
						{
							DateTime StartTime(Now.year(), Now.month(), Now.day(), Event.bStartHour, Event.bStartMinute, Event.bStartSecond);
							DateTime StopTime(Now.year(), Now.month(), Now.day(), Event.bStopHour, Event.bStopMinute, Event.bStopSecond);

							if ((Now.secondstime() >= StartTime.secondstime()) && (Now.secondstime() <= StopTime.secondstime()))
							{
								WriteRelay(Event.bRelay, HIGH);
							}
							else
							{
								WriteRelay(Event.bRelay, LOW);
							}
						}
					}

				}
			}
			break;

			default:
#ifdef DEBUG_SERIAL
				Serial.println("Invalid Event");
			//	Serial.println(EventToCheck);
#endif
				break;
		}
	}


	
}

