CREATE TABLE [dbo].[BA_Stock] (
    [ID]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [BrokerId]       BIGINT        NOT NULL,
    [Kms]            VARCHAR (50)  NULL,
    [Color]          VARCHAR (20)  NULL,
    [OwnerTypeId]    INT           NULL,
    [TransmissionId] INT           NULL,
    [FuelTypeId]     INT           NULL,
    [Comments]       VARCHAR (500) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_BA.Stock_IsActive] DEFAULT ((1)) NOT NULL,
    [PageView]       INT           CONSTRAINT [DF_BA_Stock_PageView] DEFAULT ((0)) NULL,
    [Price]          VARCHAR (20)  NULL,
    CONSTRAINT [PK_BA.Stock] PRIMARY KEY CLUSTERED ([ID] ASC)
);

