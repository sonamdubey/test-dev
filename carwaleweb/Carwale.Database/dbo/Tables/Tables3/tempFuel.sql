CREATE TABLE [dbo].[tempFuel] (
    [Email_Id] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Email]    VARCHAR (100) NULL
);


GO
CREATE CLUSTERED INDEX [IX_tempFuel]
    ON [dbo].[tempFuel]([Email_Id] ASC);

