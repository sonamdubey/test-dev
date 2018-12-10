CREATE TABLE [dbo].[CarGroupTypes] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [ModelId]        INT NULL,
    [CarGroupTypeId] INT NULL,
    [IsActive]       BIT CONSTRAINT [DF_CarGroupTypes_IsActive] DEFAULT ((1)) NOT NULL,
    [VersionId]      INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

