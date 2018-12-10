CREATE TABLE [dbo].[AbSure_QArea] (
    [AbSure_QAreaId] INT           IDENTITY (1, 1) NOT NULL,
    [Area]           VARCHAR (200) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_AbSure_QArea_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_AbSure_QArea] PRIMARY KEY CLUSTERED ([AbSure_QAreaId] ASC)
);

