CREATE TABLE [dbo].[DealerUsedCarViewsLogs] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [LogDate]       VARCHAR (100) NULL,
    [ProfileId]     VARCHAR (100) NULL,
    [BtnSellerView] VARCHAR (100) NULL,
    [DetailView]    VARCHAR (100) NULL,
    [Impression]    VARCHAR (100) NULL,
    [Photoview]     VARCHAR (100) NULL,
    [Response]      VARCHAR (100) NULL,
    [TotalViews]    VARCHAR (100) NULL,
    [EventSource]   VARCHAR (100) NULL
);

