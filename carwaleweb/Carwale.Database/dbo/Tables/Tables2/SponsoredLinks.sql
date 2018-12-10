CREATE TABLE [dbo].[SponsoredLinks] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [CampaignId]  INT           NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [IsInsideApp] BIT           NULL,
    [Url]         VARCHAR (350) NULL,
    [IsUpcoming]  BIT           NULL,
    [Payload]     VARCHAR (200) NULL,
    [UrlOrder]    SMALLINT      NULL
);

