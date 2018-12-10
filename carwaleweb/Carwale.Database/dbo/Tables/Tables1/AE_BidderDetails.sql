CREATE TABLE [dbo].[AE_BidderDetails] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerId]      NUMERIC (18)  NOT NULL,
    [BidderType]      SMALLINT      NOT NULL,
    [Name]            VARCHAR (50)  NULL,
    [Email]           VARCHAR (50)  NULL,
    [Mobile]          VARCHAR (15)  NULL,
    [Landline]        VARCHAR (15)  NULL,
    [OtherContacts]   VARCHAR (50)  NULL,
    [CityId]          NUMERIC (18)  NULL,
    [Address]         VARCHAR (250) NULL,
    [PanNumber]       VARCHAR (10)  NULL,
    [ContactPerson]   VARCHAR (50)  NOT NULL,
    [AvailableTokens] INT           CONSTRAINT [DF_AE_BidderDetails_AvailableTokens] DEFAULT ((0)) NOT NULL,
    [UsedTokens]      INT           CONSTRAINT [DF_AE_BidderDetails_UsedTokens] DEFAULT ((0)) NOT NULL,
    [EntryDate]       DATETIME      NULL,
    [UpdatedOn]       DATETIME      NULL,
    [UpdatedBy]       NUMERIC (18)  NULL,
    CONSTRAINT [PK_AE_ConsumerDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

