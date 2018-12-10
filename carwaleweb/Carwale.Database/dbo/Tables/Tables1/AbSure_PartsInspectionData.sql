CREATE TABLE [dbo].[AbSure_PartsInspectionData] (
    [Id]                         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId]        NUMERIC (18)  NULL,
    [AbSure_QCarPartsId]         INT           NULL,
    [AbSure_QCarPartResponsesId] VARCHAR (100) NULL,
    [ResponseComments]           VARCHAR (500) NULL,
    [ResponseDate]               DATETIME      CONSTRAINT [DF_AbSure_PartsInspectionData_ResponseDate] DEFAULT (getdate()) NULL,
    [Timestamp]                  DATETIME      NULL,
    CONSTRAINT [PK_AbSure_PartsInspectionData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

