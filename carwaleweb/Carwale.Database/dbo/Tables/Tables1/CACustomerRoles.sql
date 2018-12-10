CREATE TABLE [dbo].[CACustomerRoles] (
    [CustomerId] NUMERIC (18) NOT NULL,
    [RoleId]     INT          NOT NULL,
    CONSTRAINT [PK_CACustomerRoles] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

