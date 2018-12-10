CREATE TABLE [dbo].[DCRM_ADM_UserRegions] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UserId]    NUMERIC (18) NOT NULL,
    [RegionId]  INT          NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_DCRM_ADM_UserRegions_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DCRM_ADM_UserRegions_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_DCRM_ADM_UserRegions_UserId]
    ON [dbo].[DCRM_ADM_UserRegions]([UserId] ASC);

