CREATE TABLE [dbo].[CRM_ExecAllocationLog] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ExecId]    BIGINT       NOT NULL,
    [LeadId]    BIGINT       NOT NULL,
    [CBDId]     BIGINT       NOT NULL,
    [DealerId]  BIGINT       NOT NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_CRM_ExecAllocationLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CRM_ExecAllocationLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

