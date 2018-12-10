CREATE TABLE [dbo].[TC_DealersTarget] (
    [TC_DealersTargetId] INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]           INT      NULL,
    [Month]              TINYINT  NULL,
    [Year]               SMALLINT NULL,
    [Target]             INT      NULL,
    [CreatedBy]          INT      NULL,
    [IsDeleted]          BIT      CONSTRAINT [DF_TC_DealersTarget_IsDeleted] DEFAULT ((0)) NULL,
    [TC_TargetTypeId]    SMALLINT NULL,
    [CarVersionId]       INT      NULL,
    PRIMARY KEY CLUSTERED ([TC_DealersTargetId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_DealersTarget_DealerId]
    ON [dbo].[TC_DealersTarget]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_DealersTarget_CarVersionId]
    ON [dbo].[TC_DealersTarget]([CarVersionId] ASC);

