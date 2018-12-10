CREATE TABLE [DST].[DrishtiCallDetailS] (
    [DrishtiCallDetailsId] INT          IDENTITY (1, 1) NOT NULL,
    [CallId]               VARCHAR (50) NULL,
    [DialedNo]             VARCHAR (20) NULL,
    [User_ID]              INT          NULL,
    [CallStartDate]        DATETIME     NULL,
    [CallEndDate]          DATETIME     NULL,
    [DisconnectedBy]       VARCHAR (50) NULL,
    [RingingTime]          INT          NULL,
    [TalkTime]             INT          NULL,
    [TotalTime]            INT          NULL,
    [CallStatus]           VARCHAR (50) NULL,
    [CallTypeId]           TINYINT      NULL,
    [SetUpTime]            FLOAT (53)   NULL,
    [IsCallIdUpdated]      BIT          CONSTRAINT [DF_DrishtiCallDetailS_IsCallIdUpdated] DEFAULT ((0)) NOT NULL,
    [SystemDisposition]    VARCHAR (20) NULL,
    [DomainNumber]         VARCHAR (20) NULL,
    CONSTRAINT [PK_DrishtiCallDetailsId] PRIMARY KEY CLUSTERED ([DrishtiCallDetailsId] ASC)
);

