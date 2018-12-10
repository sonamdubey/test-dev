CREATE TABLE [dbo].[DCRM_TransferLog] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]   NUMERIC (18) NOT NULL,
    [FromUserId] NUMERIC (18) NOT NULL,
    [ToUserId]   NUMERIC (18) NOT NULL,
    [RoleId]     NUMERIC (18) NOT NULL,
    [UpdatedOn]  DATETIME     NOT NULL,
    [UpdatedBy]  NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DCRM_TransferLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

