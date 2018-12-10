CREATE TABLE [dbo].[Microsite_ContentCatagories] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [CatagoryName] VARCHAR (20) NULL,
    [IsActive]     BIT          CONSTRAINT [DF_Microsite_ContentCatagories_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Microsite_ContentCatagories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

