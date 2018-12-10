CREATE TABLE [dbo].[DD_DealerOutletAddress] (
    [Id]                 NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DD_DealerOutletsId] NUMERIC (18) NOT NULL,
    [DD_AddressesId]     NUMERIC (18) NOT NULL,
    [CreatedOn]          DATETIME     NOT NULL,
    [CreatedBy]          NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DD_DealerOutletAddress] PRIMARY KEY CLUSTERED ([DD_DealerOutletsId] ASC)
);

