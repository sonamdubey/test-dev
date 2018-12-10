CREATE TABLE [CRM].[MapFLCUserTeam] (
    [Id]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [FLCUserId] NUMERIC (18) NOT NULL,
    [TeamId]    NUMERIC (18) NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_MapFLCUserTeam_IsActive] DEFAULT ((0)) NOT NULL,
    [CreatedBy] NUMERIC (18) NOT NULL,
    [CreatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NULL,
    [UpdatedOn] DATETIME     NULL,
    CONSTRAINT [PK_MapFLCUserTeam] PRIMARY KEY CLUSTERED ([Id] ASC)
);

