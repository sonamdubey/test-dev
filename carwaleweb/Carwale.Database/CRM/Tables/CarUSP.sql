CREATE TABLE [CRM].[CarUSP] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [MakeId]         NUMERIC (18)  NOT NULL,
    [ModelId]        NUMERIC (18)  NOT NULL,
    [IsActive]       BIT           NULL,
    [Title]          VARCHAR (200) NOT NULL,
    [USPDescription] VARCHAR (MAX) NULL,
    [UpdatedBy]      NUMERIC (18)  NOT NULL,
    [UpdatedOn]      DATETIME      NOT NULL,
    CONSTRAINT [PK_CarUSP] PRIMARY KEY CLUSTERED ([Id] ASC)
);

