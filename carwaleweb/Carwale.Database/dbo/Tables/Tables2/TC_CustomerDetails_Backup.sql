CREATE TABLE [dbo].[TC_CustomerDetails_Backup] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CustomerName] VARCHAR (100) NOT NULL,
    [Email]        VARCHAR (100) NULL,
    [Mobile]       NUMERIC (18)  NULL,
    [IsActive]     BIT           NOT NULL,
    [Address]      VARCHAR (200) NULL,
    [City]         INT           NULL,
    [Pincode]      VARCHAR (50)  NULL,
    [Dob]          DATE          NULL,
    [Anniversary]  DATE          NULL
);

