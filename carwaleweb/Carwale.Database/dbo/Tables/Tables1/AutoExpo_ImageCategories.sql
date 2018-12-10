CREATE TABLE [dbo].[AutoExpo_ImageCategories] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [ImageType] VARCHAR (50) NULL,
    [IsActive]  BIT          CONSTRAINT [DF_AutoExpo_ImageCategories_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_AutoExpo_ImageCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

