CREATE TABLE [dbo].[TC_RewardPoints] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Action]          VARCHAR (50)  NULL,
    [Points]          NUMERIC (18)  NULL,
    [IsActive]        BIT           NULL,
    [TC_DealerTypeId] INT           NULL,
    [DependencyCount] NUMERIC (18)  NULL,
    [Description]     VARCHAR (200) NULL
);

