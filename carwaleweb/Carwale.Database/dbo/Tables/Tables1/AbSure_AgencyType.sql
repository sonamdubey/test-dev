CREATE TABLE [dbo].[AbSure_AgencyType] (
    [Id]       INT          NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [IsActive] BIT          NULL,
    CONSTRAINT [PK_AbSure_AgencyType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

