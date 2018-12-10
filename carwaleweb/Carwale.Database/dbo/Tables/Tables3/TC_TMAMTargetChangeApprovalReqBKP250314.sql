CREATE TABLE [dbo].[TC_TMAMTargetChangeApprovalReqBKP250314] (
    [TC_TMAMTargetChangeApprovalReqId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_DealersTargetId]               INT      NOT NULL,
    [DealerId]                         INT      NULL,
    [Month]                            TINYINT  NULL,
    [Year]                             SMALLINT NULL,
    [Target]                           INT      NULL,
    [CreatedBy]                        INT      NULL,
    [IsDeleted]                        BIT      NULL,
    [TC_TargetTypeId]                  SMALLINT NULL,
    [CarVersionId]                     INT      NULL,
    [TC_AMId]                          INT      NULL
);

