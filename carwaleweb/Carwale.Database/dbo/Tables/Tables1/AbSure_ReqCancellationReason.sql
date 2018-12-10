CREATE TABLE [dbo].[AbSure_ReqCancellationReason] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Reason]   VARCHAR (250) NULL,
    [IsActive] BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

