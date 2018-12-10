CREATE TABLE [dbo].[CW_ESSurveyCustomer] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50) NULL,
    [Email]         VARCHAR (50) NULL,
    [Source]        VARCHAR (50) NULL,
    [EntryDate]     DATETIME     CONSTRAINT [DF_ESSurveyCustomer_EntryDate] DEFAULT (getdate()) NULL,
    [CityId]        INT          NULL,
    [ContactNumber] VARCHAR (10) NULL,
    CONSTRAINT [PK_ESSurveyCustomer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

