CREATE TABLE [dbo].[DCRM_ADM_UserDealers] (
    [UserId]    NUMERIC (18) NOT NULL,
    [RoleId]    NUMERIC (18) NOT NULL,
    [DealerId]  NUMERIC (18) NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DCRM_ADM_UserDealers_1] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC, [DealerId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_DCRM_ADM_UserDealers_RoleId]
    ON [dbo].[DCRM_ADM_UserDealers]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [DCRM_ADM_UserDealers_DealerId_Userid]
    ON [dbo].[DCRM_ADM_UserDealers]([DealerId] ASC, [UserId] ASC);

