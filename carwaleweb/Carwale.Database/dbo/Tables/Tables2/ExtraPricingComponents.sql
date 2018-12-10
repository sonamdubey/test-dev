CREATE TABLE [dbo].[ExtraPricingComponents] (
    [Id]       NUMERIC (18) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_ExtraPricingComponents_IsActive] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_ExtraPricingComponents] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

