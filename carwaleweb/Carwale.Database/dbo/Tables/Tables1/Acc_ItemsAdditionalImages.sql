CREATE TABLE [dbo].[Acc_ItemsAdditionalImages] (
    [Id]                     NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ItemId]                 NUMERIC (18)  NOT NULL,
    [AddionalImagesLocation] VARCHAR (50)  NOT NULL,
    [IsActive]               BIT           CONSTRAINT [DF_Accessories_ItemsAdditionalImages_IsActive] DEFAULT ((1)) NOT NULL,
    [IsReplicated]           BIT           CONSTRAINT [DF__Acc_Items__IsRep__6097DCD7] DEFAULT ((0)) NULL,
    [HostURL]                VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_Accessories_ItemsAdditionalImages] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

