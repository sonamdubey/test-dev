CREATE TABLE [dbo].[BA_GroupDetails] (
    [ID]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [GroupId]         BIGINT        NOT NULL,
    [DealerId]        INT           NOT NULL,
    [ContactName]     VARCHAR (100) NULL,
    [IsCarWaleDealer] TINYINT       NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_BA_GroupDetails_IsActive] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BA_GroupDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);

