CREATE TABLE [dbo].[OEMRepresentative] (
    [CustomerId] NUMERIC (18) NOT NULL,
    [CarMakeId]  NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_OEMRepresentative] PRIMARY KEY CLUSTERED ([CustomerId] ASC, [CarMakeId] ASC) WITH (FILLFACTOR = 90)
);

