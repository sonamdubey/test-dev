CREATE TABLE [dbo].[AbSure_PartsInspectionDataLog] (
    [Id]                         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId]        NUMERIC (18)  NULL,
    [AbSure_QCarPartsId]         INT           NULL,
    [AbSure_QCarPartResponsesId] VARCHAR (100) NULL,
    [ResponseComments]           VARCHAR (500) NULL,
    [ResponseDate]               DATETIME      NULL,
    [Timestamp]                  DATETIME      NULL,
    CONSTRAINT [PK_AbSure_PartsInspectionDataLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

