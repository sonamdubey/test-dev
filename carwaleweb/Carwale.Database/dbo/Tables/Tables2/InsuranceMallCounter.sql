CREATE TABLE [dbo].[InsuranceMallCounter] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [HitDateTime] DATETIME     NOT NULL,
    CONSTRAINT [PK_InsuranceMallCounter] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

