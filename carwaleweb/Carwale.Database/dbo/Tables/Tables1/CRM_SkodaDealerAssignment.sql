CREATE TABLE [dbo].[CRM_SkodaDealerAssignment] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PushId]             NUMERIC (18)  NULL,
    [LeadId]             NUMERIC (18)  NOT NULL,
    [TokenId]            VARCHAR (50)  NULL,
    [PushStatus]         VARCHAR (150) NULL,
    [DealerId]           NUMERIC (18)  NOT NULL,
    [Comments]           VARCHAR (500) NULL,
    [StartDate]          DATETIME      CONSTRAINT [DF_CRM_SkodaDealerAssignment_EntryDate] DEFAULT (getdate()) NOT NULL,
    [EndDate]            DATETIME      NULL,
    [Status]             BIT           CONSTRAINT [DF_CRM_SkodaDealerAssignment_Status] DEFAULT ((0)) NULL,
    [IsCityChanged]      BIT           CONSTRAINT [DF_CRM_SkodaDealerAssignment_IsCityChanged] DEFAULT ((0)) NOT NULL,
    [ErrorCode]          VARCHAR (20)  NULL,
    [DMSId]              VARCHAR (15)  NULL,
    [IncomingXMLListing] VARCHAR (MAX) NULL,
    [OutgoingXMLListing] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CRM_SkodaDealerAssignment] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_SkodaDealerAssignment__LeadId]
    ON [dbo].[CRM_SkodaDealerAssignment]([LeadId] ASC)
    INCLUDE([DealerId]);

