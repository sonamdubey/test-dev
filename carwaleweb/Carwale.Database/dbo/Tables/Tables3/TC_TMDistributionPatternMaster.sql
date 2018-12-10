CREATE TABLE [dbo].[TC_TMDistributionPatternMaster] (
    [TC_TMDistributionPatternMasterId] INT          IDENTITY (1, 1) NOT NULL,
    [LegacyName]                       VARCHAR (50) NULL,
    [IsDataHistorical]                 BIT          NULL,
    [LegacyYear]                       SMALLINT     NULL,
    [CreatedOn]                        DATETIME     NULL,
    [CreatedBy]                        INT          NULL,
    [IsTargetApplied]                  BIT          NULL,
    [IsActive]                         BIT          NULL,
    [MakeId]                           INT          CONSTRAINT [TC_TMDistributionPatternMaster_MakeId] DEFAULT ((20)) NULL,
    CONSTRAINT [PK_TC_TMLegacyMasterId] PRIMARY KEY NONCLUSTERED ([TC_TMDistributionPatternMasterId] ASC)
);

