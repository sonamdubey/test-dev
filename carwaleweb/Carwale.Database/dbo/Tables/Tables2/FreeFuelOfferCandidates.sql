CREATE TABLE [dbo].[FreeFuelOfferCandidates] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (80)  NOT NULL,
    [Email]             VARCHAR (100) NOT NULL,
    [Mobile]            VARCHAR (15)  NOT NULL,
    [CWGTDPolicyNo]     VARCHAR (50)  NOT NULL,
    [ReasonBuyingCWGTD] VARCHAR (160) NOT NULL,
    [EntryDateTime]     DATETIME      CONSTRAINT [DF_FreeFuelOfferCandidates_EntryDateTime] DEFAULT (getdate()) NOT NULL,
    [Iswinner]          BIT           NOT NULL,
    [WinningDate]       DATETIME      NULL,
    [City]              VARCHAR (50)  NULL
);

