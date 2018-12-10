CREATE TABLE [dbo].[CW_AgeGroup] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MinAge]   INT          NULL,
    [MaxAge]   INT          NULL,
    [Text]     VARCHAR (50) NULL,
    [IsActive] BIT          CONSTRAINT [DF_CW_AgeGroup_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CW_CarAgeGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

