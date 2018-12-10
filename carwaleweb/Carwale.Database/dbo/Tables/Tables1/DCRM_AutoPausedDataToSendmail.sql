CREATE TABLE [dbo].[DCRM_AutoPausedDataToSendmail] (
    [Id]                     INT           IDENTITY (1, 1) NOT NULL,
    [DealerPackageFeatureId] INT           NULL,
    [TransactionId]          INT           NULL,
    [DealerId]               INT           NULL,
    [DealerName]             VARCHAR (200) NULL,
    [CityName]               VARCHAR (100) NULL,
    [CityId]                 INT           NULL,
    [L3UserId]               INT           NULL,
    [L3Name]                 VARCHAR (100) NULL,
    [L3LoginId]              VARCHAR (200) NULL,
    [Amount]                 FLOAT (53)    NULL,
    [StartDate]              DATETIME      NULL,
    [PausedDate]             DATETIME      NULL,
    [PackageId]              INT           NULL,
    [PackageName]            VARCHAR (250) NULL,
    [CreatedOn]              DATETIME      NULL,
    [IsMailSend]             BIT           CONSTRAINT [DF__DCRM_Auto__IsMai__37DFE007] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_DCRM_AutoPausedDataToSendmail] PRIMARY KEY CLUSTERED ([Id] ASC)
);

