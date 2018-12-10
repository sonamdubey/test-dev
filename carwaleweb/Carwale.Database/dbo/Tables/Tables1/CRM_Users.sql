CREATE TABLE [dbo].[CRM_Users] (
    [UserId]    NUMERIC (18) NOT NULL,
    [CreatedOn] DATETIME     NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_CRM_Users_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CNS_Users] PRIMARY KEY CLUSTERED ([UserId] ASC) WITH (FILLFACTOR = 90)
);

