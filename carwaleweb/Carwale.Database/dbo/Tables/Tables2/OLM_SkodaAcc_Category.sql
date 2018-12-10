CREATE TABLE [dbo].[OLM_SkodaAcc_Category] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Category] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_OLM_SkodaAcc_Categories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

