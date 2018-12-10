CREATE TABLE [dbo].[TC_TMTargetCopyData] (
    [TC_TMTargetCopyDataId]            INT  IDENTITY (1, 1) NOT NULL,
    [TC_SpecialUsersId]                INT  NULL,
    [CopyFrom]                         INT  NULL,
    [CopyTo]                           INT  NULL,
    [StartMonth]                       INT  NULL,
    [TargetValue]                      INT  NULL,
    [EntryDate]                        DATE NULL,
    [IsDealer]                         BIT  NULL,
    [TC_TMDistributionPatternMasterId] INT  NULL,
    PRIMARY KEY CLUSTERED ([TC_TMTargetCopyDataId] ASC)
);

