CREATE TABLE [dbo].[BerkshireCityInfo] (
    [ID]          BIGINT        NULL,
    [CITY_CODE]   INT           NULL,
    [CITY]        VARCHAR (40)  NULL,
    [STATE_CODE]  INT           NULL,
    [STATE]       VARCHAR (40)  NULL,
    [AREA_NAME]   VARCHAR (100) NULL,
    [COMBINATION] VARCHAR (255) NULL,
    [PIN]         BIGINT        NULL,
    [ZONE]        VARCHAR (40)  NULL,
    [IS_DELETED]  BIT           NOT NULL
);

