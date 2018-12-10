CREATE TABLE [dbo].[CW_CarSegments] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [Segments] VARCHAR (50) NULL,
    [IsActive] BIT          CONSTRAINT [DF_CW_CarSegments_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CW_CarSegments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

