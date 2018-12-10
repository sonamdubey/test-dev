CREATE TABLE [dbo].[TC_CallAction] (
    [TC_CallActionId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (50) NULL,
    [IsActive]        BIT          CONSTRAINT [DF_TC_CallAction_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_CallDespositionID] PRIMARY KEY NONCLUSTERED ([TC_CallActionId] ASC)
);

