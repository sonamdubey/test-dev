CREATE TABLE [dbo].[CarVideos] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [InquiryId]     NUMERIC (18) NOT NULL,
    [IsDealer]      BIT          NOT NULL,
    [IsMain]        BIT          NOT NULL,
    [IsActive]      BIT          CONSTRAINT [DF_CarVideos_IsActive] DEFAULT ((1)) NOT NULL,
    [IsApproved]    BIT          CONSTRAINT [DF_CarVideos_IsApproved] DEFAULT ((0)) NOT NULL,
    [Entrydate]     DATETIME     DEFAULT (getdate()) NULL,
    [TC_CarVideoId] BIGINT       NULL,
    [VideoUrl]      VARCHAR (20) NULL,
    CONSTRAINT [PK_CarVideos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'here inquiry id can be from the customersellinq or from the dealers table. if is dealer is 1 then dealer else customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CarVideos', @level2type = N'COLUMN', @level2name = N'InquiryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'For the approval purpose', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CarVideos', @level2type = N'COLUMN', @level2name = N'IsApproved';

