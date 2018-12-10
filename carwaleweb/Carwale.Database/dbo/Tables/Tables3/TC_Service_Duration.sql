CREATE TABLE [dbo].[TC_Service_Duration] (
    [Id]          INT     IDENTITY (1, 1) NOT NULL,
    [MakeId]      INT     NULL,
    [ServiceType] TINYINT NULL,
    [Duration]    INT     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

