CREATE TABLE [dbo].[StateRTOCities] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [StateRTOCodeId] INT          NOT NULL,
    [CityName]       VARCHAR (50) NOT NULL,
    [RTONo]          VARCHAR (5)  NOT NULL,
    CONSTRAINT [PK_StateRTOCities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

