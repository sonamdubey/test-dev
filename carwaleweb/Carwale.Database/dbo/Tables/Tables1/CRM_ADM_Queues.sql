CREATE TABLE [dbo].[CRM_ADM_Queues] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (150) NOT NULL,
    [Size]          INT           CONSTRAINT [DF_CRM_ADM_Queues_Size] DEFAULT ((0)) NULL,
    [IsActive]      BIT           NOT NULL,
    [CreatedOn]     DATETIME      NOT NULL,
    [UpdatedOn]     DATETIME      NOT NULL,
    [UpdatedBy]     NUMERIC (18)  NOT NULL,
    [LastIndex]     NUMERIC (18)  CONSTRAINT [DF_CRM_ADM_Queues_LastIndex] DEFAULT ((-1)) NOT NULL,
    [Rank]          SMALLINT      CONSTRAINT [DF_CRM_ADM_Queues_Rank] DEFAULT ((1)) NOT NULL,
    [AcceptNewLead] BIT           CONSTRAINT [DF_CRM_ADM_Queues_AcceptNewLead] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CRM_ADM_Queues] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

