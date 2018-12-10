CREATE TABLE [dbo].[NCS_RMDealers] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [RMId]        NUMERIC (18) NULL,
    [DealerId]    NUMERIC (18) NULL,
    [IsExecutive] BIT          CONSTRAINT [DF_NCS_RMDealers_IsExecutive] DEFAULT ((0)) NOT NULL,
    [CreatedOn]   DATETIME     CONSTRAINT [DF_NCS_RMDealers_CreatedOn] DEFAULT (getdate()) NULL,
    [Type]        SMALLINT     NULL,
    CONSTRAINT [PK_NCS_RMDealers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'OEMHierarchy = 0,CarwaleExecutive=1,BranchHead=2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCS_RMDealers', @level2type = N'COLUMN', @level2name = N'Type';

