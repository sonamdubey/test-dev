CREATE TABLE [dbo].[BA_RegisterBroker] (
    [ID]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [BrokerMobile]     VARCHAR (50)  NOT NULL,
    [BrokerName]       VARCHAR (50)  NULL,
    [Email]            VARCHAR (50)  NULL,
    [StateId]          INT           NULL,
    [CityId]           INT           NULL,
    [DownloadDate]     DATETIME      NULL,
    [DateofActivation] DATETIME      NULL,
    [EndOfActivation]  DATETIME      NULL,
    [ActiveDays]       INT           NULL,
    [ContactHours]     VARCHAR (50)  NULL,
    [Comments]         VARCHAR (500) NULL,
    [IsVerified]       BIT           CONSTRAINT [DF_BA.RegisterBroker_IsVerified] DEFAULT ((0)) NOT NULL,
    [IsActive]         BIT           CONSTRAINT [DF_BA.RegisterBroker_IsActive] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BA.RegisterBroker] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [Mobile_BA_RegisterBroker] UNIQUE NONCLUSTERED ([BrokerMobile] ASC)
);

