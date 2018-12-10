CREATE TABLE [dbo].[CRM_SubDisposition] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SubDisposition]   VARCHAR (100) NOT NULL,
    [DispId]           SMALLINT      NOT NULL,
    [IsLead]           BIT           NOT NULL,
    [IsStatusReceived] BIT           NULL,
    [IsFinalStatus]    BIT           NULL,
    [IsActive]         BIT           NULL,
    [IsConnected]      BIT           NULL,
    CONSTRAINT [PK_CRM_PendingStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

