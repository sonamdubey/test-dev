CREATE TABLE [dbo].[CRM_ConCall] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CBDId]        NUMERIC (18) NOT NULL,
    [UpdatedBy]    NUMERIC (18) NOT NULL,
    [UpdatedOn]    DATETIME     NOT NULL,
    [ConCallValue] BIGINT       NOT NULL
);

