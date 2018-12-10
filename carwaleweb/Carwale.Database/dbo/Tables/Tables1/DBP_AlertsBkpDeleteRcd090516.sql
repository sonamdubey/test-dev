CREATE TABLE [dbo].[DBP_AlertsBkpDeleteRcd090516] (
    [Id]            NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [DealerId]      NUMERIC (18)    NULL,
    [CityId]        NUMERIC (18)    NULL,
    [MakeId]        NUMERIC (18)    NULL,
    [ModelId]       NUMERIC (18)    NULL,
    [MinPrice]      NUMERIC (18, 2) NULL,
    [MaxPrice]      NUMERIC (18, 2) NULL,
    [MinKm]         NUMERIC (18)    NULL,
    [MaxKm]         NUMERIC (18)    NULL,
    [MinYear]       NUMERIC (18)    NULL,
    [MaxYear]       NUMERIC (18)    NULL,
    [NoOfOwners]    INT             NULL,
    [EntryDateTime] DATETIME        NOT NULL,
    [RegNo]         VARCHAR (4)     NULL
);

