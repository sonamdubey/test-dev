CREATE TABLE [dbo].[OLM_MakeData] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MakeId]    NUMERIC (18) NOT NULL,
    [MonthVal]  DATETIME     NOT NULL,
    [PageViews] NUMERIC (18) NOT NULL,
    [UpdatedOn] DATETIME     CONSTRAINT [DF_OLM_MakeData_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_MakeData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

