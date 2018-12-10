CREATE TABLE [dbo].[AbSure_QCarParts] (
    [AbSure_QCarPartsId] INT           IDENTITY (1, 1) NOT NULL,
    [PartName]           VARCHAR (200) NULL,
    [IsActive]           BIT           CONSTRAINT [DF_AbSure_QCarParts_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_AbSure_QCarParts] PRIMARY KEY CLUSTERED ([AbSure_QCarPartsId] ASC)
);

