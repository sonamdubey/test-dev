CREATE TABLE [dbo].[Microsite_DealerConversionDetails] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT          NOT NULL,
    [ConversionId]    INT          NOT NULL,
    [ConversionLabel] VARCHAR (50) NOT NULL,
    [IsActive]        BIT          DEFAULT ((1)) NULL,
    [UserId]          INT          NOT NULL,
    [PageId]          INT          NOT NULL,
    [EntryDate]       DATETIME     DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]    DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

