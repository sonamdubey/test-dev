CREATE TABLE [dbo].[BhartiAxa_Cities] (
    [BhartiAxa_CitiesId] INT          IDENTITY (1, 1) NOT NULL,
    [RTA_CODE]           VARCHAR (10) NULL,
    [STATE]              VARCHAR (60) NULL,
    [Rto_CITY]           VARCHAR (60) NULL,
    [Zone]               VARCHAR (10) NULL,
    [Branch]             VARCHAR (50) NULL,
    [CW_City]            VARCHAR (50) NULL,
    [CW_CityId]          INT          NULL,
    [CW_State]           VARCHAR (50) NULL,
    [CW_StateId]         INT          NULL,
    CONSTRAINT [PK_BhartiAxa_CitiesId] PRIMARY KEY CLUSTERED ([BhartiAxa_CitiesId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_BhartiAxa_Cities_CW_CityId]
    ON [dbo].[BhartiAxa_Cities]([CW_CityId] ASC);

