CREATE TABLE [dbo].[TC_CustomerDetails_Backup_2] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId]   NUMERIC (18)  NOT NULL,
    [CustomerName] VARCHAR (100) NOT NULL,
    [Email]        VARCHAR (100) NULL,
    [Mobile]       NUMERIC (18)  NULL,
    [IsActive]     BIT           NOT NULL,
    [Address]      VARCHAR (200) NULL,
    [City]         INT           NULL,
    [Pincode]      VARCHAR (50)  NULL,
    [Dob]          DATE          NULL,
    [Anniversary]  DATE          NULL,
    [BranchId]     NUMERIC (18)  NULL,
    [ToBeDeleted]  BIT           CONSTRAINT [DF__TC_Custom__ToBeD__1D16DB32] DEFAULT ((0)) NULL
);

