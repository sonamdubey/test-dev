CREATE TABLE [dbo].[PQPageIds_MasterTblFrom3rdNov2015] (
    [PageURL]  VARCHAR (MAX) NOT NULL,
    [PQPageID] INT           NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_PQPageIds_MasterTbl]
    ON [dbo].[PQPageIds_MasterTblFrom3rdNov2015]([PQPageID] ASC);

