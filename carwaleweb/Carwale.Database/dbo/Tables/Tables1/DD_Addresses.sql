CREATE TABLE [dbo].[DD_Addresses] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Address]          VARCHAR (500) NOT NULL,
    [DD_DealerNamesId] INT           NOT NULL,
    [CityId]           NUMERIC (18)  NOT NULL,
    [AreaId]           NUMERIC (18)  NOT NULL,
    [Pincode]          VARCHAR (7)   NOT NULL,
    [Latitude]         FLOAT (53)    NULL,
    [Longitude]        FLOAT (53)    NULL,
    [CreatedOn]        DATETIME      NOT NULL,
    [CreatedBy]        INT           NOT NULL,
    CONSTRAINT [PK_DD_Addresses] PRIMARY KEY CLUSTERED ([Id] ASC)
);

