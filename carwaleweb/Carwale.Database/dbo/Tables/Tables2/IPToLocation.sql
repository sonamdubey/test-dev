CREATE TABLE [dbo].[IPToLocation] (
    [IP_FROM]      NUMERIC (18)  NULL,
    [IP_TO]        NUMERIC (18)  NULL,
    [COUNTRY_CODE] CHAR (2)      NULL,
    [COUNTRY_NAME] VARCHAR (64)  NULL,
    [REGION]       VARCHAR (128) NULL,
    [CITY]         VARCHAR (128) NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_IPToLocation_IPFrom_IPTo]
    ON [dbo].[IPToLocation]([IP_FROM] ASC, [IP_TO] ASC)
    INCLUDE([CITY]);


GO
CREATE NONCLUSTERED INDEX [ix_IPToLocation_CITY]
    ON [dbo].[IPToLocation]([CITY] ASC);

