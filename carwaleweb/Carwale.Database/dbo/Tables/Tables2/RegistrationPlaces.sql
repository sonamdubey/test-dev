CREATE TABLE [dbo].[RegistrationPlaces] (
    [ID]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [IsDeleted] BIT          CONSTRAINT [DF_RegistrationPlaces_IsDeleted] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_RegistrationPlaces] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

