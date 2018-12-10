CREATE TABLE [dbo].[CRM_HolidayList] (
    [Id]      INT  IDENTITY (1, 1) NOT NULL,
    [Holiday] DATE NOT NULL,
    CONSTRAINT [PK_CRM_HolidayList] PRIMARY KEY CLUSTERED ([Id] ASC)
);

