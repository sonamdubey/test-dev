CREATE TABLE [dbo].[Absure_ReportProblems] (
    [Absure_ReportProblemsId] INT            IDENTITY (1, 1) NOT NULL,
    [Absure_CarDetailsId]     INT            NULL,
    [SurveyorId]              INT            NULL,
    [IsProblem]               BIT            NULL,
    [Comment]                 NVARCHAR (800) NULL,
    [AppVersion]              VARCHAR (40)   NULL,
    [PhoneApiLevel]           VARCHAR (100)  NULL,
    [PhoneImei]               VARCHAR (100)  NULL,
    [PhoneManufacturer]       VARCHAR (100)  NULL,
    [PhoneModel]              VARCHAR (100)  NULL,
    [CityId]                  INT            NULL,
    [AreaId]                  INT            NULL,
    [EntryDate]               DATETIME       NULL,
    [ImageCount]              INT            NULL
);

