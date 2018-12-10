CREATE TABLE [dbo].[Acc_ReviewComments] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ReviewId]     NUMERIC (18)   NOT NULL,
    [CustomerId]   NUMERIC (18)   NOT NULL,
    [Comments]     VARCHAR (1000) NOT NULL,
    [PostDateTime] DATETIME       NOT NULL,
    [IsApproved]   BIT            CONSTRAINT [DF_Acc_ReviewComments_IsApproved] DEFAULT ((0)) NOT NULL,
    [ReportAbuse]  BIT            CONSTRAINT [DF_Acc_ReviewComments_ReportAbuse] DEFAULT ((0)) NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_Acc_ReviewComments_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Acc_ReviewComments] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

