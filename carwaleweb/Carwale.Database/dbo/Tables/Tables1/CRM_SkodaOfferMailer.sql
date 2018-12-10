CREATE TABLE [dbo].[CRM_SkodaOfferMailer] (
    [EMail]        VARCHAR (100) NOT NULL,
    [Mobile]       VARCHAR (20)  NOT NULL,
    [CityId]       NUMERIC (18)  NOT NULL,
    [VersionId]    NUMERIC (18)  NOT NULL,
    [CustomerName] VARCHAR (50)  NOT NULL,
    [IsResponded]  BIT           CONSTRAINT [DF_CRM_SkodaOfferMailer_IsResponded] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CRM_SkodaOfferMailer] PRIMARY KEY CLUSTERED ([EMail] ASC)
);

