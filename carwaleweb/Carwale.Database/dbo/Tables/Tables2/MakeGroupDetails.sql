CREATE TABLE [dbo].[MakeGroupDetails] (
    [ID]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MakeGroupId] NUMERIC (18) NULL,
    [MakeId]      NUMERIC (18) NULL,
    [IsDeleted]   BIT          CONSTRAINT [DF_MakeGroupDetails_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_MakeGroupDetails] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

