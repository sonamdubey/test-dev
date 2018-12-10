CREATE TABLE [dbo].[BA_GroupSharing] (
    [ID]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [StockId]        BIGINT        NULL,
    [GroupDetailsID] BIGINT        NULL,
    [Url]            VARCHAR (100) NULL,
    [CreatedOn]      DATETIME      NULL,
    [DeletedOn]      DATETIME      NULL,
    [IsClicked]      BIT           CONSTRAINT [DF_BA_GroupSharing_IsClicked] DEFAULT ((0)) NULL,
    [Comments]       VARCHAR (500) NULL,
    CONSTRAINT [PK_BA_GroupSharing] PRIMARY KEY CLUSTERED ([ID] ASC)
);

