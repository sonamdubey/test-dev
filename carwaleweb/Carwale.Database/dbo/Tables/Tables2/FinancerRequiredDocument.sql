CREATE TABLE [dbo].[FinancerRequiredDocument] (
    [Id]             INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FinancerId]     INT           NOT NULL,
    [UserTypeId]     INT           NOT NULL,
    [isUsed]         BIT           NOT NULL,
    [DocumentTypeId] INT           NOT NULL,
    [isActive]       CHAR (10)     NOT NULL,
    [Comment]        VARCHAR (250) NULL,
    CONSTRAINT [PK_FinancerRequiredDocument] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

