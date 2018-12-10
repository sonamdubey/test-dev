CREATE TABLE [dbo].[ESM_Inventory] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [ProposalId]    INT           NULL,
    [AdFor]         VARCHAR (30)  NULL,
    [TargetedCar]   INT           NULL,
    [Placement]     INT           NULL,
    [AdUnit]        SMALLINT      NULL,
    [Comment]       VARCHAR (250) NULL,
    [UpdatedBy]     INT           NULL,
    [LastUpdatedOn] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

