CREATE TABLE [dbo].[BA_ImageSize] (
    [ID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [StockImageId] BIGINT         NOT NULL,
    [Image]        VARCHAR (50)   NULL,
    [Dir]          VARCHAR (500)  NULL,
    [HostUrl]      VARCHAR (500)  NULL,
    [StatusId]     TINYINT        NULL,
    [IsReplicated] BIT            CONSTRAINT [DF_BA_ImageSize_IsReplicated] DEFAULT ((0)) NULL,
    [Small]        VARCHAR (1000) NULL,
    [Medium]       VARCHAR (1000) NULL,
    [Large]        VARCHAR (1000) NULL,
    CONSTRAINT [PK_BA_ImageSize] PRIMARY KEY CLUSTERED ([ID] ASC)
);

