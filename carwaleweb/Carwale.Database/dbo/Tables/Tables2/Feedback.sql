CREATE TABLE [dbo].[Feedback] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId] NUMERIC (18) NULL,
    [AnswerId]   NUMERIC (18) NULL,
    [FBDate]     DATETIME     NULL,
    [UpdatedOn]  DATETIME     NULL,
    [UpdatedBy]  NUMERIC (18) NULL,
    CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

