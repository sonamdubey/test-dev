CREATE TABLE [dbo].[ComponentProviderDetails] (
    [ComponentProviderId] NUMERIC (18) NOT NULL,
    [ComponentId]         NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ComponentProviderDetails] PRIMARY KEY CLUSTERED ([ComponentProviderId] ASC, [ComponentId] ASC) WITH (FILLFACTOR = 90)
);

