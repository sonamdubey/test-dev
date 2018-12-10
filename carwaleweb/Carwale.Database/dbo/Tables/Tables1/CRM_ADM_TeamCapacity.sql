CREATE TABLE [dbo].[CRM_ADM_TeamCapacity] (
    [ID]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MTeamId]       NUMERIC (18) NOT NULL,
    [UserCount]     NUMERIC (18) NOT NULL,
    [Capacity]      NUMERIC (18) NOT NULL,
    [TotalCapacity] NUMERIC (18) NOT NULL,
    [UpdatedOn]     DATETIME     NOT NULL,
    [UpdatedBy]     NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_ADM_TeamCapacity] PRIMARY KEY CLUSTERED ([ID] ASC)
);

