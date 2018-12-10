CREATE TABLE [dbo].[NCS_Dealers] (
    [ID]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]             VARCHAR (200) NOT NULL,
    [DealerTitle]      VARCHAR (200) NULL,
    [CityId]           NUMERIC (18)  NOT NULL,
    [LandlineNo]       VARCHAR (50)  NULL,
    [Mobile]           VARCHAR (100) NULL,
    [ContactPerson]    VARCHAR (200) NULL,
    [Address]          VARCHAR (500) NULL,
    [EMail]            VARCHAR (200) NULL,
    [EntryDateTime]    DATETIME      NOT NULL,
    [IsActive]         BIT           CONSTRAINT [DF_INS_Dealers_IsActive] DEFAULT ((1)) NOT NULL,
    [IsPQActive]       BIT           CONSTRAINT [DF_NCS_Dealers_IsPQActive] DEFAULT ((0)) NULL,
    [DealerCode]       VARCHAR (50)  NULL,
    [UpdatedBy]        NUMERIC (18)  NULL,
    [UpdatedOn]        DATETIME      NULL,
    [IsNCDDealer]      BIT           NULL,
    [TCDealerId]       NUMERIC (18)  NULL,
    [CampaignId]       NUMERIC (18)  NULL,
    [TargetLeads]      BIGINT        NULL,
    [DelLeads]         BIGINT        NULL,
    [EndDate]          DATETIME      NULL,
    [PrincipleEmail]   VARCHAR (200) NULL,
    [PrincipleMobile]  VARCHAR (50)  NULL,
    [PrincipleName]    VARCHAR (200) NULL,
    [IsDealerFeedback] BIT           NULL,
    [AreaId]           NUMERIC (18)  NULL,
    [Latitude]         DECIMAL (18)  NULL,
    [Longitude]        DECIMAL (18)  NULL,
    [DealerType]       TINYINT       NULL,
    [IsDailyBased]     BIT           NULL,
    [DailyDel]         INT           NULL,
    [DailyCount]       INT           NULL,
    [IsModelBased]     BIT           CONSTRAINT [DF_NCS_Dealers_IsModelBased] DEFAULT ((0)) NULL,
    [AreaName]         VARCHAR (250) NULL,
    CONSTRAINT [PK_INS_Dealers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_NCS_Dealers__CityId__IsActive]
    ON [dbo].[NCS_Dealers]([CityId] ASC, [IsActive] ASC)
    INCLUDE([ID], [Name]);


GO
CREATE NONCLUSTERED INDEX [ix_NCS_Dealers__CityId]
    ON [dbo].[NCS_Dealers]([CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_NCS_Dealers__IsActive]
    ON [dbo].[NCS_Dealers]([IsActive] ASC)
    INCLUDE([ID], [Name], [CityId]);

