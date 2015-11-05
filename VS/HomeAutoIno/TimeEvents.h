#ifndef TIME_EVENTS_H
#define TIME_EVENTS_H

//#include "RTClib.h"

#include "Defs.h"

#define WK_DOMINGO 1U;
#define WK_SEGUNDA 2U;
#define WK_TERCA   4U;
#define WK_QUARTA  8U;
#define WK_QUINTA  16U;
#define WK_SEXTA   32U;
#define WK_SABADO  64U;

#define EEPROM_SLOTS_ADDR 0

#define SLOTS_OFFSET 12

#define MAX_SLOTS 16

typedef enum
{
	OnceDayEvent = 1,
	DailyEvent,
	WeeklyEvent
}EventType;

typedef struct
{
	BYTE bEventSlot;
	
	BYTE bEventType;

	BYTE bRelay;
	
	BYTE bDay;
	BYTE bMounth;
	BYTE bYear;

	BYTE bWeekDays;

	BYTE bStartHour;
	BYTE bStartMinute;
	BYTE bStartSecond;

	BYTE bStopHour;
	BYTE bStopMinute;
	BYTE bStopSecond;
	
}sEvent;


#endif
