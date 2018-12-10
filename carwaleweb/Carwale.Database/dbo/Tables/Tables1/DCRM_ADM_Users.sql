CREATE TABLE [dbo].[DCRM_ADM_Users] (
    [UserId]      NUMERIC (18) NOT NULL,
    [UpdatedOn]   DATETIME     NOT NULL,
    [UpdatedBy]   NUMERIC (18) NOT NULL,
    [IsActive]    BIT          CONSTRAINT [DF_DCRM_ADM_Users_IsActive] DEFAULT ((1)) NOT NULL,
    [MappedLevel] SMALLINT     NULL,
    CONSTRAINT [PK_DCRM_ADM_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

