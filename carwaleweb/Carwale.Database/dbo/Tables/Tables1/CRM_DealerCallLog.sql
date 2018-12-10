CREATE TABLE [dbo].[CRM_DealerCallLog] (
    [Id]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [CallId]    VARCHAR (1500) NOT NULL,
    [DealerId]  NUMERIC (18)   NOT NULL,
    [Subject]   VARCHAR (500)  NOT NULL,
    [Comment]   VARCHAR (1500) NOT NULL,
    [UpdatedOn] DATETIME       NOT NULL,
    [UpdatedBy] NUMERIC (18)   NOT NULL,
    CONSTRAINT [PK_CRM_DealerCallLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

