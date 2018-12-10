CREATE TABLE [dbo].[ICB_Dealers] (
    [DealerId]  NUMERIC (18) NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_ICB_Dealers_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_ICB_Dealers_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ICB_Dealers] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90)
);

