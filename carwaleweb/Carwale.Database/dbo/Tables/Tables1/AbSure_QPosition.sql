CREATE TABLE [dbo].[AbSure_QPosition] (
    [AbSure_QPositionId] INT           IDENTITY (1, 1) NOT NULL,
    [Position]           VARCHAR (200) NULL,
    [IsActive]           BIT           CONSTRAINT [DF_AbSure_QPosition_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_AbSure_QPosition] PRIMARY KEY CLUSTERED ([AbSure_QPositionId] ASC)
);

