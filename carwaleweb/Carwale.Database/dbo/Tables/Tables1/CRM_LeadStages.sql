CREATE TABLE [dbo].[CRM_LeadStages] (
    [ID]       SMALLINT      NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsClosed] BIT           CONSTRAINT [DF_CNS_LeadStatus_IsOpen] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CNS_LeadStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

