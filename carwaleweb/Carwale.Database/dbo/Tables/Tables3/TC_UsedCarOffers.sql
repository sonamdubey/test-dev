CREATE TABLE [dbo].[TC_UsedCarOffers] (
    [TC_UsedCarOfferId] INT           IDENTITY (1, 1) NOT NULL,
    [BranchId]          INT           NULL,
    [OfferName]         VARCHAR (80)  NULL,
    [StartDate]         DATETIME      NULL,
    [EndDate]           DATETIME      NULL,
    [ModifiedDate]      DATETIME      NULL,
    [OfferAmount]       INT           NULL,
    [Description]       VARCHAR (200) NULL,
    [Terms]             VARCHAR (200) NULL,
    [CreatedDate]       DATETIME      CONSTRAINT [DF_TC_UsedCarOffers_CreatedDate] DEFAULT (getdate()) NULL,
    [IsActive]          BIT           CONSTRAINT [DF_TC_UsedCarOffers_IsActive] DEFAULT ((1)) NULL,
    [ActualEndDate]     DATETIME      NULL,
    CONSTRAINT [PK__TC_UsedC__A0996A547CEA02C2] PRIMARY KEY CLUSTERED ([TC_UsedCarOfferId] ASC)
);

