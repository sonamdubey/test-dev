CREATE TABLE [dbo].[LDDealers] (
    [ID]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LDTakerId]      INT           NOT NULL,
    [DealerId]       NUMERIC (18)  NOT NULL,
    [DealershipName] VARCHAR (150) NULL,
    [ContactPerson]  VARCHAR (150) NULL,
    [Address1]       VARCHAR (200) NULL,
    [Address2]       VARCHAR (200) NULL,
    [Address3]       VARCHAR (200) NULL,
    [Address4]       VARCHAR (200) NULL,
    [CityId]         NUMERIC (18)  NOT NULL,
    [PhoneNumber]    VARCHAR (200) NULL,
    [FaxNo]          VARCHAR (50)  NULL,
    [Email]          VARCHAR (150) NULL,
    CONSTRAINT [PK_LDDealers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

