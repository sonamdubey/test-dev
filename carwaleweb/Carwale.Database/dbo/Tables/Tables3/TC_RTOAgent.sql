CREATE TABLE [dbo].[TC_RTOAgent] (
    [TC_RTOAgent_Id] INT          IDENTITY (1, 1) NOT NULL,
    [TC_RTO_Id]      INT          NOT NULL,
    [AgentName]      VARCHAR (50) NOT NULL,
    [IsActive]       BIT          CONSTRAINT [DF_TC_RTOAgent_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]      DATETIME     CONSTRAINT [DF_TC_RTOAgent_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]   DATETIME     NULL,
    [ModifiedBy]     INT          NULL,
    CONSTRAINT [PK_TC_RTOAgent] PRIMARY KEY CLUSTERED ([TC_RTOAgent_Id] ASC)
);

