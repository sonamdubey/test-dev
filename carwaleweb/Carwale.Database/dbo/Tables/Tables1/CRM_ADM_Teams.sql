CREATE TABLE [dbo].[CRM_ADM_Teams] (
    [ID]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]      VARCHAR (100) NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_CRM_ADM_Teams_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn] DATETIME      NOT NULL,
    [UpdatedOn] DATETIME      NOT NULL,
    [UpdatedBy] NUMERIC (18)  NOT NULL,
    [IsNcd]     BIT           CONSTRAINT [DF_CRM_ADM_Teams_IsNcd] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CRM_ADM_Teams] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

