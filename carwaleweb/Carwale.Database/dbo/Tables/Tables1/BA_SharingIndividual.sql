CREATE TABLE [dbo].[BA_SharingIndividual] (
    [ID]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [StockId]         BIGINT        NOT NULL,
    [DealerId]        INT           NOT NULL,
    [Url]             VARCHAR (100) NULL,
    [CreatedOn]       DATETIME      NULL,
    [ModifiedOn]      DATETIME      NULL,
    [IsCarwaleDealer] BIT           CONSTRAINT [DF_BA_SharingIndividual_IsCarwaleDealer] DEFAULT ((0)) NOT NULL,
    [IsClicked]       BIT           CONSTRAINT [DF_BA_SharingIndividual_IsClicked] DEFAULT ((0)) NULL,
    [Comments]        VARCHAR (500) NULL,
    CONSTRAINT [PK_BA_SharingIndividual] PRIMARY KEY CLUSTERED ([ID] ASC)
);

