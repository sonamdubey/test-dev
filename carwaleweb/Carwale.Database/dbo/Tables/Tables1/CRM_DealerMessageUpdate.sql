CREATE TABLE [dbo].[CRM_DealerMessageUpdate] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SMSText]        VARCHAR (100) NULL,
    [SMSKey]         VARCHAR (10)  NULL,
    [CustomerMobile] VARCHAR (20)  NULL,
    [Message]        VARCHAR (50)  NULL,
    [DealerMobile]   VARCHAR (20)  NULL,
    [MessageDate]    DATETIME      CONSTRAINT [DF_CRM_DealerMessageUpdate_MessageDate] DEFAULT (getdate()) NOT NULL,
    [IsApproved]     BIT           CONSTRAINT [DF_CRM_DealerMessageUpdate_IsApproved] DEFAULT ((0)) NOT NULL,
    [UpdatedBy]      NUMERIC (18)  NULL,
    [UpdatedOn]      DATETIME      NULL,
    CONSTRAINT [PK_CRM_DealerMessageUpdate] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

