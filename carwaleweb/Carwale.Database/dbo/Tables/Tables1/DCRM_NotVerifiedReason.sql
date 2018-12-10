CREATE TABLE [dbo].[DCRM_NotVerifiedReason] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Reason]   VARCHAR (100) NULL,
    [StatusId] SMALLINT      NULL,
    CONSTRAINT [PK_DCRM_NotVerifiedReason] PRIMARY KEY CLUSTERED ([Id] ASC)
);

