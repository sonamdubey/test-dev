CREATE TABLE [dbo].[TC_SyndicationWebsite] (
    [TC_SyndicationWebsiteId] SMALLINT     NOT NULL,
    [WebsiteName]             VARCHAR (50) NOT NULL,
    [IsActive]                BIT          CONSTRAINT [DF_TC_SyndicationWebsite_IsActive] DEFAULT ((1)) NOT NULL,
    [WebsiteFileName]         VARCHAR (50) NULL,
    [WebsiteIP]               VARCHAR (50) NULL,
    [IsTesting]               BIT          CONSTRAINT [DF_TC_SyndicationWebsite_IsTesting] DEFAULT ((1)) NULL,
    [SourceId]                INT          NULL,
    [IsVerificationRequired]  BIT          NULL,
    CONSTRAINT [PK_TC_SyndicationWebsite] PRIMARY KEY CLUSTERED ([TC_SyndicationWebsiteId] ASC)
);

