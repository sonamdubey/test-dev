CREATE TABLE [dbo].[TC_Deals_Cities] (
    [TC_Deals_Cities_Id] INT IDENTITY (1, 1) NOT NULL,
    [CitiesId]           INT NOT NULL,
    [IsActive]           BIT CONSTRAINT [DF_TC_Deals_Cities_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_Deals_Cities] PRIMARY KEY CLUSTERED ([TC_Deals_Cities_Id] ASC)
);

