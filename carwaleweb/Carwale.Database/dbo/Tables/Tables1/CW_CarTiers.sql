CREATE TABLE [dbo].[CW_CarTiers] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [IsActive] BIT          CONSTRAINT [DF_CW_CarTiers_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CW_CarTiers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

