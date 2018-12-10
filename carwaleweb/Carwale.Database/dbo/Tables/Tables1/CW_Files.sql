CREATE TABLE [dbo].[CW_Files] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [CategoryId]   TINYINT       NOT NULL,
    [ItemId]       INT           NOT NULL,
    [OriginalPath] VARCHAR (250) NULL,
    [CreatedOn]    DATETIME      NOT NULL,
    [UploadedOn]   DATETIME      NULL,
    [IsUploaded]   BIT           NOT NULL,
    [UpdatedBy]    INT           NULL,
    [CreatedBy]    INT           NULL,
    CONSTRAINT [PK_CW_Files] PRIMARY KEY CLUSTERED ([Id] ASC)
);

