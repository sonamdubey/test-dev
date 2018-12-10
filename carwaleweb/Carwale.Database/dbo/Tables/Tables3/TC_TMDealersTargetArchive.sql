CREATE TABLE [dbo].[TC_TMDealersTargetArchive] (
    [TC_DealersTargetId] INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]           INT      NULL,
    [Month]              TINYINT  NULL,
    [Year]               SMALLINT NULL,
    [Target]             INT      NULL,
    [CreatedBy]          INT      NULL,
    [IsDeleted]          BIT      NULL,
    [TC_TargetTypeId]    SMALLINT NULL,
    [CarVersionId]       INT      NULL,
    [TC_SpecialUsersId]  INT      NULL,
    [CreatedOn]          DATETIME NULL
);

