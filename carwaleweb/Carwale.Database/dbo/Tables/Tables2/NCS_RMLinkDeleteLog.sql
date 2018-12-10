CREATE TABLE [dbo].[NCS_RMLinkDeleteLog] (
    [Id]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [DealerId]  VARCHAR (2000) NOT NULL,
    [UpdatedBy] NUMERIC (18)   NOT NULL,
    [UpdatedOn] DATETIME       CONSTRAINT [DF_NCS_RMLinkDeleteLog_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_NCS_RMLinkDeleteLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

