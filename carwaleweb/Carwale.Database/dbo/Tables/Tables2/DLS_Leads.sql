CREATE TABLE [dbo].[DLS_Leads] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (200) NOT NULL,
    [EMail]         VARCHAR (150) NOT NULL,
    [Mobile]        VARCHAR (15)  NOT NULL,
    [Landline]      VARCHAR (15)  NULL,
    [CityId]        NUMERIC (18)  NOT NULL,
    [CWCustomerId]  NUMERIC (18)  NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_DLS_Leads_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]     DATETIME      NULL,
    [LeadStage]     SMALLINT      NULL,
    [ProductStatus] SMALLINT      NULL,
    [DealerId]      NUMERIC (18)  NULL,
    [Comments]      VARCHAR (250) NULL,
    [BuyTime]       VARCHAR (15)  NULL,
    [LeadType]      SMALLINT      CONSTRAINT [DF_DLS_Leads_LeadType] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DLS_Customers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Verification, 2-Consultation, 3-Closing', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DLS_Leads', @level2type = N'COLUMN', @level2name = N'LeadStage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Fiat Leads, 2-OtherLeads', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DLS_Leads', @level2type = N'COLUMN', @level2name = N'LeadType';

