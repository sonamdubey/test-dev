CREATE TABLE [dbo].[TC_MMDealersPointLogs] (
    [TC_MMDealersPointLogsId] INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]                INT      NULL,
    [Amount]                  INT      NULL,
    [Points]                  INT      NULL,
    [CreatedOn]               DATETIME CONSTRAINT [DF_TC_MMDealersPointLogs_CreatedOn] DEFAULT (getdate()) NULL,
    [CreatedBy]               INT      NULL,
    CONSTRAINT [PK_TC_MMDealersPointLogs] PRIMARY KEY CLUSTERED ([TC_MMDealersPointLogsId] ASC)
);

