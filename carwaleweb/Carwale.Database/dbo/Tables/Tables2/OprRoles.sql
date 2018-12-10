CREATE TABLE [dbo].[OprRoles] (
    [ID]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (200) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_OprRoles_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_OprRoles] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

