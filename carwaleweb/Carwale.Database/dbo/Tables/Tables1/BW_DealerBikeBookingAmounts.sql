CREATE TABLE [dbo].[BW_DealerBikeBookingAmounts] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [DealerId]  INT NOT NULL,
    [VersionId] INT NOT NULL,
    [Amount]    INT NULL,
    [IsActive]  BIT DEFAULT ((1)) NOT NULL
);

