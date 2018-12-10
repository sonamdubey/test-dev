CREATE TABLE [dbo].[CarwaleCompetitors] (
    [ID]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NAme]      VARCHAR (50) NOT NULL,
    [IsRelated] BIT          CONSTRAINT [DF_CarwaleCompetitors_IsRelated] DEFAULT (0) NULL,
    [IsActive]  BIT          CONSTRAINT [DF_CarwaleCompetitors_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_CarwaleCompetitors] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

