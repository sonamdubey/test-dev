CREATE TABLE [dbo].[Microsite_DealerPages] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [PageType] VARCHAR (20) NOT NULL,
    [IsActive] BIT          DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

