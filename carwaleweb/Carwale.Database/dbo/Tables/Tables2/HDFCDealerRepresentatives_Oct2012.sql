CREATE TABLE [dbo].[HDFCDealerRepresentatives_Oct2012] (
    [DealerId]       NUMERIC (18)  NOT NULL,
    [Representative] VARCHAR (50)  NOT NULL,
    [Mobile]         VARCHAR (10)  NULL,
    [Email]          VARCHAR (100) NULL,
    [EntryDate]      DATETIME      NOT NULL,
    [IsActive]       BIT           NOT NULL
);

