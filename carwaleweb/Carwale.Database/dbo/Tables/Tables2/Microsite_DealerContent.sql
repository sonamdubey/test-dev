CREATE TABLE [dbo].[Microsite_DealerContent] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]             INT           NOT NULL,
    [ContentCatagoryId]    INT           NOT NULL,
    [DealerContent]        VARCHAR (MAX) NOT NULL,
    [IsActive]             BIT           CONSTRAINT [DF_Microsite_DealerContent_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]            DATETIME      NOT NULL,
    [ContentTitle]         VARCHAR (200) NULL,
    [ContentSubCatagoryId] INT           NULL,
    CONSTRAINT [PK_Microsite_DealerContent] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_Microsite_DealerContent_DealerId]
    ON [dbo].[Microsite_DealerContent]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Microsite_DealerContent_ContentCatagoryId]
    ON [dbo].[Microsite_DealerContent]([DealerId] ASC, [ContentCatagoryId] ASC, [IsActive] ASC, [ContentSubCatagoryId] ASC);

