CREATE TABLE [dbo].[WS_ConfirmedEmails] (
    [EMail]        VARCHAR (100) NOT NULL,
    [ResponseDate] DATETIME      CONSTRAINT [DF_WS_ConfirmedEmails_ResponseDate] DEFAULT (getdate()) NOT NULL,
    [CampaignName] VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_WS_ConfirmedEmails] PRIMARY KEY CLUSTERED ([EMail] ASC)
);

