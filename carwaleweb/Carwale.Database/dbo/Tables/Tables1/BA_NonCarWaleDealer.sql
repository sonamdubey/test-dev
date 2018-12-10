CREATE TABLE [dbo].[BA_NonCarWaleDealer] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Mobile]      BIGINT        NOT NULL,
    [Name]        VARCHAR (50)  NULL,
    [Address]     VARCHAR (500) NULL,
    [CreatedDate] DATETIME      NULL,
    [ModifyDate]  DATETIME      NULL,
    [IsActive]    BIT           CONSTRAINT [DF_BA_NonCarWaleDealer_IsActive] DEFAULT ((1)) NOT NULL,
    [BrokerId]    INT           NULL,
    CONSTRAINT [PK_BA_NonCarWaleDealer] PRIMARY KEY CLUSTERED ([ID] ASC)
);

