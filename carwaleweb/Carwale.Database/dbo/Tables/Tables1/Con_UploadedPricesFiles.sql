CREATE TABLE [dbo].[Con_UploadedPricesFiles] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [MakeId]       NUMERIC (18)  NOT NULL,
    [ModelId]      NUMERIC (18)  NOT NULL,
    [StateId]      NUMERIC (18)  NOT NULL,
    [CityId]       NUMERIC (18)  NOT NULL,
    [UploadedBy]   VARCHAR (100) NOT NULL,
    [UploadedDate] DATETIME      CONSTRAINT [DF_Con_UploadedPricesFiles_UploadedDate] DEFAULT (getdate()) NOT NULL,
    [FileName]     VARCHAR (100) NOT NULL,
    [HostUrl]      VARCHAR (100) NOT NULL
);

