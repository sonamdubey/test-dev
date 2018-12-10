CREATE TABLE [dbo].[Con_TopSellingCars] (
    [ModelId]         INT           NULL,
    [EntryDate]       DATETIME      NULL,
    [HostUrl]         VARCHAR (50)  NULL,
    [ImgPath]         VARCHAR (50)  NULL,
    [Status]          BIT           NULL,
    [SortOrder]       INT           NULL,
    [IsReplicated]    BIT           CONSTRAINT [DF_Con_TopSellingCars_IsReplicated] DEFAULT ((0)) NULL,
    [OriginalImgPath] VARCHAR (150) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_con_topsellingcars_Modelid]
    ON [dbo].[Con_TopSellingCars]([ModelId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_con_topsellingcars_Status]
    ON [dbo].[Con_TopSellingCars]([ImgPath] ASC, [Status] ASC);

