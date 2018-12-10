CREATE TABLE [dbo].[UP_Comments] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PhotoId]      NUMERIC (18)  NOT NULL,
    [CustomerId]   NUMERIC (18)  NOT NULL,
    [Comments]     VARCHAR (500) NULL,
    [PostDateTime] DATETIME      NULL,
    [IsApproved]   BIT           NULL,
    [ReportAbuse]  BIT           NULL,
    [IsActive]     BIT           NULL,
    CONSTRAINT [PK_UP_Comments] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

