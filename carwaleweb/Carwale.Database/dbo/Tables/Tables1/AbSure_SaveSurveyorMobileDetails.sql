CREATE TABLE [dbo].[AbSure_SaveSurveyorMobileDetails] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [PhoneDetails]      VARCHAR (100) NULL,
    [PhoneManufacturer] VARCHAR (100) NULL,
    [PhoneApiLevel]     VARCHAR (100) NULL,
    [AppVersion]        VARCHAR (40)  NULL,
    [UserId]            INT           NULL,
    [AbSure_CarId]      INT           NULL,
    [PhoneImei]         VARCHAR (100) NULL,
    [EntryDate]         DATETIME      NULL,
    CONSTRAINT [PK__AbSure_S__3214EC2736AAF955] PRIMARY KEY CLUSTERED ([ID] ASC)
);

