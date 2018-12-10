CREATE TABLE [dbo].[DD_DealerNames] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [IsActive]  BIT          NOT NULL,
    [CreatedOn] DATETIME     NOT NULL,
    [CreatedBy] INT          NOT NULL,
    CONSTRAINT [PK_DD_DealerNames] PRIMARY KEY CLUSTERED ([Id] ASC)
);

