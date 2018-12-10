CREATE TABLE [dbo].[OLM_HolidayList] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId] INT          NULL,
    [Holiday]  DATETIME     NULL,
    CONSTRAINT [PK_OLM_HolidayList] PRIMARY KEY CLUSTERED ([Id] ASC)
);

