CREATE TABLE [dbo].[ICB_Cities] (
    [CityId]    NUMERIC (18) NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_ICB_Cities_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_ICB_Cities_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ICB_Cities] PRIMARY KEY CLUSTERED ([CityId] ASC) WITH (FILLFACTOR = 90)
);

