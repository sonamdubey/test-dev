CREATE TABLE [dbo].[ESM_Users] (
    [id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [UserId]    NUMERIC (18) NOT NULL,
    [CreatedOn] DATETIME     NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_ESM_Users_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ESM_Users] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

