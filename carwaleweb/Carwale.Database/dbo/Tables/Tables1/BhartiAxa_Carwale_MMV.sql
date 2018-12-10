CREATE TABLE [dbo].[BhartiAxa_Carwale_MMV] (
    [CWVersionId] INT          NOT NULL,
    [RefrenceId]  VARCHAR (10) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_BhartiAxa_Carwale_MMV_RefrenceId]
    ON [dbo].[BhartiAxa_Carwale_MMV]([RefrenceId] ASC);

