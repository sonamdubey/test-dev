CREATE TABLE [dbo].[Reviews] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]    NUMERIC (18)  NOT NULL,
    [Title]      VARCHAR (200) NOT NULL,
    [Summary]    VARCHAR (200) NULL,
    [ReviewDate] DATETIME      NOT NULL,
    [IsActive]   BIT           CONSTRAINT [DF_Reviews_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

