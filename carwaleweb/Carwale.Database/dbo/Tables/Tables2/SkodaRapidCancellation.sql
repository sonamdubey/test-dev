CREATE TABLE [dbo].[SkodaRapidCancellation] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (100) NOT NULL,
    [Mobile]           VARCHAR (20)  NOT NULL,
    [CityID]           INT           NOT NULL,
    [Email]            VARCHAR (250) NOT NULL,
    [CarWaleBookingId] VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_SkodaRapidCancellation] PRIMARY KEY CLUSTERED ([ID] ASC)
);

