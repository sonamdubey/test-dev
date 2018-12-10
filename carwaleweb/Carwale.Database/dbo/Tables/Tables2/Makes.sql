CREATE TABLE [dbo].[Makes] (
    [makeID]  INT           NOT NULL,
    [Make]    NVARCHAR (50) NULL,
    [deleted] BIT           NOT NULL,
    CONSTRAINT [PK_Makes] PRIMARY KEY CLUSTERED ([makeID] ASC) WITH (FILLFACTOR = 90)
);

