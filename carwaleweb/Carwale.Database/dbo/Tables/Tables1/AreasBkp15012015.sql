CREATE TABLE [dbo].[AreasBkp15012015] (
    [ID]        NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50)    NOT NULL,
    [CityId]    NUMERIC (18)    NOT NULL,
    [PinCode]   VARCHAR (10)    NULL,
    [Lattitude] DECIMAL (18, 6) NULL,
    [Longitude] DECIMAL (18, 6) NULL,
    [IsDeleted] NUMERIC (18)    NOT NULL
);

