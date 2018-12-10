CREATE TABLE [dbo].[Absure_Dailytracker] (
    [Trackerid]                     INT      IDENTITY (1, 1) NOT NULL,
    [TrackerDate]                   DATE     CONSTRAINT [DF__Absure_Da__Track__32338A57] DEFAULT (getdate()-(1)) NOT NULL,
    [InspectionRequests]            INT      NULL,
    [Allocations]                   INT      NULL,
    [InspectionsDone]               INT      NULL,
    [EligibleforWarranty]           INT      NULL,
    [WarrantiesApproved]            INT      NULL,
    [PendingForAllocation]          INT      NULL,
    [PendingForInspection]          INT      NULL,
    [InspectionsRequestedBefore4pm] INT      NULL,
    [PendingForInspection4pm]       INT      NULL,
    [InspectionRequestsMTD]         INT      NULL,
    [AllocationsMTD]                INT      NULL,
    [InspectionsDoneMTD]            INT      NULL,
    [EligibleforWarrantyMTD]        INT      NULL,
    [WarrantiesApprovedMTD]         INT      NULL,
    [Createdon]                     DATETIME DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([TrackerDate] ASC)
);

