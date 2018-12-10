CREATE TABLE [dbo].[AbSure_QCarPartResponses] (
    [AbSure_QCarPartResponsesId] INT           IDENTITY (1, 1) NOT NULL,
    [Response]                   VARCHAR (200) NULL,
    [IsActive]                   BIT           CONSTRAINT [DF_AbSure_QCarPartResponses_IsActive] DEFAULT ((1)) NULL,
    [WeightagePercent]           INT           NULL,
    CONSTRAINT [PK_AbSure_QCarPartResponses] PRIMARY KEY CLUSTERED ([AbSure_QCarPartResponsesId] ASC)
);

