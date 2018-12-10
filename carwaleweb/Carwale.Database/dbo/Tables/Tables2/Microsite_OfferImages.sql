CREATE TABLE [dbo].[Microsite_OfferImages] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [OfferId]         INT           NULL,
    [HostUrl]         VARCHAR (50)  NULL,
    [OriginalImgPath] VARCHAR (300) NULL,
    [IsActive]        BIT           NULL,
    [EntryDate]       DATETIME      CONSTRAINT [EntryD_Constraint] DEFAULT (getdate()) NULL,
    [ModifiedDate]    DATETIME      NULL,
    [ImgType]         SMALLINT      NULL,
    CONSTRAINT [PK__Microsit__3214EC0720A6A99D] PRIMARY KEY CLUSTERED ([Id] ASC)
);

