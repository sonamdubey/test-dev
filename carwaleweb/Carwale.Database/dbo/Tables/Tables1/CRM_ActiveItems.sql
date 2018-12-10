CREATE TABLE [dbo].[CRM_ActiveItems] (
    [InterestedInId] NUMERIC (18) NOT NULL,
    [ItemId]         NUMERIC (18) NOT NULL,
    [Priority]       SMALLINT     NOT NULL,
    CONSTRAINT [PK_CNS_ActiveItems] PRIMARY KEY CLUSTERED ([InterestedInId] ASC, [ItemId] ASC) WITH (FILLFACTOR = 90)
);

