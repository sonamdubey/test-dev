CREATE TABLE [dbo].[AxisBank_UserSecurityKey] (
    [UserId]  NUMERIC (18) NOT NULL,
    [UserKey] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AxisBank_UserSecurityKey] PRIMARY KEY CLUSTERED ([UserId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [IX_AxisBank_UserSecurityKey] UNIQUE NONCLUSTERED ([UserKey] ASC) WITH (FILLFACTOR = 90)
);

