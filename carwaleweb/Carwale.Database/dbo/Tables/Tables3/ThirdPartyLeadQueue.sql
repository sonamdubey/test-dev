CREATE TABLE [dbo].[ThirdPartyLeadQueue] (
    [ThirdPartyLeadId] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [PQId]             NUMERIC (18)   NOT NULL,
    [MakeId]           NUMERIC (18)   NOT NULL,
    [ModelId]          NUMERIC (18)   NOT NULL,
    [TPLeadSettingId]  NUMERIC (18)   NOT NULL,
    [ModelName]        VARCHAR (30)   NOT NULL,
    [CityId]           NUMERIC (18)   NOT NULL,
    [City]             VARCHAR (50)   NOT NULL,
    [CustomerName]     VARCHAR (100)  NOT NULL,
    [Email]            VARCHAR (100)  NOT NULL,
    [Mobile]           VARCHAR (20)   NOT NULL,
    [EntryDate]        DATETIME       CONSTRAINT [DF_ThirdPartyLeads_EntryDate] DEFAULT (getdate()) NOT NULL,
    [PushStatus]       NVARCHAR (200) NULL,
    [CampaignCode]     VARCHAR (20)   NULL,
    [DNDStatus]        VARCHAR (50)   NULL,
    [IsSuccess]        BIT            NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_ThirdPartyLeadQueue_PQId]
    ON [dbo].[ThirdPartyLeadQueue]([PQId] ASC);

