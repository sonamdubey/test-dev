CREATE TABLE [dbo].[TC_TMAMTargetChangeMaster] (
    [TC_TMAMTargetChangeMasterId] INT            IDENTITY (1, 1) NOT NULL,
    [TC_AMId]                     INT            NULL,
    [Year]                        SMALLINT       NULL,
    [SentForApprovalDate]         DATETIME       NULL,
    [IsActive]                    BIT            NULL,
    [IsAprrovedByRM]              BIT            NULL,
    [IsAprrovedByNSC]             BIT            NULL,
    [RMActionDate]                DATETIME       NULL,
    [NSCActionDate]               DATETIME       NULL,
    [RMId]                        INT            NULL,
    [NSCId]                       INT            NULL,
    [AMComments]                  VARCHAR (1000) NULL,
    [RMComments]                  VARCHAR (1000) NULL,
    [NSCComments]                 VARCHAR (1000) NULL
);

