CREATE TYPE [dbo].[AbSure_PartsInspectionDataTblTyp] AS TABLE (
    [QCarPartsId]         INT           NULL,
    [QCarPartResponsesId] VARCHAR (100) NULL,
    [ResponseComments]    VARCHAR (500) NULL,
    [Timestamp]           DATETIME      NULL);

