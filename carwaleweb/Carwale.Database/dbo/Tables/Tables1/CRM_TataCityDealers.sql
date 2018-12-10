CREATE TABLE [dbo].[CRM_TataCityDealers] (
    [Id]               INT           NOT NULL,
    [CWCityId]         INT           NULL,
    [DealerDivisionId] VARCHAR (50)  NULL,
    [PplRowId]         VARCHAR (50)  NULL,
    [OrgId]            VARCHAR (50)  NULL,
    [OrgName]          VARCHAR (100) NULL,
    [CommonName]       VARCHAR (100) NULL,
    [IsActive]         BIT           NULL,
    CONSTRAINT [PK_CRM_TataCityDealers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

