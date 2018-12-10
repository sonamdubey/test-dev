CREATE TABLE [dbo].[Microsite_DealerEvents] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]         INT           NULL,
    [EventName]        VARCHAR (300) NULL,
    [EventStartDate]   DATETIME      NULL,
    [EventEndDate]     DATETIME      NULL,
    [EventDescription] VARCHAR (500) NULL,
    [EventVenue]       VARCHAR (50)  NULL,
    [CreatedOn]        DATETIME      CONSTRAINT [DF_Microsite_DealerEvents_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]        DATETIME      NULL,
    [IsActive]         BIT           CONSTRAINT [DF_Microsite_DealerEvents_IsActive] DEFAULT ((0)) NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_Microsite_DealerEvents_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Microsite_DealerEvents] PRIMARY KEY CLUSTERED ([Id] ASC)
);

