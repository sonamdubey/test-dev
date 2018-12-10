CREATE TABLE [dbo].[dbo.MappedBhartiAxa_Carwale_MMV] (
    [CWVersionId] INT          NOT NULL,
    [RefrenceId]  VARCHAR (10) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TBhartiAxa_Carwale_MMV_RefrenceId]
    ON [dbo].[dbo.MappedBhartiAxa_Carwale_MMV]([RefrenceId] ASC);

