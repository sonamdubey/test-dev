CREATE TABLE [dbo].[TC_CarVideos] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [BranchId]     INT          NULL,
    [StockId]      NUMERIC (18) NOT NULL,
    [IsMain]       BIT          NOT NULL,
    [IsActive]     BIT          CONSTRAINT [DF_TC_CarVideos_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]    DATETIME     CONSTRAINT [DF_TC_CarVideos_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME     NULL,
    [ModifiedBy]   INT          NULL,
    [IsSellerInq]  BIT          CONSTRAINT [DF__TC_CarVid__IsSel__1E4AF68D] DEFAULT ((0)) NULL,
    [StatusId]     SMALLINT     NULL,
    [VideoUrl]     VARCHAR (20) NULL,
    CONSTRAINT [PK_TC_CarVideos] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CarVideos_StockId]
    ON [dbo].[TC_CarVideos]([StockId] ASC);

