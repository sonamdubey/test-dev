CREATE TABLE [dbo].[OldModelMaskingNames] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ModelId]        NUMERIC (18) NOT NULL,
    [OldMaskingName] VARCHAR (50) NULL,
    [UpdatedOn]      DATETIME     NULL,
    [UpdatedBy]      NUMERIC (18) NULL
);

