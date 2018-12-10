CREATE TABLE [dbo].[ForumCustomerRoles] (
    [CustomerId] NUMERIC (18) NOT NULL,
    [RoleId]     INT          NOT NULL,
    CONSTRAINT [PK_ForumCustomerRoles] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

