CREATE TABLE [dbo].[OpinionPollCategory] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [IsActive] BIT          CONSTRAINT [DF_OpinionPollCategory_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_OpinionPollCategory] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

