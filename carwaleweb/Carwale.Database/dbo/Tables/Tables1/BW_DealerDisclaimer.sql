CREATE TABLE [dbo].[BW_DealerDisclaimer] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]      INT           NOT NULL,
    [BikeVersionId] INT           NULL,
    [Disclaimer]    VARCHAR (MAX) NULL,
    [IsActive]      BIT           CONSTRAINT [DF_BW_DealerDisclaimer_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_BW_DealerDisclaimer] PRIMARY KEY CLUSTERED ([ID] ASC)
);

