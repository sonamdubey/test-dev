CREATE TABLE [dbo].[TC_LeadStage] (
    [TC_LeadStageId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [LeadStage]      VARCHAR (50) NULL,
    [IsActive]       BIT          CONSTRAINT [DF_TC_LeadStage_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_LeadStageId] PRIMARY KEY NONCLUSTERED ([TC_LeadStageId] ASC)
);

