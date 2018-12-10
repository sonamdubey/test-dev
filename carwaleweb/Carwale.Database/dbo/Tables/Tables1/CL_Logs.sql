CREATE TABLE [dbo].[CL_Logs] (
    [ID]               NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId]       SMALLINT     NOT NULL,
    [ReferenceId]      NUMERIC (18) NOT NULL,
    [StartDateTime]    DATETIME     NOT NULL,
    [AgentId]          VARCHAR (50) NOT NULL,
    [CalledNumber]     VARCHAR (50) NOT NULL,
    [IsOpen]           BIT          CONSTRAINT [DF_CL_Logs_IsOpen] DEFAULT ((1)) NOT NULL,
    [IsCallMatured]    BIT          NULL,
    [EndDateTime]      DATETIME     NULL,
    [DialingTime]      NUMERIC (18) NULL,
    [CallStartTime]    DATETIME     NULL,
    [CallEndTime]      DATETIME     NULL,
    [TalkDuration]     NUMERIC (18) NULL,
    [RecordingId]      VARCHAR (50) NULL,
    [AspectCallId]     VARCHAR (50) NULL,
    [DispositionType]  SMALLINT     NULL,
    [AspectLogId]      NUMERIC (18) NULL,
    [AspectSequenceId] NUMERIC (18) NULL,
    CONSTRAINT [PK_CL_Logs] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

