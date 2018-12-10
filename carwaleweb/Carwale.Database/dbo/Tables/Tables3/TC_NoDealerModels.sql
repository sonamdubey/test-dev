CREATE TABLE [dbo].[TC_NoDealerModels] (
    [DealerId] INT NOT NULL,
    [ModelId]  INT NOT NULL,
    [Source]   INT CONSTRAINT [df_SourceDefault] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_NoDealerModels] PRIMARY KEY CLUSTERED ([DealerId] ASC, [ModelId] ASC)
);

