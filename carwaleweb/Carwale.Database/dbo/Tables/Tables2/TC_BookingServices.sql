CREATE TABLE [dbo].[TC_BookingServices] (
    [TC_BookingServices_Id] INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]              INT          NULL,
    [ServiceName]           VARCHAR (50) NULL,
    [IsActive]              BIT          CONSTRAINT [DF_TC_BookingServices_IsActive] DEFAULT ((1)) NULL,
    [EntryDate]             DATETIME     CONSTRAINT [DF_TC_BookingServices_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]          DATETIME     NULL,
    [ModifiedBy]            INT          NULL,
    CONSTRAINT [PK_TC_BookingServices] PRIMARY KEY CLUSTERED ([TC_BookingServices_Id] ASC)
);

