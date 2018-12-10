CREATE TABLE [dbo].[OLM_ServiceAppointments] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ServiceCenterId] NUMERIC (18)  NOT NULL,
    [FullName]        VARCHAR (150) NOT NULL,
    [Email]           VARCHAR (100) NOT NULL,
    [Mobile]          VARCHAR (15)  NOT NULL,
    [City]            NUMERIC (18)  NOT NULL,
    [PreferredDate]   DATETIME      NULL,
    [VehicleRegNum]   VARCHAR (15)  NULL,
    [VehicleIdnNum]   VARCHAR (20)  NULL,
    [CarKms]          NUMERIC (18)  NULL,
    [Comments]        VARCHAR (500) NULL,
    [IPAddress]       VARCHAR (20)  NULL,
    [IsMailSent]      BIT           NULL,
    [EntryDate]       DATETIME      NULL,
    CONSTRAINT [PK_OLM_ServiceAppointments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

