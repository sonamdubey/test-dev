CREATE TABLE [dbo].[ESM_Properties] (
    [ESM_PropertyId] INT           IDENTITY (1, 1) NOT NULL,
    [PropertyName]   VARCHAR (100) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_ESM_Properties_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ESM_Properties] PRIMARY KEY CLUSTERED ([ESM_PropertyId] ASC)
);

